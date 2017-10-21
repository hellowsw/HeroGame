namespace Network.Compress
{
    public class RC6
    {
        const int r = 20;									/* based on security estimates */
        const int R24 = (2 * r + 4);
        const int WORD_BITS = 32;									/* word size in bits */

        /* derived constants */
        const int WORD_BYTES = (WORD_BITS / 8);							/* bytes per word */
        const int lgw = 5;									/* log2(w)*/ 

        const uint P32 = 0xB7E15163;							/* Magic constants for key setup */
        const uint Q32 = 0x9E3779B9;

        /* 循环位移 */
        uint ROTL(uint x,uint y)
        {
            return ((x << (int)(y & (WORD_BITS - 1))) | (x >> (int)(WORD_BITS - (y & (WORD_BITS - 1)))));
        }
        uint ROTR(uint x,uint y)
        {
            return ((x >> (int)(y & (WORD_BITS - 1))) | (x << (int)(WORD_BITS - (y & (WORD_BITS - 1)))));
        }

        int GetKeyPos(int keyLen)
        {
            return ((keyLen + WORD_BYTES - 1) / WORD_BYTES); /* key in words, rounded up */ 
        }

        uint[] S = new uint[R24];/* 子密钥组 */
        uint[] L = new uint[(32 + WORD_BYTES - 1) / WORD_BYTES]; /* Big enough for max b */
        unsafe void SetKey(byte *key_data, int keyLen)
	    {
		    int i, j, s, v;
		    uint A, B;

		    L[GetKeyPos(keyLen) - 1] = 0;
		    for (i = keyLen - 1; i >= 0; i--)
			    L[i / WORD_BYTES] = (L[i / WORD_BYTES] << 8) + key_data[i];

		    S[0] = P32;
            for (i = 1; i < R24; i++)
			    S[i] = S[i - 1] + Q32;

            i = j = 0;
		    A = B = 0;
		    v = R24;
		    if (GetKeyPos(keyLen) > v) v = GetKeyPos(keyLen);
		    v *= 3;

		    for (s = 1; s <= v; s++)
		    {
			    A = S[i] = ROTL((S[i] + A + B), 3);
			    B = L[j] = ROTL((L[j] + A + B), A + B);
			    i = (i + 1) % R24;
			    j = (j + 1) % GetKeyPos(keyLen);
		    }
	    }

	    unsafe void DoEncrypt(uint *src, uint *dest)
	    {
		    uint A, B, C, D, t, u, x;
		    int i;

		    A = src[0];
		    B = src[1];
		    C = src[2];
		    D = src[3];
		    B += S[0];
		    D += S[1];
		    for (i = 2; i <= 2 * r; i += 2)
		    {
			    t = ROTL(B * (2 * B + 1), lgw);
			    u = ROTL(D * (2 * D + 1), lgw);
			    A = ROTL(A ^ t, u) + S[i];
			    C = ROTL(C ^ u, t) + S[i + 1];
			    x = A;
			    A = B;
			    B = C;
			    C = D;
			    D = x;
		    }
		    A += S[2 * r + 2];
		    C += S[2 * r + 3];
		    dest[0] = A;
		    dest[1] = B;
		    dest[2] = C;
		    dest[3] = D;
	    }

	    unsafe void DoDecrypt(uint *src, uint *dest)
	    {
		    uint A, B, C, D, t, u, x;
		    int i;

		    A = src[0];
		    B = src[1];
		    C = src[2];
		    D = src[3];
		    C -= S[2 * r + 3];
		    A -= S[2 * r + 2];
		    for (i = 2 * r; i >= 2; i -= 2)
		    {
			    x = D;
			    D = C;
			    C = B;
			    B = A;
			    A = x;
			    u = ROTL(D * (2 * D + 1), lgw);
			    t = ROTL(B * (2 * B + 1), lgw);
			    C = ROTR(C - S[i + 1], t) ^ u;
			    A = ROTR(A - S[i], u) ^ t;
		    }
		    D -= S[1];
		    B -= S[0];
		    dest[0] = A;
		    dest[1] = B;
		    dest[2] = C;
		    dest[3] = D;
	    }


	    /************************************************************************/
	    /*                                                                      */
	    /************************************************************************/
        public unsafe bool Encrypt(byte* buffer, int size, byte* key, ref int keyLen)
	    {
            uint now = (uint)System.Environment.TickCount;
            *(uint*)(key) = now;
		    keyLen = 4;

		    SetKey(key,4);

		    //每次仅处理16字节的加密(4个int)
		    //除16,剩余字节不加密
		    int times = size >> 4;
		    uint* pos = (uint*)(buffer);
		    while(times > 0)
		    {
			    DoEncrypt(pos,pos);
			    pos += 4;
			    times--;
		    }
		    return true;
	    }

        public unsafe bool Decrypt(byte* buffer, int size, byte* key, int keyLen)
	    {
		    SetKey(key,keyLen);

		    //每次仅处理16字节的加密(4个int)
		    //除16,剩余字节不加密
		    int times = size >> 4;
            uint* pos = (uint*)(buffer);
		    while(times > 0)
		    {
			    DoDecrypt(pos,pos);
			    pos += 4;
			    times--;
		    }
		    return true;
	    }

        public bool Encrypt(byte[] buffer, int offset, int size, byte[] key, ref int keyLen)
        {
            unsafe
            {
                fixed (byte* buffPtr = &buffer[offset], keyPtr = &key[0])
                {
                    return Encrypt(buffPtr, size, keyPtr, ref keyLen);
                }
            }
        }

        public bool Decrypt(byte[] buffer, int offset, int size, byte[] key, int keyLen)
        {
            unsafe
            {
                fixed (byte* buffPtr = &buffer[offset], keyPtr = &key[0])
                {
                    return Decrypt(buffPtr, size, keyPtr, keyLen);
                }
            }
        }
    }
}
