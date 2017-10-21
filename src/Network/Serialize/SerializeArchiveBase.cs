using System.Text;
using Thrift.Protocol;
namespace Network.Net
{
    public interface ISerializable
    {
        void Serialize(ISerializeArchiveBase ar);
    }

    public interface ISerializeArchiveBase
    {
        void Reset(byte[] newBuff, int offset, int dataSize);
        void HandleThirft<T>(ref T val) where T : TBase;
    }

    //须线程安全
    public class SerializeArchiveBase : TMemoryBufferEx
    {
        protected TBinaryProtocol tb = null;

        public SerializeArchiveBase()
            : this(8192)
        {
        }
        public SerializeArchiveBase(int initSize)
            :base(initSize)
        {
        }

        public SerializeArchiveBase(byte[] buffIn)
            : base(buffIn)
        {
        }

        public override string ToString()
        {
            return Encoding.ASCII.GetString(Buff, 0, Size);
        }
    }
}