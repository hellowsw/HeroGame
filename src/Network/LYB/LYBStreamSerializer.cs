
using Network.Serialize;
/**
* LYB流序列化管理(如邮件,RPC等使用)
*/
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Network.Net
{
    public enum LYBVariant : int
    {
        VT_EMPTY = 0,        // 完全的空类型，变量未初始化
        VT_NULL = 1,        // 空类型，表示没有数据

        VT_RPC_OP_LITE = 4,        // 特殊控制字符，轻量级版本，但为了向下兼容，以前的VT_RPC_OP也保留

        VT_WORD = 8,        // [EX]2字节无符号数据
        VT_SHORTINTEGER = 9,        // [EX]16位带符号整数

        VT_U32_24 = 0x0a,     // [EX] 原本是UINT32类型，数据压缩后变为24位数据
        VT_I32_24 = 0x0b,     // [EX] 原本是 INT32类型，数据压缩后变为24位数据
        VT_U64_24 = 0x0c,     // [EX] 原本是UINT64类型，数据压缩后变为24位数据
        VT_I64_24 = 0x0d,     // [EX] 原本是 INT64类型，数据压缩后变为24位数据

        VT_DWORD = 0x10,     // 4字节无符号数据
        VT_INTEGER = 0x11,     // 32位带符号整数
        VT_FLOAT = 0x12,     // 单精度浮点数

        VT_U64_56 = 0x1a,     // [EX] 原本是UINT64类型，数据压缩后变为56位数据
        VT_I64_56 = 0x1b,     // [EX] 原本是 INT64类型，数据压缩后变为56位数据

        VT_RPC_OP = 0x1f,     // 特殊控制字符

        VT_QWORD = 0x20,     // 8字节无符号数据
        VT_LARGINTEGER = 0x21,     // 64位带符号大整数
        VT_DOUBLE = 0x22,     // 双精度浮点数
        VT_DATE64 = 0x23,     // 64位日期

        VT_POINTER = 0x40,     // 指针类型数据
        VT_STRING = 0x41,     // 标准字符串
        VT_BSTRING = 0x42,     // BSTR字符串
        VT_UTF8 = 0x43,     // UTF8字符串

        // 压缩用的
        //_UBIT_M32   = 0xff000000,
        //_IBIT_M32   = 0xff800000,
        //_UBIT_M64:Number = 0xffffffffff000000, // 这两个64位的暂时没用到，被转换为32位分开计算了
        //_IBIT_M64:Number = 0xffffffffff800000,

        // 自定义的类型名，方便与PC客户端一致，同时为了序列化统一指定类型
        INT8 = 0x10000001,
        UINT8 = 0x10000002,
        INT16 = 0x10000003,
        UINT16 = 0x10000004,
        INT32 = 0x10000005,
        UINT32 = 0x10000006,
        INT64 = 0x10000007,
        UINT64 = 0x10000008,
        FLOAT = 0x10000009,
        DOUBLE = 0x1000000a,
        STRING = 0x1000000b,
        RPCOP = 0x1000000c,
        NULL = 0x1000000d,        // 空类型，表示没有数据
    }

    public unsafe class LYBStreamIn : LYBStream, ILYBStreamSerializerBase
    {
        public LYBStreamIn()
        {
            OnWritePosChange = OnStreamWritePosChange;
            Skip(4);
        }

        public int StreamSize = 0;
        public int StreamMaxSize = 0;
        public override void Reset()
        {
            base.Reset();
            Skip(4);
        }
        public override void Reset(TMemoryBufferEx rhs)
        {
            base.Reset(rhs);
            Skip(4);
        }
        public void ResetToContinue(TMemoryBufferEx rhs)
        {
            base.Reset(rhs);
        }

        [Obsolete("未实现的函数")]
        public ILYBStreamSerializerBase GetStream() { throw new Exception("Don't call GetStream in LYBStreamIn;"); }

        public void HandleNull()
        {
            WriteInt((int)LYBVariant.VT_NULL);
        }
        public void HandleRpcOP(ref uint opType)
        {
            WriteShort((short)LYBVariant.VT_RPC_OP);
            WriteShort(sizeof(uint));
            WriteUInt(opType);
        }

        public void Handle(ref sbyte rval)
        {
            WriteShort((short)LYBVariant.VT_SHORTINTEGER);
            WriteShort(rval);
        }
        public void Handle(ref byte rval)
        {
            WriteShort((short)LYBVariant.VT_WORD);
            WriteShort(rval);
        }
        public void Handle(ref short rval)
        {
            WriteShort((short)LYBVariant.VT_SHORTINTEGER);
            WriteShort(rval);
        }
        public void Handle(ref ushort rval)
        {
            WriteShort((short)LYBVariant.VT_WORD);
            WriteUShort(rval);
        }
        public void Handle(ref int rval)
        {
            WriteInt((int)LYBVariant.VT_INTEGER);
            WriteInt(rval);
        }
        public void Handle(ref uint rval)
        {
            WriteInt((int)LYBVariant.VT_DWORD);
            WriteUInt(rval);
        }
        public void Handle(ref long rval)
        {
            ulong urval = (ulong)rval >> 23;
            if (urval == 0 || urval == 0x1ffffffffff)
            {
                uint tempVal = (uint)rval;
                tempVal = tempVal << 8 | (byte)LYBVariant.VT_I64_24;
                WriteUInt(tempVal);
            }
            else
            {
                urval = (ulong)rval >> 55;
                if (urval == 0 || urval == 0x1ff)
                {
                    urval = (ulong)rval << 8 | (byte)LYBVariant.VT_I64_56;
                    WriteUInt64(urval);
                }
                else
                {
                    WriteShort((short)LYBVariant.VT_LARGINTEGER);
                    WriteShort(sizeof(long));
                    WriteInt64(rval);
                }
            }
        }
        public void Handle(ref ulong rval)
        {
            ulong urval = rval >> 24;
            if (urval == 0)
            {
                uint tempVal = (uint)rval;
                tempVal = tempVal << 8 | (byte)LYBVariant.VT_U64_24;
                WriteUInt(tempVal);
            }
            else
            {
                urval = rval >> 56;
                if (urval == 0)
                {
                    urval = rval << 8 | (byte)LYBVariant.VT_U64_56;
                    WriteUInt64(urval);
                }
                else
                {
                    WriteShort((short)LYBVariant.VT_QWORD);
                    WriteShort(sizeof(long));
                    WriteUInt64(rval);
                }
            }
        }
        public void Handle(ref float rval)
        {
            WriteInt((short)LYBVariant.VT_FLOAT);
            WriteFloat(rval);
        }
        public void Handle(ref double rval)
        {
            WriteShort((short)LYBVariant.VT_DOUBLE);
            WriteShort(sizeof(double));
            WriteDouble(rval);
        }
        public void Handle(ref bool rval)
        {
            WriteShort((short)LYBVariant.VT_WORD);
            WriteShort((short)(rval ? 1 : 0));
        }
        public void Handle(ref string rval)
        {
            WriteShort((short)LYBVariant.VT_STRING);

            byte[] buffer = encodingGBK.GetBytes(rval);
            var byteSize = (short)(buffer.Length + 1);
            WriteShort(byteSize);
            Write(buffer);
            Fill(0, SizeAlign4B(byteSize) - buffer.Length);
        }
        public void Handle(ref char[] rval)
        {
            WriteShort((short)LYBVariant.VT_STRING);

            byte[] buffer = encodingGBK.GetBytes(rval);
            var byteSize = (short)(buffer.Length + 1);
            WriteShort(byteSize);
            Write(buffer);
            Fill(0, SizeAlign4B(byteSize) - buffer.Length);
        }
        public void Handle(ref byte[] rval)
        {
            WriteShort((short)LYBVariant.VT_POINTER);
            WriteShort((short)rval.Length);
            Write(rval);
            Skip(SizeAlign4B(rval.Length) - rval.Length);
        }
        public void Handle(ref TBinaryData rval)
        {
            WriteShort((short)LYBVariant.VT_POINTER);
            WriteShort((short)rval.Size);
            Write(rval.Buff, rval.ReadPos, rval.Size);
            Skip(SizeAlign4B(rval.Size) - rval.Size);
        }
        public void Handle(ref LYBStream rval)
        {
            WriteShort((short)LYBVariant.VT_POINTER);
            Write(rval.Buff, rval.ReadPos, rval.Size);
            Skip(SizeAlign4B(rval.Size) - rval.Size);
        }
        public void Handle<T>(ref T rval) where T : ILYBSerializable, new()
        {
            rval.Serialize(this);
        }
        //-----------------------------------------
        public void Handle(ref short[] rval)
        {
            var rvalLength = rval.Length;
            WriteShort((short)rvalLength);
            for (int i = 0; i < rvalLength; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref ushort[] rval)
        {
            var rvalLength = rval.Length;
            WriteShort((short)rvalLength);
            for (int i = 0; i < rvalLength; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref int[] rval)
        {
            var rvalLength = rval.Length;
            WriteShort((short)rvalLength);
            for (int i = 0; i < rvalLength; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref uint[] rval)
        {
            var rvalLength = rval.Length;
            WriteShort((short)rvalLength);
            for (int i = 0; i < rvalLength; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref long[] rval)
        {
            var rvalLength = rval.Length;
            WriteShort((short)rvalLength);
            for (int i = 0; i < rvalLength; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref ulong[] rval)
        {
            var rvalLength = rval.Length;
            WriteShort((short)rvalLength);
            for (int i = 0; i < rvalLength; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref float[] rval)
        {
            var rvalLength = rval.Length;
            WriteShort((short)rvalLength);
            for (int i = 0; i < rvalLength; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref double[] rval)
        {
            var rvalLength = rval.Length;
            WriteShort((short)rvalLength);
            for (int i = 0; i < rvalLength; ++i)
                Handle(ref rval[i]);
        }
        public void Handle<T>(ref T[] rval) where T : ILYBSerializable, new()
        {
            var rvalLength = rval.Length;
            WriteShort((short)rvalLength);
            for (int i = 0; i < rvalLength; ++i)
                Handle(ref rval[i]);
        }

        //-----------------------------------------------------
        [Obsolete("未实现的函数")]
        public void Handle(ref string rval, short strLen)
        {
            throw new Exception("Not implemented.");
        }
        [Obsolete("未实现的函数")]
        public void Handle(ref char[] rval, short strLen)
        {
            throw new Exception("Not implemented.");
        }
        public void Handle(ref byte[] rval, short valCount)
        {
            throw new Exception("Not implemented.");
        }
        public void Handle(ref short[] rval, short valCount)
        {
            for (short i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref ushort[] rval, short valCount)
        {
            for (short i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref int[] rval, short valCount)
        {
            for (short i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref uint[] rval, short valCount)
        {
            for (short i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref long[] rval, short valCount)
        {
            for (short i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref ulong[] rval, short valCount)
        {
            for (short i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref float[] rval, short valCount)
        {
            for (short i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref double[] rval, short valCount)
        {
            for (short i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle<T>(ref T[] rval, short valCount) where T : ILYBSerializable, new()
        {
            for (int i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }

        //-----------------------------------------------------
        public void Skip(int byteCount)
        {
            Fill(0, byteCount);
        }

        public void SkipVariant()
        {
            HandleNull();
        }
        public void WriteBinaryAlign4B(TBinaryData rval)
        {
            WriteShort((short)rval.Size);
            Write(rval.Buff, rval.ReadPos, rval.Size);
            Skip(SizeAlign4B(rval.Size) - rval.Size);
        }
        public void WriteStringAlign4B(string rval, short type)
        {
            var encoder = EncodeHelper.GetEncoder(type);
            if (encoder == null)
            {
                throw new Exception("错误的类型, 与string不匹配");
            }

            byte[] buffer = encoder.GetBytes(rval);
            var byteSize = (short)(buffer.Length + 1);
            WriteShort(byteSize);
            Write(buffer);
            Fill(0, SizeAlign4B(byteSize) - buffer.Length);
        }
        public void Handle(ref BaseVariant rval)
        {
            var flag = (LYBVariant)rval.type;
            switch (flag)
            {
                case LYBVariant.VT_EMPTY:
                case LYBVariant.VT_NULL:
                    WriteInt((int)LYBVariant.VT_NULL);
                    break;
                case LYBVariant.VT_SHORTINTEGER:
                    {
                        short val = (short)rval.lVal;
                        Handle(ref val);
                        break;
                    }
                case LYBVariant.VT_WORD:
                    {
                        ushort val = (ushort)rval.lVal;
                        Handle(ref val);
                        break;
                    }
                case LYBVariant.VT_RPC_OP:
                    {
                        uint val = (uint)rval.lVal;
                        HandleRpcOP(ref val);
                        break;
                    }
                case LYBVariant.VT_INTEGER:
                case LYBVariant.VT_I32_24:
                    {
                        int val = (int)rval.lVal;
                        Handle(ref val);
                        break;
                    }
                case LYBVariant.VT_DWORD:
                case LYBVariant.VT_U32_24:
                    {
                        uint val = (uint)rval.lVal;
                        Handle(ref val);
                        break;
                    }
                case LYBVariant.VT_QWORD:
                case LYBVariant.VT_U64_24:
                case LYBVariant.VT_U64_56:
                    {
                        ulong val = (ulong)rval.lVal;
                        Handle(ref val);
                        break;
                    }
                case LYBVariant.VT_LARGINTEGER:
                case LYBVariant.VT_I64_24:
                case LYBVariant.VT_I64_56:
                    {
                        long val = rval.lVal;
                        Handle(ref val);
                        break;
                    }
                case LYBVariant.VT_FLOAT:
                    {
                        float val = (float)rval.dbVal;
                        Handle(ref val);
                        break;
                    }
                case LYBVariant.VT_DOUBLE:
                    {
                        double val = rval.dbVal;
                        Handle(ref val);
                        break;
                    }
                case LYBVariant.VT_UTF8:
                case LYBVariant.VT_STRING:
                case LYBVariant.VT_BSTRING:
                    {
                        string val = (string)rval.obj;
                        Handle(ref val);
                        break;
                    }
                case LYBVariant.VT_POINTER:
                    {
                        byte[] val = (byte[])rval.obj;
                        Handle(ref val);
                        break;
                    }
            }
        }
        
        unsafe void OnStreamWritePosChange(int pos)
        {
            fixed (byte* streamPos = &buffer[0])
            {
                var streamFirstShort = (short*)streamPos;
                var streamSecondShort = (short*)(streamPos + 2);
                *streamFirstShort = (short)pos;
                *streamSecondShort = (short)pos;
            }
            StreamSize = StreamMaxSize = pos;
        }
    }
    public unsafe class LYBStreamOut : LYBStream, ILYBStreamSerializerBase
    {
        public LYBStreamOut() { }
        public override void Reset()
        {
            base.Reset();
            HeaderSkiped = false;
        }
        public override void Reset(TMemoryBufferEx rhs)
        {
            base.Reset(rhs);
            HeaderSkiped = false;
        }

        public int StreamSize = 0;
        public int StreamMaxSize = 0;
        public bool HeaderSkiped = false;
        public void SkipHeader()
        {
            if (HeaderSkiped)
                return;
            StreamSize = ReadShort();
            StreamMaxSize = ReadShort();
            HeaderSkiped = true;
        }

        [Obsolete("未实现的函数")]
        public ILYBStreamSerializerBase GetStream() { throw new Exception("Don't call GetStream in LYBStreamIn;"); }

        public void Handle(ref sbyte rval)
        {
            var flag = (LYBVariant)ReadShort();
            Assert.IsTrue(
                flag == LYBVariant.VT_SHORTINTEGER);

            var valLen = ReadShort();
            rval = (sbyte)valLen;
        }
        public void HandleRpcOP(ref uint opType)
        {
            var flag = (LYBVariant)ReadShort();
            Assert.IsTrue(
                flag == LYBVariant.VT_RPC_OP);
            var valLen = ReadShort();
            opType = ReadUInt();
        }

        public void Handle(ref byte rval)
        {
            var flag = (LYBVariant)ReadShort();
            Assert.IsTrue(
                flag == LYBVariant.VT_WORD);

            var valLen = ReadShort();
            rval = (byte)valLen;
        }
        public void Handle(ref short rval)
        {
            var flag = (LYBVariant)ReadShort();
            Assert.IsTrue(
                flag == LYBVariant.VT_SHORTINTEGER);
            rval = ReadShort();
        }
        public void Handle(ref ushort rval)
        {
            var flag = (LYBVariant)ReadShort();
            Assert.IsTrue(
                flag == LYBVariant.VT_WORD);
            rval = ReadUShort();
        }
        public void Handle(ref int rval)
        {
            var flag = (LYBVariant)ReadByte();
            Assert.IsTrue(
                flag == LYBVariant.VT_INTEGER ||
                flag == LYBVariant.VT_RPC_OP ||
                flag == LYBVariant.VT_I32_24);
            switch (flag)
            {
                case LYBVariant.VT_INTEGER:
                case LYBVariant.VT_RPC_OP:
                    Skip(3);
                    rval = ReadInt();
                    break;
                case LYBVariant.VT_I32_24:
                    var low2 = ReadUShort();
                    var low1 = ReadSByte();//符号继承
                    rval = low1;
                    rval = rval << 16 | low2;
                    break;
            }
        }
        public void Handle(ref uint rval)
        {
            var flag = (LYBVariant)ReadByte();
            Assert.IsTrue(
                flag == LYBVariant.VT_DWORD ||
                flag == LYBVariant.VT_U32_24);

            switch (flag)
            {
                case LYBVariant.VT_DWORD:
                    Skip(3);
                    rval = ReadUInt();
                    break;
                case LYBVariant.VT_U32_24:
                    var low2 = ReadUShort();
                    var low1 = ReadByte();
                    rval = low1;
                    rval = rval << 16 | low2;
                    break;
            }
        }
        public void Handle(ref long rval)
        {
            var flag = (LYBVariant)ReadByte();
            switch (flag)
            {
                case LYBVariant.VT_LARGINTEGER:
                    Skip(3);
                    rval = ReadInt64();
                    break;
                case LYBVariant.VT_I64_24:
                    var low2 = ReadUShort();
                    var low1 = ReadSByte();
                    rval = low1;//符号继承
                    rval = rval << 16 | low2;
                    break;
                case LYBVariant.VT_I64_56:
                    var low = ReadUInt();
                    var high2 = ReadUShort();
                    var high1 = ReadSByte();
                    rval = high1;//符号继承
                    rval = rval << 16 | high2;
                    rval = rval << 32 | low;
                    break;
            }
        }
        public void Handle(ref ulong rval)
        {
            var flag = (LYBVariant)ReadShort();
            var valLen = ReadUShort();
            switch (flag)
            {
                case LYBVariant.VT_QWORD:
                    Skip(3);
                    rval = ReadUInt64();
                    break;
                case LYBVariant.VT_U64_24:
                    var low2 = ReadUShort();
                    var low1 = ReadByte();
                    rval = low1;
                    rval = rval << 16 | low2;
                    break;
                case LYBVariant.VT_U64_56:
                    var low = ReadUInt();
                    var high2 = ReadUShort();
                    var high1 = ReadByte();
                    rval = high1;
                    rval = rval << 16 | high2;
                    rval = rval << 32 | low;
                    break;
            }
        }

        public void Handle(ref float rval)
        {
            var flag = (LYBVariant)ReadInt();
            Assert.IsTrue(
                flag == LYBVariant.VT_FLOAT);
            rval = ReadFloat();
        }
        public void Handle(ref double rval)
        {
            var flag = (LYBVariant)ReadShort();
            Assert.IsTrue(
                flag == LYBVariant.VT_DOUBLE);
            var valLen = ReadUShort();
            rval = ReadDouble();
        }
        public void Handle(ref bool rval)
        {
            var flag = (LYBVariant)ReadShort();
            Assert.IsTrue(
                flag == LYBVariant.VT_WORD);
            rval = ReadUShort() != 0;
        }
        public void Handle(ref string rval)
        {
            var flag = (LYBVariant)ReadShort();
            int byteSize = 0;
            switch (flag)
            {
                case LYBVariant.VT_UTF8:
                    rval = ReadStringUtf8(out byteSize);
                    break;
                case LYBVariant.VT_STRING:
                    rval = ReadStringGBK(out byteSize);
                    break;
                case LYBVariant.VT_BSTRING:
                    rval = ReadString(out byteSize);
                    break;
                default:
                    Debugger.LogError("错误的类型, 与string不匹配");
                    return;
            }
            var alignedSize = SizeAlign4B(byteSize);
            Read(null, 0, alignedSize - byteSize);
        }
        public void Handle(ref char[] rval)
        {
            var flag = (LYBVariant)ReadShort();
            int byteSize = 0;
            switch (flag)
            {
                case LYBVariant.VT_UTF8:
                    rval = ReadCharsUtf8(out byteSize);
                    break;
                case LYBVariant.VT_STRING:
                    rval = ReadCharsGBK(out byteSize);
                    break;
                case LYBVariant.VT_BSTRING:
                    rval = ReadChars(out byteSize);
                    break;
                default:
                    Debugger.LogError("错误的类型, 与char[]不匹配");
                    return;
            }
            var alignedSize = SizeAlign4B(byteSize);
            Read(null, 0, alignedSize - byteSize);
        }

        public void Handle(ref byte[] rval)
        {
            var flag = (LYBVariant)ReadShort();
            Assert.IsTrue(
                flag == LYBVariant.VT_POINTER);

            var valLen = ReadShort();
            rval = new byte[valLen];
            Read(rval, 0, valLen);
            Skip(SizeAlign4B(valLen) - valLen);
        }
        public void Handle(ref TBinaryData rval)
        {
            var flag = (LYBVariant)ReadShort();
            Assert.IsTrue(
                flag == LYBVariant.VT_POINTER);

            if (rval == null)
                rval = new TBinaryData();
            else
                rval.Reset();
            var valLen = ReadShort();
            rval.Reserve(valLen);
            Read(rval.Buff, 0, valLen);
            rval.Write(null, 0, valLen);
            Skip(SizeAlign4B(valLen) - valLen);
        }
        public void Handle(ref LYBStream rval)
        {
            var flag = (LYBVariant)ReadShort();
            Assert.IsTrue(
                flag == LYBVariant.VT_POINTER);

            if (rval == null)
                rval = new LYBStream();
            else
                rval.Reset();

            var valCount = ReadShort();
            var maxCount = ReadShort();
            rval.Reserve(valCount);

            rval.WriteShort(valCount);
            rval.WriteShort(valCount);

            var bodyLen = valCount - sizeof(short) * 2;
            Read(rval.Buff, rval.WritePos, bodyLen);
            rval.Write(null, 0, bodyLen);

            var leftCount = maxCount - valCount;
            if (leftCount > 0)
                Skip(leftCount);
            Skip(SizeAlign4B(maxCount) - maxCount);
        }

        public void Handle<T>(ref T rval) where T : ILYBSerializable, new()
        {
            if (rval == null)
                rval = new T();
            rval.Serialize(this);
        }

        //-----------------------------------------
        public void Handle(ref short[] rval)
        {
            var valCount = ReadShort();
            rval = new short[valCount];
            for (int i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref ushort[] rval)
        {
            var valCount = ReadShort();
            rval = new ushort[valCount];
            for (int i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref int[] rval)
        {
            var valCount = ReadShort();
            rval = new int[valCount];
            for (int i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref uint[] rval)
        {
            var valCount = ReadShort();
            rval = new uint[valCount];
            for (int i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref long[] rval)
        {
            var valCount = ReadShort();
            rval = new long[valCount];
            for (int i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref ulong[] rval)
        {
            var valCount = ReadShort();
            rval = new ulong[valCount];
            for (int i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref float[] rval)
        {
            var valCount = ReadShort();
            rval = new float[valCount];
            for (int i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref double[] rval)
        {
            var valCount = ReadShort();
            rval = new double[valCount];
            for (int i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle<T>(ref T[] rval) where T : ILYBSerializable, new()
        {
            var valCount = ReadShort();
            rval = new T[valCount];
            for (int i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }

        //-----------------------------------------------------
        [Obsolete("未实现的函数")]
        public void Handle(ref string rval, short strLen)
        {
            throw new Exception("Not implemented.");
        }

        [Obsolete("未实现的函数")]
        public void Handle(ref char[] rval, short strLen)
        {
            throw new Exception("Not implemented.");
        }
        public void Handle(ref byte[] rval, short valCount)
        {
            throw new Exception("Not implemented.");
        }
        public void Handle(ref short[] rval, short valCount)
        {
            rval = (rval == null || rval.Length < valCount) ? new short[valCount] : rval;
            for (int i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref ushort[] rval, short valCount)
        {
            rval = (rval == null || rval.Length < valCount) ? new ushort[valCount] : rval;
            for (int i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref int[] rval, short valCount)
        {
            rval = (rval == null || rval.Length < valCount) ? new int[valCount] : rval;
            for (int i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref uint[] rval, short valCount)
        {
            rval = (rval == null || rval.Length < valCount) ? new uint[valCount] : rval;
            for (int i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref long[] rval, short valCount)
        {
            rval = (rval == null || rval.Length < valCount) ? new long[valCount] : rval;
            for (int i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref ulong[] rval, short valCount)
        {
            rval = (rval == null || rval.Length < valCount) ? new ulong[valCount] : rval;
            for (int i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref float[] rval, short valCount)
        {
            rval = (rval == null || rval.Length < valCount) ? new float[valCount] : rval;
            for (int i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle(ref double[] rval, short valCount)
        {
            rval = (rval == null || rval.Length < valCount) ? new double[valCount] : rval;
            for (int i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        public void Handle<T>(ref T[] rval, short valCount) where T : ILYBSerializable, new()
        {
            rval = (rval == null || rval.Length < valCount) ? new T[valCount] : rval;
            for (int i = 0; i < valCount; ++i)
                Handle(ref rval[i]);
        }
        //-----------------------------------------------------
        public void Skip(int byteCount)
        {
            ReadPos += byteCount;
        }
        public TBinaryData ReadBinaryAlign4B()
        {
            TBinaryData rval = new TBinaryData();
            var valLen = ReadShort();
            rval.Reserve(valLen);
            Read(rval.Buff, 0, valLen);
            rval.Write(null, 0, valLen);
            Skip(SizeAlign4B(valLen) - valLen);
            return rval;
        }
        public string ReadStringAlign4B(short flag)
        {
            var encoder = EncodeHelper.GetEncoder(flag);
            if (encoder == null)
            {
                throw new Exception("错误的类型, 与string不匹配");
            }

            int byteSize = 0;
            var rval = DoReadString(encoder, out byteSize);
            var alignedSize = SizeAlign4B(byteSize);
            Read(null, 0, alignedSize - byteSize);
            return rval;
        }
        public void Handle(ref BaseVariant rval)
        {
            rval.type = ReadByte();
            ReadPos--;
            var flag = (LYBVariant)rval.type;
            switch (flag)
            {
                case LYBVariant.VT_EMPTY:
                case LYBVariant.VT_NULL:
                    Skip(3);
                    break;
                case LYBVariant.VT_SHORTINTEGER:
                    {
                        short val = 0;
                        Handle(ref val);
                        rval.lVal = val;
                        break;
                    }
                case LYBVariant.VT_WORD:
                    {
                        ushort val = 0;
                        Handle(ref val);
                        rval.lVal = val;
                        break;
                    }
                case LYBVariant.VT_RPC_OP:
                    {
                        uint val = 0;
                        HandleRpcOP(ref val);
                        rval.lVal = val;
                        break;
                    }
                case LYBVariant.VT_INTEGER:
                case LYBVariant.VT_I32_24:
                    {
                        int val = 0;
                        Handle(ref val);
                        rval.lVal = val;
                        break;
                    }
                case LYBVariant.VT_DWORD:
                case LYBVariant.VT_U32_24:
                    {
                        uint val = 0;
                        Handle(ref val);
                        rval.lVal = val;
                        break;
                    }
                case LYBVariant.VT_QWORD:
                case LYBVariant.VT_U64_24:
                case LYBVariant.VT_U64_56:
                    {
                        ulong val = 0;
                        Handle(ref val);
                        rval.lVal = (long)val;
                        break;
                    }
                case LYBVariant.VT_LARGINTEGER:
                case LYBVariant.VT_I64_24:
                case LYBVariant.VT_I64_56:
                    {
                        long val = 0;
                        Handle(ref val);
                        rval.lVal = val;
                        break;
                    }
                case LYBVariant.VT_FLOAT:
                    {
                        float val = 0;
                        Handle(ref val);
                        rval.dbVal = val;
                        break;
                    }
                case LYBVariant.VT_DOUBLE:
                    {
                        double val = 0;
                        Handle(ref val);
                        rval.dbVal = val;
                        break;
                    }
                case LYBVariant.VT_UTF8:
                case LYBVariant.VT_STRING:
                case LYBVariant.VT_BSTRING:
                    {
                        string val = null;
                        Handle(ref val);
                        rval.obj = val;
                        break;
                    }
                case LYBVariant.VT_POINTER:
                    {
                        byte[] val = null;
                        Handle(ref val);
                        rval.obj = val;
                        break;
                    }
            }
        }

        public void SkipVariant()
        {
            var flag = (LYBVariant)ReadByte();
            switch(flag)
            {
                case LYBVariant.VT_EMPTY:
                case LYBVariant.VT_NULL:
                case LYBVariant.VT_SHORTINTEGER:
                case LYBVariant.VT_WORD:
                case LYBVariant.VT_I32_24:
                case LYBVariant.VT_U32_24:
                case LYBVariant.VT_U64_24:
                case LYBVariant.VT_I64_24:
                    Skip(3);
                    break;
                case LYBVariant.VT_INTEGER:
                case LYBVariant.VT_RPC_OP:
                case LYBVariant.VT_DWORD:
                case LYBVariant.VT_U64_56:
                case LYBVariant.VT_I64_56:
                case LYBVariant.VT_FLOAT:
                    Skip(7);
                    break;
                case LYBVariant.VT_QWORD:
                case LYBVariant.VT_LARGINTEGER:
                case LYBVariant.VT_DOUBLE:
                    Skip(11);
                    break;
                case LYBVariant.VT_UTF8:
                    {
                        Skip(1);
                        int byteSize = 0;
                        ReadStringUtf8(out byteSize);
                        var alignedSize = SizeAlign4B(byteSize);
                        Skip(alignedSize - byteSize);
                        break;
                    }
                case LYBVariant.VT_STRING:
                    {
                        Skip(1);
                        int byteSize = 0;
                        ReadStringGBK(out byteSize);
                        var alignedSize = SizeAlign4B(byteSize);
                        Skip(alignedSize - byteSize);
                        break;
                    }
                case LYBVariant.VT_BSTRING:
                    {
                        Skip(1);
                        int byteSize = 0;
                        ReadString(out byteSize);
                        var alignedSize = SizeAlign4B(byteSize);
                        Skip(alignedSize - byteSize);
                        break;
                    }
                case LYBVariant.VT_POINTER:
                    {
                        Skip(1);
                        var valLen = ReadShort();
                        Skip(SizeAlign4B(valLen));
                        break;
                    }
            }
        }
    }
}
