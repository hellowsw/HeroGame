using System;
using Network.Log;
using Network.Timer;
using Thrift.Protocol;

namespace Network.Net
{
    public unsafe class RecvBuff
    {
        public byte[] buff;
        public int size;
        int readPos;
        int writePos;

        public RecvBuff()
            : this(16384)
        {
        }
        public RecvBuff(int initSize)
        {
            buff = new byte[initSize];
            readPos = writePos = size = 0;
        }

        public int ReadPos
        {
            get { return readPos; }
            set
            {
                if (value > writePos)
                {
                    Exception ex = new Exception("接收缓冲, 读取位置设置错误.");
                    LogManager.Instance.LogInfo(ex.Message);
                    LogManager.Instance.LogInfo(ex.StackTrace);
                    throw ex;
                }
                readPos = value; size = writePos - readPos;
            }
        }

        public int WritePos
        {
            get { return writePos; }
            set
            {
                if (value > buff.Length)
                {
                    Exception ex = new Exception("接收缓冲, 写入位置设置错误.");
                    LogManager.Instance.LogInfo(ex.Message);
                    LogManager.Instance.LogInfo(ex.StackTrace);
                    throw ex;
                }
                writePos = value; size = writePos - readPos;
            }
        }

        public int PreFreeSpace
        {
            get { return readPos; }
        }

        public int LeftSpace
        {
            get { return buff.Length - writePos; }
            set
            {
                if ((buff.Length - writePos) < value)
                {
                    int newSize = size + value;
                    byte[] newBuff = new byte[newSize];
                    Buffer.BlockCopy(buff, readPos, newBuff, 0, size);

                    buff = newBuff;
                    readPos = 0;
                    writePos = size;
                }
            }
        }

        public void MoveDataToBegin()
        {
            try
            {
                //safe模式
                if (readPos == 0)
                    return;
                for (int i = 0; i < size; ++i)
                {
                    buff[i] = buff[readPos + i];
                }
                readPos = 0;
                writePos = size;

                //Unsafe模式
                //fixed (byte* beginPtr = &buff[0], dataPtr = &buff[readPos])
                //{
                //    byte* destPtr = beginPtr;
                //    byte* srcPtr = dataPtr;

                //    int count = size;
                //    while (count-- > 0)
                //        *destPtr++ = *srcPtr++;
                //}
                //readPos = 0;
                //writePos = size;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogInfo(ex.Message);
                LogManager.Instance.LogInfo(ex.StackTrace);
                throw ex;
            }
        }
    }

    public interface ILinkListenner
    {
        void OnConnected(UInt64 linkID, bool isOk);
        void OnDisconnected(UInt64 linkID);
        void OnDestory(UInt64 linkID);
        void OnReuseSessionSuccess(UInt64 linkID, UInt64 otherSessionKey);
        void OnReuseSessionFailed(UInt64 linkID, UInt64 otherSessionKey);
    }

    public enum NetErrors
    {
        NE_NET_INTERRUPT,
        NE_PACKET_ERROR,
    }

    public class MsgHeader : TBase
    {
        public byte dir;
        public byte type;
        public ushort id;
        public int sendTimeStamp;
        public MsgHeader() { }
        public MsgHeader(uint opcode)
        {
            this.dir = (byte)(opcode >> 24 & 0xff);
            this.type = (byte)(opcode >> 16 & 0xff);
            this.id = (ushort)(opcode & 0xffff);
            this.sendTimeStamp = (int)TickerPolicy.SysTicker.GetTick32();
        }

        public uint GetOpCode()
        {
            return MakeOpCode(dir, type, id);
        }

        public uint GetSendTimeStamp()
        {
            return (uint)sendTimeStamp;
        }

        public void Write(TProtocol protocol)
        {
            protocol.WriteByte((sbyte)dir);
            protocol.WriteByte((sbyte)type);
            protocol.WriteI16((short)id);
            protocol.WriteI32(sendTimeStamp);
        }

        public void Read(TProtocol protocol)
        {
            dir = (byte)protocol.ReadByte();
            type = (byte)protocol.ReadByte();
            id = (ushort)protocol.ReadI16();
            sendTimeStamp = protocol.ReadI32();
        }

        public virtual void Serialize(ISerializeArchiveBase ar)
        {
            MsgHeader header = this;
            ar.HandleThirft<MsgHeader>(ref header);
        }

        public static uint MakeOpCode(byte dir, byte type, ushort id)
        {
            uint code = dir;
            code = (code << 8) | type;
            code = (code << 16) | id;
            return code;
        }

        static MsgHeader headerAssistor = new MsgHeader();
        public static uint GetDataOpCode(TMemoryBufferEx packetBuff)
        {
            ar.Reset(packetBuff.GetBuffer(), packetBuff.ReadPos, packetBuff.Size);
            headerAssistor.Serialize(ar);
            return headerAssistor.GetOpCode();
        }

        private static SerializeArchiveOut ar = new SerializeArchiveOut();
        public static bool GetMsg<T>(TMemoryBufferEx packetBuff, ref T msg) where T : MsgHeader, new()
        {
            try
            {
                if (msg == null)
                    msg = new T();
                ar.Reset(packetBuff.GetBuffer(), packetBuff.ReadPos, packetBuff.Size);
                msg.Serialize(ar);
                return true;
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.LogError("NetManager:GetMsg:Error:" + ex.Message);
                return false;
            }
        }
    }
}
