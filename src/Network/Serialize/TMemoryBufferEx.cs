using System;
using Network.Log;
using System.Text;
using Network.DataDefs;

namespace Network.Net
{
    public static class EncodeHelper
    {
        public const short UTF8 = 0;
        public const short GBK = 1;
        public const short Unicode = 2;
        public static Encoding GetEncoder(short type)
        {
            Encoding encoder = null;
            switch (type)
            {
                case UTF8:
                    encoder = Encoding.UTF8;
                    break;
                case GBK:
                    encoder = encodingGBK;
                    break;
                case Unicode:
                    encoder = Encoding.Unicode;
                    break;
            }
            return encoder;
        }

        #region private
        static Encoding encodingGBK = Encoding.GetEncoding(LYBGlobalConsts.ENCODE);
        #endregion
    }

    /// <summary>
    /// 重写thirft中TMemoryBuffer类以便重复利用buffer
    /// </summary>
    public class TMemoryBufferEx : Thrift.Transport.TTransport
    {
        protected byte[] buffer;
        protected int writePos = 0;
        protected int readPos = 0;
        protected int defaultSize = 1024;
        private int memAlignSize = 1024;
        private int needSize = 0;//用于收集写入一个结构时的尺寸
        private bool isGettingSize = false;

        public Action<int> OnReadPosChange = null;
        public Action<int> OnWritePosChange = null;

        public TMemoryBufferEx()
            :this(-1)
        {
        }
        public TMemoryBufferEx(int initSize)
        {
            if (initSize == -1)
                initSize = defaultSize;
            buffer = new byte[initSize];
        }

        public TMemoryBufferEx(byte[] bufferIn)
        {
            buffer = bufferIn;
            readPos = 0;
            writePos = bufferIn.Length;
        }

        public TMemoryBufferEx(byte[] bufferIn, int offset, int dataSize)
        {
            buffer = bufferIn;
            readPos = offset;
            writePos = offset + dataSize;
        }

        public void Clone(TMemoryBufferEx rhs)
        {
            Reset();

            int dataSize = rhs.Size;
            readPos = 0;
            writePos = 0;
            Reserve(dataSize);

            Buffer.BlockCopy(rhs.buffer, rhs.readPos, buffer, 0, dataSize);
            writePos = dataSize;
        }

        public byte[] Buff { get { return GetBuffer(); } }
        public int ReadPos
        {
            get { return readPos; }
            set
            {
                if (value > writePos)
                {
                    throw new Exception("Buffer Error: ReadPos set: value great than writePos"
                        + (new System.Diagnostics.StackTrace()).ToString());
                }
                readPos = value;
                OnReadPosChange?.Invoke(readPos);
            }
        }
        public int WritePos { get { return writePos; } }
        public int Size { get { return WritePos - ReadPos; } }
        public int Capacity { get { return buffer.Length; } }
        public int Space { get { return Capacity - writePos; } }
        public int PreSpace { get { return readPos; } }

        public override int Read(byte[] buf, int outOffset, int readSize)
        {
            int resultPos = readPos + readSize;
            if (resultPos > writePos)
            {
                throw new Exception("Buffer Error: Read: readPos + readSize great than writePos"
                    + (new System.Diagnostics.StackTrace()).ToString());
            }

            // 允许移动读取指针
            if (buf != null)
                Buffer.BlockCopy(buffer, readPos, buf, outOffset, readSize);
            readPos = resultPos;
            OnReadPosChange?.Invoke(readPos);
            return readSize;
        }

        // 摸拟流的函数

        public unsafe void WriteBytes(int val, int writeCount)
        {
            byte* valPtr = (byte*)&val;
            WriteUnsafeBuff(valPtr, 0, writeCount);
        }
        public unsafe void WriteByte(byte val)
        {
            byte* valPtr = &val;
            WriteUnsafeBuff(valPtr, 0, sizeof(byte));
        }
        public unsafe void WriteSByte(sbyte val)
        {
            sbyte* valPtr = &val;
            WriteUnsafeBuff((byte*)valPtr, 0, sizeof(sbyte));
        }

