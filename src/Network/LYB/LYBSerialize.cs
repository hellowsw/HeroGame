using System;
using System.Runtime.InteropServices;
using Network.Serialize;

namespace Network.Net
{
    public unsafe class TBinaryData : TMemoryBufferEx
    {
        public TBinaryData() { }
        public TBinaryData(int initSize):base(initSize) { }

        protected int SizeAlign4B(int size)
        {
            return ((((size - 1) >> 2) + 1) << 2);
        }

        public bool IsEmpty()
        {
            return Size != 0;
        }
    }
    public class LYBStream : TBinaryData
    {

    }

    public interface ILYBSerializable
    {
        void Serialize(ILYBSerializerBase ar);
    }
    
    public interface ILYBSerializerBase
    {
        void Handle(ref sbyte rval);
        void Handle(ref byte rval);
        void Handle(ref short rval);
        void Handle(ref ushort rval);
        void Handle(ref int rval);
        void Handle(ref uint rval);
        void Handle(ref long rval);
        void Handle(ref ulong rval);
        void Handle(ref float rval);
        void Handle(ref double rval);
        void Handle(ref bool rval);
        void Handle(ref string rval);
        void Handle(ref char[] rval);
        void Handle(ref byte[] rval);
        void Handle(ref TBinaryData rval);
        void Handle(ref LYBStream rval);
        void Handle<T>(ref T rval) where T : ILYBSerializable, new();
        void Handle<T>(ref T[] rval) where T : ILYBSerializable, new();

        //-----------------------------------------------------
        void Handle(ref short[] rval);
        void Handle(ref ushort[] rval);
        void Handle(ref int[] rval);
        void Handle(ref uint[] rval);
        void Handle(ref long[] rval);
        void Handle(ref ulong[] rval);
        void Handle(ref float[] rval);
        void Handle(ref double[] rval);

        void Handle(ref string rval, short valCount);
        void Handle(ref char[] rval, short valCount);
        void Handle(ref byte[] rval, short valCount);
        void Handle(ref short[] rval, short valCount);
        void Handle(ref ushort[] rval, short valCount);
        void Handle(ref int[] rval, short valCount);
        void Handle(ref uint[] rval, short valCount);
        void Handle(ref long[] rval, short valCount);
        void Handle(ref ulong[] rval, short valCount);
        void Handle(ref float[] rval, short valCount);
        void Handle(ref double[] rval, short valCount);
        void Handle<T>(ref T[] rval, short valCount) where T : ILYBSerializable, new();
        //-----------------------------------------------------
        void Skip(int byteCount);
        ILYBStreamSerializerBase GetStream();
        bool IsEmpty();
    }
    
    public interface ILYBStreamSerializerBase : ILYBSerializerBase
    {
        void HandleRpcOP(ref uint opType);
        void Handle(ref BaseVariant rval);
        void SkipVariant();
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct SNetClientPacket
    {
        public IntPtr m_pvBuffer;
        public ushort m_wLength;
    }

    public class LYBMsgHeader : ILYBSerializable
    {
        public byte firstProtol;
        public byte secondProtol;
        public LYBMsgHeader() { }
        public LYBMsgHeader(ushort opcode)
        {
            this.firstProtol = (byte)(opcode >> 8 & 0xff);
            this.secondProtol = (byte)(opcode & 0xff);
        }

        public static ushort MakeOpCode(byte firstProtol, byte secondProtol)
        {
            ushort code = firstProtol;
            code = (ushort)((code << 8) | secondProtol);
            return code;
        }

        public ushort GetOpCode()
        {
            return MakeOpCode(firstProtol, secondProtol);
        }

        public virtual void Serialize(ILYBSerializerBase ar)
        {
            ar.Handle(ref firstProtol);
            ar.Handle(ref secondProtol);
        }
    }


    public unsafe class LYBSerializerIn : TBinaryData, ILYBSerializerBase
    {
        public LYBSerializerIn() { }

        protected ILYBStreamSerializerBase stream = null;//流处理器

        protected int streamBegin = -1;
        public void ResetStream()
        {
            streamBegin = -1;
        }

        public ILYBStreamSerializerBase GetStream()
        {
            if (stream == null)
                return null;

            //AS中ByteArray 前置4字节
            if (streamBegin == -1)
            {
                streamBegin = writePos;
                Skip(4);
            }
            ((LYBStreamIn)stream).ResetToContinue(this);
            return stream;
        }

