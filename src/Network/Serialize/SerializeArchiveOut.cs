using Thrift.Protocol;
namespace Network.Net
{
    public unsafe class SerializeArchiveOut : SerializeArchiveBase, ISerializeArchiveBase
    {
        public SerializeArchiveOut()
        {
            tb = new TBinaryProtocol(this);
        }

        public SerializeArchiveOut(byte[] buff)
            : base(buff)
        {
            tb = new TBinaryProtocol(this);
        }

        public void HandleThirft<T>(ref T val) where T : TBase
        {
            val.Read(tb);
        }
    }
}