        public unsafe void WriteShort(short val)
        {
            short* valPtr = &val;
            WriteUnsafeBuff((byte*)valPtr, 0, sizeof(short));
        }
        public unsafe void WriteUShort(ushort val)
        {
            ushort* valPtr = &val;
            WriteUnsafeBuff((byte*)valPtr, 0, sizeof(ushort));
        }

        public unsafe void WriteInt(int val)
        {
            int* valPtr = &val;
            WriteUnsafeBuff((byte*)valPtr, 0, sizeof(int));
        }

        public unsafe void WriteUInt(uint val)
        {
            uint* valPtr = &val;
            WriteUnsafeBuff((byte*)valPtr, 0, sizeof(uint));
        }

        public unsafe void WriteInt64(Int64 val)
        {
            Int64* valPtr = &val;
            WriteUnsafeBuff((byte*)valPtr, 0, sizeof(Int64));
        }

        public unsafe void WriteUInt64(UInt64 val)
        {
            UInt64* valPtr = &val;
            WriteUnsafeBuff((byte*)valPtr, 0, sizeof(UInt64));
        }

        public unsafe void WriteFloat(float val)
        {
            float* valPtr = &val;
            WriteUnsafeBuff((byte*)valPtr, 0, sizeof(float));
        }

        public unsafe void WriteDouble(double val)
        {
            double* valPtr = &val;
            WriteUnsafeBuff((byte*)valPtr, 0, sizeof(double));
        }
        
        public sbyte ReadSByte()
        {
            if ((readPos + 1) > writePos)
            {
                throw new Exception("Buffer Error: ReadSByte: readPos great than writePos"
                    + (new System.Diagnostics.StackTrace()).ToString());
            }
            sbyte result = (sbyte)buffer[readPos];
            readPos += 1;
            OnReadPosChange?.Invoke(readPos);
            return result;
        }

        public byte ReadByte()
        {
            if ((readPos + 1) > writePos)
            {
                throw new Exception("Buffer Error: ReadByte: readPos great than writePos"
                    + (new System.Diagnostics.StackTrace()).ToString());
            }
            byte result = buffer[readPos];
            readPos += 1;
            OnReadPosChange?.Invoke(readPos);
            return result;
        }

        public unsafe short ReadShort()
        {
            short result = 0;
            ReadUnsafeBuff((byte*)&result, 0, sizeof(short));
            return result;
        }

        public unsafe ushort ReadUShort()
        {
            ushort result = 0;
            ReadUnsafeBuff((byte*)&result, 0, sizeof(ushort));
            return result;
        }

        public unsafe int ReadInt()
        {
            int result = 0;
            ReadUnsafeBuff((byte*)&result, 0, sizeof(int));
            return result;
        }

        public unsafe uint ReadUInt()
        {
            uint result = 0;
            ReadUnsafeBuff((byte*)&result, 0, sizeof(uint));
            return result;
        }

        public unsafe long ReadInt64()
        {
            long result = 0;
            ReadUnsafeBuff((byte*)&result, 0, sizeof(long));
            return result;
        }

        public unsafe ulong ReadUInt64()
        {
            ulong result = 0;
            ReadUnsafeBuff((byte*)&result, 0, sizeof(ulong));
            return result;
        }

        public unsafe float ReadFloat()
        {
            float result = 0;
            ReadUnsafeBuff((byte*)&result, 0, sizeof(float));
            return result;
        }

        public unsafe double ReadDouble()
        {
            double result = 0;
            ReadUnsafeBuff((byte*)&result, 0, sizeof(double));
            return result;
        }