        public void Handle(ref sbyte rval)
        {
            WriteSByte(rval);
        }
        public void Handle(ref byte rval)
        {
            WriteByte(rval);
        }
        public void Handle(ref short rval)
        {
            WriteShort(rval);
        }
        public void Handle(ref ushort rval)
        {
            WriteUShort(rval);
        }
        public void Handle(ref int rval)
        {
            WriteInt(rval);
        }
        public void Handle(ref uint rval)
        {
            WriteUInt(rval);
        }
        public void Handle(ref long rval)
        {
            WriteInt64(rval);
        }
        public void Handle(ref ulong rval)
        {
            WriteUInt64(rval);
        }
        public void Handle(ref float rval)
        {
            WriteFloat(rval);
        }
        public void Handle(ref double rval)
        {
            WriteDouble(rval);
        }
        public void Handle(ref bool rval)
        {
            WriteByte((byte)(rval ? 1 : 0));
        }
        public void Handle(ref string rval)
        {
            WriteStringGBK(rval);
        }
        public void Handle(ref char[] rval)
        {
            WriteStringGBK(rval);
        }
        public void Handle(ref byte[] rval)
        {
            WriteShort((short)rval.Length);
            Write(rval);
        }

        public void Handle(ref TBinaryData rval)
        {
            WriteShort((short)rval.Size);
            Write(rval.Buff, rval.ReadPos, rval.Size);
        }
        public void Handle(ref LYBStream rval)
        {
            Write(rval.Buff, rval.ReadPos, rval.Size);
        }
        public void Handle<T>(ref T rval) where T : ILYBSerializable, new()
        {
            rval.Serialize(this);
        }
        //-----------------------------------------
        public void Handle<T>(ref T[] rval) where T : ILYBSerializable, new()
        {
            var rvalLength = rval.Length;
            WriteShort((short)rvalLength);
            for (int i = 0; i < rvalLength; ++i)
                Handle(ref rval[i]);
        }
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

        //-----------------------------------------------------
        public void Handle(ref string rval, short strLen)
        {
            WriteStringGBK(rval, strLen);
        }
        public void Handle(ref char[] rval, short strLen)
        {
            WriteStringGBK(rval, strLen);
        }
        public void Handle(ref byte[] rval, short valCount)
        {
            Write(rval, 0, valCount);
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
    }
    public unsafe class LYBSerializerOut : TBinaryData, ILYBSerializerBase
    {
        public LYBSerializerOut() { }

        protected ILYBStreamSerializerBase stream = null;//流处理器

        protected int streamBegin = -1;
        public void ResetStream()
        {
            streamBegin = -1;
        }
        public ILYBStreamSerializerBase GetStream()
        {
            if (stream == null)
                return null;

            var streamOut = (LYBStreamOut)stream;

            //AS中ByteArray 前置4字节
            if (streamBegin == -1)
            {
                streamBegin = readPos;
                Skip(4);
            }
            streamOut.Reset(this);
            streamOut.HeaderSkiped = true;
            return stream;
        }

        public void Handle(ref sbyte rval)
        {
            rval = ReadSByte();
        }
        public void Handle(ref byte rval)
        {
            rval = ReadByte();
        }
        public void Handle(ref short rval)
        {
            rval = ReadShort();
        }
        public void Handle(ref ushort rval)
        {
            rval = ReadUShort();
        }
        public void Handle(ref int rval)
        {
            rval = ReadInt();
        }
        public void Handle(ref uint rval)
        {
            rval = ReadUInt();
        }
        public void Handle(ref long rval)
        {
            rval = ReadInt64();
        }
        public void Handle(ref ulong rval)
        {
            rval = ReadUInt64();
        }

        public void Handle(ref float rval)
        {
            rval = ReadFloat();
        }
        public void Handle(ref double rval)
        {
            rval = ReadDouble();
        }
        public void Handle(ref bool rval)
        {
            rval = ReadByte() != 0;
        }
        public void Handle(ref string rval)
        {
            rval = ReadStringGBK();
        }
        public void Handle(ref char[] rval)
        {
            rval = ReadCharsGBK();
        }

        public void Handle(ref byte[] rval)
        {
            var valCount = ReadShort();
            rval = (rval == null || rval.Length < valCount) ? new byte[valCount] : rval;
            Read(rval, 0, valCount);
        }

