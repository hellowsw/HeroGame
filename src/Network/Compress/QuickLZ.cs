// QuickLZ data compression library
// Copyright (C) 2006-2011 Lasse Mikkel Reinhold
// lar@quicklz.com
//
// QuickLZ can be used for free under the GPL 1, 2 or 3 license (where anything 
// released into public must be open source) or under a commercial license if such 
// has been acquired (see http://www.quicklz.com/order.html). The commercial license 
// does not cover derived or ported versions created by third parties under GPL.
//
// Only a subset of the C library has been ported, namely level 1 and 3 not in 
// streaming mode. 
//
// Version: 1.5.0 final

using System;

static class QuickLZ
{
    public const int QLZ_VERSION_MAJOR = 1;
    public const int QLZ_VERSION_MINOR = 5;
    public const int QLZ_VERSION_REVISION = 0;

    // Streaming mode not supported
    public const int QLZ_STREAMING_BUFFER = 0;

    // Bounds checking not supported  Use try...catch instead
    public const int QLZ_MEMORY_SAFE = 0;

    // Decrease QLZ_POINTERS_3 to increase level 3 compression speed. Do not edit any other values!
    private const int HASH_VALUES = 4096;
    private const int MINOFFSET = 2;
    private const int UNCONDITIONAL_MATCHLEN = 6;
    private const int UNCOMPRESSED_END = 4;
    private const int CWORD_LEN = 4;
    public const int DEFAULT_HEADERLEN = 9;
    private const int QLZ_POINTERS_1 = 1;
    private const int QLZ_POINTERS_3 = 16;

    /************************************************************************/
    /*                                                                      */
    /************************************************************************/
    private static int GetHeaderLen(byte[] source, int offset)
    {
        return ((source[offset] & 2) == 2) ? 9 : 3;
    }

    public static int GetUncompressedLength(byte[] source, int offset)
    {
        if (GetHeaderLen(source, offset) == 9)
            return source[offset + 5] | (source[offset + 6] << 8) | (source[offset + 7] << 16) | (source[offset + 8] << 24);
        else
            return source[offset + 2];
    }

    private static void write_header(byte[] dst, int level, bool compressible, int size_compressed, int size_decompressed)
    {
        dst[0] = (byte)(2 | (compressible ? 1 : 0));
        dst[0] |= (byte)(level << 2);
        dst[0] |= (1 << 6);
        dst[0] |= (0 << 4);
        fast_write(dst, 1, size_decompressed, 4);
        fast_write(dst, 5, size_compressed, 4);
    }