        public void Reserve(int needSize)
        {
            if (Space > needSize)
                return;
            if (PreSpace > needSize)
            {
                MoveDataToBegin();
                return;
            }

            if (buffer.Length < (writePos + needSize))
            {
                try
                {
                    int oldSize = Size;
                    int needCapacity = oldSize + needSize;
                    int newCapacity = Capacity;
                    if (newCapacity == 0)
                        newCapacity = defaultSize;

                    while (newCapacity < needCapacity)
                    {
                        // 大于64K时,尺寸不再按乘以2计算
                        if (newCapacity >= (64 * 1024))
                            newCapacity += 4 * memAlignSize;
                        else
                            newCapacity = newCapacity << 1;
                    }

                    byte[] newbuf = new byte[newCapacity];
                    Buffer.BlockCopy(buffer, readPos, newbuf, 0, oldSize);
                    buffer = newbuf;
                    readPos = 0;
                    writePos = oldSize;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.LogInfo(ex.Message);
                    LogManager.Instance.LogInfo(ex.StackTrace);
                    throw ex;
                }
            }
            else
            {
                if (readPos > (4 * 1024) && readPos > (buffer.Length / 3))
                    MoveDataToBegin();
            }
        }

        public void Reset(byte[] newRefBuffer, int dataOffset, int dataSize)
        {
            try
            {
                buffer = newRefBuffer;
                writePos = dataOffset + dataSize;
                readPos = dataOffset;
            }
            catch(Exception ex)
            {
                LogManager.Instance.LogInfo(ex.Message);
                LogManager.Instance.LogInfo(ex.StackTrace);
                throw ex;
            }
        }

        public virtual void Reset(TMemoryBufferEx rhs)
        {
            try
            {
                buffer = rhs.buffer;
                writePos = rhs.writePos;
                readPos = rhs.readPos;
            }
            catch(Exception ex)
            {
                LogManager.Instance.LogInfo(ex.Message);
                LogManager.Instance.LogInfo(ex.StackTrace);
                throw ex;
            }
        }

        public virtual void Reset()
        {
            writePos = 0;
            readPos = 0;
        }

        public void Fill(byte bt, int count)
        {
            Reserve(count);
            if (isGettingSize)
            {
                needSize += count;
                return;
            }

            for (int i = 0; i < count; ++i)
                buffer[writePos + i] = bt;
            writePos += count;
            OnWritePosChange?.Invoke(writePos);
        }

        public override void Write(byte[] buf, int srcOff, int writeSize)
        {
            Reserve(writeSize);
            if (isGettingSize)
            {
                needSize += writeSize;
                return;
            }

            if (buf != null)
                Buffer.BlockCopy(buf, srcOff, buffer, writePos, writeSize);
            writePos += writeSize;
            OnWritePosChange?.Invoke(writePos);
        }

        internal unsafe void ReadUnsafeBuff(byte* buf, int destOff, int readSize)
        {
            if ((readPos + readSize) > writePos)
            {
                throw new Exception("Buffer Error: ReadUnsafeBuff: readPos + readSize great than writePos"
                    + (new System.Diagnostics.StackTrace()).ToString());
            }

            fixed (byte* srcBytePtr = &buffer[readPos])   //var is string
            {
                int* srcPtr = (int*)srcBytePtr;
                int* destPtr = (int*)(buf + destOff);

                while (readSize > sizeof(int))
                {
                    *(destPtr++) = *(srcPtr++);

                    readSize -= sizeof(int);
                    readPos += sizeof(int);
                }

                byte* srcByteP = (byte*)srcPtr;
                byte* destByteP = (byte*)destPtr;
                for (int i = 0; i < readSize; ++i)
                {
                    *(destByteP++) = *(srcByteP++);
                }
                readPos += readSize;
            }
            OnReadPosChange?.Invoke(readPos);
        }