        public void Handle(ref TBinaryData rval)
        {
            var valCount = ReadShort();
            if (rval == null)
                rval = new TBinaryData();
            else
                rval.Reset();
            rval.Reserve(valCount);
            Read(rval.Buff, 0, valCount);
            rval.Write(null, 0, valCount);
        }
        public void Handle(ref LYBStream rval)
        {
            var valCount = ReadShort();
            var maxCount = ReadShort();
            if (rval == null)
                rval = new LYBStream();
            else
                rval.Reset();
            rval.Reserve(valCount);

            rval.WriteShort(valCount);
            rval.WriteShort(valCount);

            var bodyLen = valCount - sizeof(short) * 2;
            Read(rval.Buff, rval.WritePos, bodyLen);
            rval.Write(null, 0, bodyLen);

            var leftCount = maxCount - valCount;
            if (leftCount > 0)
                Skip(leftCount);
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
            Handle(ref rval, valCount);
        }
        public void Handle(ref ushort[] rval)
        {
            var valCount = ReadShort();
            Handle(ref rval, valCount);
        }
        public void Handle(ref int[] rval)
        {
            var valCount = ReadShort();
            Handle(ref rval, valCount);
        }
        public void Handle(ref uint[] rval)
        {
            var valCount = ReadShort();
            Handle(ref rval, valCount);
        }
        public void Handle(ref long[] rval)
        {
            var valCount = ReadShort();
            Handle(ref rval, valCount);
        }
        public void Handle(ref ulong[] rval)
        {
            var valCount = ReadShort();
            Handle(ref rval, valCount);
        }
        public void Handle(ref float[] rval)
        {
            var valCount = ReadShort();
            Handle(ref rval, valCount);
        }
        public void Handle(ref double[] rval)
        {
            var valCount = ReadShort();
            Handle(ref rval, valCount);
        }
        public void Handle<T>(ref T[] rval) where T : ILYBSerializable, new()
        {
            var valCount = ReadShort();
            Handle(ref rval, valCount);
        }

        #region Array处理
        //-----------------------------------------
        public void Handle(ref byte[] rval, short valCount)
        {
            rval = (rval == null || rval.Length < valCount) ? new byte[valCount] : rval;
            Read(rval, 0, valCount);
        }
        public void Handle(ref string rval, short strLen)
        {
            rval = ReadStringGBK(strLen);
        }
        public void Handle(ref char[] rval, short strLen)
        {
            rval = ReadCharsGBK(strLen);
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
        #endregion

        //-----------------------------------------------------
        public void Skip(int byteCount)
        {
            ReadPos += byteCount;
        }
    }

    public class LYBMsgSerializerIn : LYBSerializerIn
    {
        public LYBMsgSerializerIn()
        {
            var streamHandler = new LYBStreamIn();
            streamHandler.OnWritePosChange = OnStreamWritePosChange;
            streamHandler.OnReadPosChange = OnStreamReadPosChange;
            stream = streamHandler;
        }
        unsafe void OnStreamWritePosChange(int pos)
        {
            writePos = pos;

            fixed (byte* streamPos = &buffer[streamBegin])
            {
                var streamFirstShort = (short*)streamPos;
                var streamSecondShort = (short*)(streamPos + 2);
                *streamFirstShort = (short)(pos - streamBegin);
                *streamSecondShort = (short)(pos - streamBegin);
            }
        }
        void OnStreamReadPosChange(int pos)
        {
            readPos = pos;
        }
        public void WriteRpcStreamAlign4B(LYBStream rpcStream)
        {
            Handle(ref rpcStream);
        }
    }

    public class LYBMsgSerializerOut : LYBSerializerOut
    {
        public LYBMsgSerializerOut()
        {
            var streamHandler = new LYBStreamOut();
            streamHandler.OnWritePosChange = OnStreamWritePosChange;
            streamHandler.OnReadPosChange = OnStreamReadPosChange;
            stream = streamHandler;
        }
        void OnStreamWritePosChange(int pos)
        {
            writePos = pos;
        }
        unsafe void OnStreamReadPosChange(int pos)
        {
            readPos = pos;
        }

        public LYBStreamOut ReadRpcStreamAlign4B()
        {
            LYBStream rval = new LYBStreamOut();
            Handle(ref rval);
            return (LYBStreamOut)rval;
        }
    }
}