    public static int CompressLevel = 1;
    public static int Compress(byte[] source, int srcOffset, int srcSize, byte[] outDest, int outOffset)
    {
        int level = CompressLevel;
        int src = 0;
        int dst = DEFAULT_HEADERLEN + CWORD_LEN;
        uint cword_val = 0x80000000;
        int cword_ptr = DEFAULT_HEADERLEN;
        byte[] destination = outDest;
        int[,] hashtable;
        int[] cachetable = new int[HASH_VALUES];
        byte[] hash_counter = new byte[HASH_VALUES];

        int fetch = 0;
        int last_matchstart = (srcSize - UNCONDITIONAL_MATCHLEN - UNCOMPRESSED_END - 1);
        int lits = 0;

        if (level != 1 && level != 3)
            throw new ArgumentException("C# version only supports level 1 and 3");

        if (level == 1)
            hashtable = new int[HASH_VALUES, QLZ_POINTERS_1];
        else
            hashtable = new int[HASH_VALUES, QLZ_POINTERS_3];

        if (srcSize == 0)
            return 0;

        if (src <= last_matchstart)
            fetch = source[srcOffset + src] | (source[srcOffset + src + 1] << 8) | (source[srcOffset + src + 2] << 16);

        while (src <= last_matchstart)
        {
            if ((cword_val & 1) == 1)
            {
                if (src > srcSize >> 1 && dst > src - (src >> 5))
                {
                    return 0;
                }

                fast_write(destination, cword_ptr, (int)((cword_val >> 1) | 0x80000000), 4);
                cword_ptr = dst;
                dst += CWORD_LEN;
                cword_val = 0x80000000;
            }

            if (level == 1)
            {
                int hash = ((fetch >> 12) ^ fetch) & (HASH_VALUES - 1);
                int o = hashtable[hash, 0];
                int cache = cachetable[hash] ^ fetch;
                cachetable[hash] = fetch;
                hashtable[hash, 0] = src;

                if (cache == 0 && hash_counter[hash] != 0 && (src - o > MINOFFSET || (src == o + 1 && lits >= 3 && src > 3 && source[srcOffset + src] == source[srcOffset + src - 3] && source[srcOffset + src] == source[srcOffset + src - 2] && source[srcOffset + src] == source[srcOffset + src - 1] && source[srcOffset + src] == source[srcOffset + src + 1] && source[srcOffset + src] == source[srcOffset + src + 2])))
                {
                    cword_val = ((cword_val >> 1) | 0x80000000);
                    if (source[srcOffset + o + 3] != source[srcOffset + src + 3])
                    {
                        int f = 3 - 2 | (hash << 4);
                        destination[dst + 0] = (byte)(f >> 0 * 8);
                        destination[dst + 1] = (byte)(f >> 1 * 8);
                        src += 3;
                        dst += 2;
                    }
                    else
                    {
                        int old_src = src;
                        int remaining = ((srcSize - UNCOMPRESSED_END - src + 1 - 1) > 255 ? 255 : (srcSize - UNCOMPRESSED_END - src + 1 - 1));

                        src += 4;
                        if (source[srcOffset + o + src - old_src] == source[srcOffset + src])
                        {
                            src++;
                            if (source[srcOffset + o + src - old_src] == source[srcOffset + src])
                            {
                                src++;
                                while (source[srcOffset + o + (src - old_src)] == source[srcOffset + src] && (src - old_src) < remaining)
                                    src++;
                            }
                        }

                        int matchlen = src - old_src;

                        hash <<= 4;
                        if (matchlen < 18)
                        {
                            int f = (hash | (matchlen - 2));
                            destination[dst + 0] = (byte)(f >> 0 * 8);
                            destination[dst + 1] = (byte)(f >> 1 * 8);
                            dst += 2;
                        }
                        else
                        {
                            fast_write(destination, dst, hash | (matchlen << 16), 3);
                            dst += 3;
                        }
                    }
                    fetch = source[srcOffset + src] | (source[srcOffset + src + 1] << 8) | (source[srcOffset + src + 2] << 16);
                    lits = 0;
                }
                else
                {
                    lits++;
                    hash_counter[hash] = 1;
                    destination[dst] = source[srcOffset + src];
                    cword_val = (cword_val >> 1);
                    src++;
                    dst++;
                    fetch = ((fetch >> 8) & 0xffff) | (source[srcOffset + src + 2] << 16);
                }

            }
            else
            {
                fetch = source[srcOffset + src] | (source[srcOffset + src + 1] << 8) | (source[srcOffset + src + 2] << 16);

                int o, offset2;
                int matchlen, k, m/*, best_k*/ = 0;
                byte c;
                int remaining = ((srcSize - UNCOMPRESSED_END - src + 1 - 1) > 255 ? 255 : (srcSize - UNCOMPRESSED_END - src + 1 - 1));
                int hash = ((fetch >> 12) ^ fetch) & (HASH_VALUES - 1);

                c = hash_counter[hash];
                matchlen = 0;
                offset2 = 0;
                for (k = 0; k < QLZ_POINTERS_3 && c > k; k++)
                {
                    o = hashtable[hash, k];
                    if ((byte)fetch == source[srcOffset + o] && (byte)(fetch >> 8) == source[srcOffset + o + 1] && (byte)(fetch >> 16) == source[srcOffset + o + 2] && o < src - MINOFFSET)
                    {
                        m = 3;
                        while (source[srcOffset + o + m] == source[srcOffset + src + m] && m < remaining)
                            m++;
                        if ((m > matchlen) || (m == matchlen && o > offset2))
                        {
                            offset2 = o;
                            matchlen = m;
                            /*best_k = k;*/
                        }
                    }
                }
                o = offset2;
                hashtable[hash, c & (QLZ_POINTERS_3 - 1)] = src;
                c++;
                hash_counter[hash] = c;

                if (matchlen >= 3 && src - o < 131071)
                {
                    int offset = src - o;

                    for (int u = 1; u < matchlen; u++)
                    {
                        fetch = source[srcOffset + src + u] | (source[srcOffset + src + u + 1] << 8) | (source[srcOffset + src + u + 2] << 16);
                        hash = ((fetch >> 12) ^ fetch) & (HASH_VALUES - 1);
                        c = hash_counter[hash]++;
                        hashtable[hash, c & (QLZ_POINTERS_3 - 1)] = src + u;
                    }

                    src += matchlen;
                    cword_val = ((cword_val >> 1) | 0x80000000);

                    if (matchlen == 3 && offset <= 63)
                    {
                        fast_write(destination, dst, offset << 2, 1);
                        dst++;
                    }
                    else if (matchlen == 3 && offset <= 16383)
                    {
                        fast_write(destination, dst, (offset << 2) | 1, 2);
                        dst += 2;
                    }
                    else if (matchlen <= 18 && offset <= 1023)
                    {
                        fast_write(destination, dst, ((matchlen - 3) << 2) | (offset << 6) | 2, 2);
                        dst += 2;
                    }
                    else if (matchlen <= 33)
                    {
                        fast_write(destination, dst, ((matchlen - 2) << 2) | (offset << 7) | 3, 3);
                        dst += 3;
                    }
                    else
                    {
                        fast_write(destination, dst, ((matchlen - 3) << 7) | (offset << 15) | 3, 4);
                        dst += 4;
                    }
                    lits = 0;
                }
                else
                {
                    destination[dst] = source[srcOffset + src];
                    cword_val = (cword_val >> 1);
                    src++;
                    dst++;
                }
            }
        }
        while (src <= srcSize - 1)
        {
            if ((cword_val & 1) == 1)
            {
                fast_write(destination, cword_ptr, (int)((cword_val >> 1) | 0x80000000), 4);
                cword_ptr = dst;
                dst += CWORD_LEN;
                cword_val = 0x80000000;
            }

            destination[dst] = source[srcOffset + src];
            src++;
            dst++;
            cword_val = (cword_val >> 1);
        }
        while ((cword_val & 1) != 1)
        {
            cword_val = (cword_val >> 1);
        }
        fast_write(destination, cword_ptr, (int)((cword_val >> 1) | 0x80000000), CWORD_LEN);
        write_header(destination, level, true, srcSize, dst);

        System.Array.Copy(destination, 0, outDest, outOffset, dst);
        return dst;
    }