        internal unsafe void WriteUnsafeBuff(byte* buf, int srcOff, int writeSize)
        {
            Reserve(writeSize);
            if (isGettingSize)
            {
                needSize += writeSize;
                return;
            }

            fixed (byte* destBytePtr = &buffer[writePos])   //var is string
            {
                int* srcPtr = (int*)(buf + srcOff);
                int* destPtr = (int*)destBytePtr;

                while (writeSize > sizeof(int))
                {
                    *(destPtr++) = *(srcPtr++);

                    writeSize -= sizeof(int);
                    writePos += sizeof(int);
                }

                byte* srcByteP = (byte*)srcPtr;
                byte* destByteP = (byte*)destPtr;
                for (int i = 0; i < writeSize; ++i)
                {
                    *(destByteP++) = *(srcByteP++);
                }
                writePos += writeSize;
            }
            OnWritePosChange?.Invoke(writePos);
        }

        public override void Write(byte[] buf)
        {
            Write(buf, 0, buf.Length);
        }
        static TMemoryBufferEx tempBuffer = null;
        static TMemoryBufferEx TempBuffer
        {
            get
            {
                if (tempBuffer == null)
                    tempBuffer = new TMemoryBufferEx();
                return tempBuffer;
            }
        }
        public TMemoryBufferEx ReadUntil(byte untilByte)
        {
            TempBuffer.Reset();

            byte c = ReadByte();
            while (c != untilByte)
            {
                TempBuffer.WriteByte(c);
                c = ReadByte();
            }
            TempBuffer.WriteByte(untilByte);
            return TempBuffer;
        }

        //-------------------------------------------------------
        #region 字符数组
        public unsafe int WriteString(string val, int destStrLen)
        {
            short destByteLen = (short)(destStrLen * sizeof(char));
            short byteLen = (short)(val.Length * sizeof(char));

            fixed (char* str = val)
            {
                if (destByteLen > byteLen)
                {
                    WriteUnsafeBuff((byte*)str, 0, byteLen);
                    Fill(0, destByteLen - byteLen);
                }
                else
                {
                    WriteUnsafeBuff((byte*)str, 0, destByteLen);
                }
                return destByteLen;
            }
        }
        public unsafe int WriteString(char[] val, int destStrLen)
        {
            short destByteLen = (short)(destStrLen * sizeof(char));
            short byteLen = (short)(val.Length * sizeof(char));

            fixed (char* str = val)
            {
                if (destByteLen > byteLen)
                {
                    WriteUnsafeBuff((byte*)str, 0, byteLen);
                    Fill(0, destByteLen - byteLen);
                }
                else
                {
                    WriteUnsafeBuff((byte*)str, 0, destByteLen);
                }
                return destByteLen;
            }
        }

        public unsafe int WriteStringUtf8(string val, int destByteLen)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(val);
            var byteLen = buffer.Length;

            if (destByteLen > byteLen)
            {
                Write(buffer, 0, byteLen);
                Fill(0, destByteLen - byteLen);
            }
            else
            {
                Write(buffer, 0, destByteLen);
            }
            return destByteLen;
        }
        public unsafe int WriteStringUtf8(char[] val, int destByteLen)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(val);
            var byteLen = buffer.Length;