    private static void fast_write(byte[] a, int i, int value, int numbytes)
    {
        for (int j = 0; j < numbytes; j++)
            a[i + j] = (byte)(value >> (j * 8));
    }

    public static int Decompress(byte[] source, int srcOffset, int srcSize, byte[] outDest, int outOffset)
    {
        int level;
        int size = GetUncompressedLength(source, srcOffset);
        int src = GetHeaderLen(source, srcOffset);
        int dst = 0;
        uint cword_val = 1;
        byte[] destination = outDest;
        int[] hashtable = new int[4096];
        byte[] hash_counter = new byte[4096];
        int last_matchstart = size - UNCONDITIONAL_MATCHLEN - UNCOMPRESSED_END - 1;
        int last_hashed = -1;
        int hash;
        uint fetch = 0;

        level = (source[srcOffset + 0] >> 2) & 0x3;

        if (level != 1 && level != 3)
            throw new ArgumentException("C# version only supports level 1 and 3");

        if ((source[srcOffset + 0] & 1) != 1)
        {
            System.Array.Copy(source, srcOffset + GetHeaderLen(source, srcOffset), outDest, outOffset, size);
            return size;
        }

        for (; ; )
        {
            if (cword_val == 1)
            {
                cword_val = (uint)(source[srcOffset + src] | (source[srcOffset + src + 1] << 8) | (source[srcOffset + src + 2] << 16) | (source[srcOffset + src + 3] << 24));
                src += 4;
                if (dst <= last_matchstart)
                {
                    if (level == 1)
                        fetch = (uint)(source[srcOffset + src] | (source[srcOffset + src + 1] << 8) | (source[srcOffset + src + 2] << 16));
                    else
                        fetch = (uint)(source[srcOffset + src] | (source[srcOffset + src + 1] << 8) | (source[srcOffset + src + 2] << 16) | (source[srcOffset + src + 3] << 24));
                }
            }

            if ((cword_val & 1) == 1)
            {
                uint matchlen;
                uint offset2;

                cword_val = cword_val >> 1;

                if (level == 1)
                {
                    hash = ((int)fetch >> 4) & 0xfff;
                    offset2 = (uint)hashtable[hash];

                    if ((fetch & 0xf) != 0)
                    {
                        matchlen = (fetch & 0xf) + 2;
                        src += 2;
                    }
                    else
                    {
                        matchlen = source[srcOffset + src + 2];
                        src += 3;
                    }
                }
                else
                {
                    uint offset;
                    if ((fetch & 3) == 0)
                    {
                        offset = (fetch & 0xff) >> 2;
                        matchlen = 3;
                        src++;
                    }
                    else if ((fetch & 2) == 0)
                    {
                        offset = (fetch & 0xffff) >> 2;
                        matchlen = 3;
                        src += 2;
                    }
                    else if ((fetch & 1) == 0)
                    {
                        offset = (fetch & 0xffff) >> 6;
                        matchlen = ((fetch >> 2) & 15) + 3;
                        src += 2;
                    }
                    else if ((fetch & 127) != 3)
                    {
                        offset = (fetch >> 7) & 0x1ffff;
                        matchlen = ((fetch >> 2) & 0x1f) + 2;
                        src += 3;
                    }
                    else
                    {
                        offset = (fetch >> 15);
                        matchlen = ((fetch >> 7) & 255) + 3;
                        src += 4;
                    }
                    offset2 = (uint)(dst - offset);
                }

                destination[outOffset + dst + 0] = destination[outOffset + offset2 + 0];
                destination[outOffset + dst + 1] = destination[outOffset + offset2 + 1];
                destination[outOffset + dst + 2] = destination[outOffset + offset2 + 2];

                for (int i = 3; i < matchlen; i += 1)
                {
                    destination[outOffset + dst + i] = destination[outOffset + offset2 + i];
                }

                dst += (int)matchlen;

                if (level == 1)
                {
                    fetch = (uint)(destination[outOffset + last_hashed + 1] | (destination[outOffset + last_hashed + 2] << 8) | (destination[outOffset + last_hashed + 3] << 16));
                    while (last_hashed < dst - matchlen)
                    {
                        last_hashed++;
                        hash = (int)(((fetch >> 12) ^ fetch) & (HASH_VALUES - 1));
                        hashtable[hash] = last_hashed;
                        hash_counter[hash] = 1;
                        fetch = (uint)(fetch >> 8 & 0xffff | (uint)destination[outOffset + last_hashed + 3] << 16);
                    }
                    fetch = (uint)(source[srcOffset + src] | (source[srcOffset + src + 1] << 8) | (source[srcOffset + src + 2] << 16));
                }
                else
                {
                    fetch = (uint)(source[srcOffset + src] | (source[srcOffset + src + 1] << 8) | (source[srcOffset + src + 2] << 16) | (source[srcOffset + src + 3] << 24));
                }
                last_hashed = dst - 1;
            }
            else
            {
                if (dst <= last_matchstart)
                {
                    destination[outOffset + dst] = source[srcOffset + src];
                    dst += 1;
                    src += 1;
                    cword_val = cword_val >> 1;

                    if (level == 1)
                    {
                        while (last_hashed < dst - 3)
                        {
                            last_hashed++;
                            int fetch2 = destination[outOffset + last_hashed] | (destination[outOffset + last_hashed + 1] << 8) | (destination[outOffset + last_hashed + 2] << 16);
                            hash = ((fetch2 >> 12) ^ fetch2) & (HASH_VALUES - 1);
                            hashtable[hash] = last_hashed;
                            hash_counter[hash] = 1;
                        }
                        fetch = (uint)(fetch >> 8 & 0xffff | (uint)source[srcOffset + src + 2] << 16);
                    }
                    else
                    {
                        fetch = (uint)(fetch >> 8 & 0xffff | (uint)source[srcOffset + src + 2] << 16 | (uint)source[srcOffset + src + 3] << 24);
                    }
                }
                else
                {
                    while (dst <= size - 1)
                    {
                        if (cword_val == 1)
                        {
                            src += CWORD_LEN;
                            cword_val = 0x80000000;
                        }

                        destination[outOffset + dst] = source[srcOffset + src];
                        dst++;
                        src++;
                        cword_val = cword_val >> 1;
                    }
                    return size;
                }
            }
        }
    }
}