            if (destByteLen > byteLen)
            {
                Write(buffer, 0, byteLen);
                Fill(0, destByteLen - byteLen);
            }
            else
            {
                Write(buffer, 0, destByteLen);
            }
            return destByteLen;
        }

        protected static Encoding encodingGBK = Encoding.GetEncoding(LYBGlobalConsts.ENCODE);
        public unsafe int WriteStringGBK(string val, int destByteLen)
        {
            byte[] buffer = encodingGBK.GetBytes(val);
            var byteLen = buffer.Length;

            if (destByteLen > byteLen)
            {
                Write(buffer, 0, byteLen);
                Fill(0, destByteLen - byteLen);
            }
            else
            {
                Write(buffer, 0, destByteLen);
            }
            return destByteLen;
        }
        public unsafe int WriteStringGBK(char[] val, int destByteLen)
        {
            byte[] buffer = encodingGBK.GetBytes(val);
            var byteLen = buffer.Length;

            if (destByteLen > byteLen)
            {
                Write(buffer, 0, byteLen);
                Fill(0, destByteLen - byteLen);
            }
            else
            {
                Write(buffer, 0, destByteLen);
            }
            return destByteLen;
        }
        #endregion

        public unsafe int WriteString(string val)
        {
            short byteLen = (short)(val.Length * sizeof(char));
            WriteShort(byteLen);
            fixed (char* str = val)   //var is string
            {
                WriteUnsafeBuff((byte*)str, 0, byteLen);
                return byteLen;
            }
        }
        public unsafe int WriteStringGBK(string val)
        {
            byte[] buffer = encodingGBK.GetBytes(val);
            WriteShort((short)buffer.Length);
            Write(buffer);
            return buffer.Length;
        }

        public unsafe int WriteStringUtf8(string val)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(val);
            WriteShort((short)buffer.Length);
            Write(buffer);
            return buffer.Length;
        }

        public unsafe int WriteString(char[] val)
        {
            short byteLen = (short)(val.Length * sizeof(char));
            WriteShort(byteLen);
            fixed (char* str = val)   //var is string
            {
                WriteUnsafeBuff((byte*)str, 0, byteLen);
                return byteLen;
            }
        }
        public unsafe int WriteStringGBK(char[] val)
        {
            byte[] buffer = encodingGBK.GetBytes(val);
            WriteShort((short)buffer.Length);
            Write(buffer);
            return buffer.Length;
        }

        public unsafe int WriteStringUtf8(char[] val)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(val);
            WriteShort((short)buffer.Length);
            Write(buffer);
            return buffer.Length;
        }

        //-------------------------------------------------------
        protected string DoReadString(Encoding strEncoder)
        {
            int readSize = ReadShort();
            TempBuffer.Reset();
            TempBuffer.Reserve(readSize);
            Read(TempBuffer.GetBuffer(), 0, readSize);
            return strEncoder.GetString(TempBuffer.GetBuffer(), 0, readSize);
        }
        protected char[] DoReadChars(Encoding strEncoder)
        {
            int readSize = ReadShort();
            TempBuffer.Reset();
            TempBuffer.Reserve(readSize);
            Read(TempBuffer.GetBuffer(), 0, readSize);
            return strEncoder.GetChars(TempBuffer.GetBuffer(), 0, readSize);
        }
        protected string DoReadString(Encoding strEncoder, out int readSize)
        {
            readSize = ReadShort();
            TempBuffer.Reset();
            TempBuffer.Reserve(readSize);
            Read(TempBuffer.GetBuffer(), 0, readSize);
            return strEncoder.GetString(TempBuffer.GetBuffer(), 0, readSize);
        }
        protected char[] DoReadChars(Encoding strEncoder, out int readSize)
        {
            readSize = ReadShort();
            TempBuffer.Reset();
            TempBuffer.Reserve(readSize);
            Read(TempBuffer.GetBuffer(), 0, readSize);
            return strEncoder.GetChars(TempBuffer.GetBuffer(), 0, readSize);
        }
        protected string DoReadString(Encoding strEncoder, int readSize)
        {
            TempBuffer.Reset();
            TempBuffer.Reserve(readSize);
            Read(TempBuffer.GetBuffer(), 0, readSize);
            return strEncoder.GetString(TempBuffer.GetBuffer(), 0, readSize);
        }
        protected char[] DoReadChars(Encoding strEncoder, int readSize)
        {
            TempBuffer.Reset();
            TempBuffer.Reserve(readSize);
            Read(TempBuffer.GetBuffer(), 0, readSize);
            return strEncoder.GetChars(TempBuffer.GetBuffer(), 0, readSize);
        }

        public unsafe string ReadString(out int readSize)
        {
            return DoReadString(System.Text.Encoding.Unicode, out readSize);
        }

        public unsafe string ReadStringGBK(out int readSize)
        {
            return DoReadString(encodingGBK, out readSize);
        }

        public unsafe string ReadStringUtf8(out int readSize)
        {
            return DoReadString(System.Text.Encoding.UTF8, out readSize);
        }

        public unsafe char[] ReadChars(out int readSize)
        {
            return DoReadChars(System.Text.Encoding.Unicode, out readSize);
        }

        public unsafe char[] ReadCharsGBK(out int readSize)
        {
            return DoReadChars(encodingGBK, out readSize);
        }

        public unsafe char[] ReadCharsUtf8(out int readSize)
        {
            return DoReadChars(System.Text.Encoding.UTF8, out readSize);
        }

        public unsafe string ReadString()
        {
            return DoReadString(System.Text.Encoding.Unicode);
        }

        public unsafe string ReadStringGBK()
        {
            return DoReadString(encodingGBK);
        }

        public unsafe string ReadStringUtf8()
        {
            return DoReadString(System.Text.Encoding.UTF8);
        }
        public unsafe string ReadString(int destStrLen)
        {
            var readBytesCount = destStrLen * sizeof(char);
            return DoReadString(Encoding.Unicode, readBytesCount);
        }
        public unsafe char[] ReadChars(int destStrLen)
        {
            var readBytesCount = destStrLen * sizeof(char);
            return DoReadChars(Encoding.Unicode, readBytesCount);
        }
        public unsafe string ReadStringGBK(int readBytesCount)
        {
            return DoReadString(encodingGBK, readBytesCount);
        }
        public unsafe char[] ReadCharsGBK(int readBytesCount)
        {
            return DoReadChars(encodingGBK, readBytesCount);
        }
        public unsafe string ReadStringUtf8(int readBytesCount)
        {
            return DoReadString(Encoding.UTF8, readBytesCount);
        }
        public unsafe char[] ReadCharsUtf8(int readBytesCount)
        {
            return DoReadChars(Encoding.UTF8, readBytesCount);
        }

        public unsafe char[] ReadChars()
        {
            return DoReadChars(System.Text.Encoding.Unicode);
        }

        public unsafe char[] ReadCharsGBK()
        {
            return DoReadChars(encodingGBK);
        }

        public unsafe char[] ReadCharsUtf8()
        {
            return DoReadChars(System.Text.Encoding.UTF8);
        }

        //-------------------------------------------------------
        public byte[] GetBuffer()
        {
            return buffer;
        }

        public byte[] ToByteArray()
        {
            return ToByteArray(0);
        }
        public byte[] ToByteArray(int readCount)
        {
            if (readCount == 0)
                readCount = Size;
            if (readCount == 0)
                return new byte[0];
            byte[] ret = new byte[readCount];
            Buffer.BlockCopy(buffer, readPos, ret, 0, readCount);
            return ret;
        }

        public override void Open()
        {
            /** do nothing **/
        }

        public override void Close()
        {
            /** do nothing **/
        }

        protected override void Dispose(bool disposing)
        {
        }

        public override bool IsOpen
        {
            get { return true; }
        }

        public void GetSizeBegin()
        {
            isGettingSize = true;
            needSize = 0;
        }

        public int GetSizeEnd()
        {
            isGettingSize = false;
            return needSize;
        }

        public void MoveDataToBegin()
        {
            try
            {
                //safe模式
                if (readPos == 0)
                    return;
                int dataSize = Size;
                for (int i = 0; i < dataSize; ++i)
                {
                    buffer[i] = buffer[readPos + i];
                }
                readPos = 0;
                writePos = dataSize;

                ////Unsafe模式
                //int dataSize = Size;
                //fixed (byte* beginPtr = &buffer[0], dataPtr = &buffer[readPos])
                //{
                //    byte* destPtr = beginPtr;
                //    byte* srcPtr = dataPtr;

                //    int count = dataSize;
                //    while (count-- > 0)
                //        *destPtr++ = *srcPtr++;
                //}
                //readPos = 0;
                //writePos = dataSize;

                OnReadPosChange?.Invoke(readPos);
                OnWritePosChange?.Invoke(writePos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
