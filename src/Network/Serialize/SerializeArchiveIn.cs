using Thrift.Protocol;
namespace Network.Net
{
    public unsafe class SerializeArchiveIn : SerializeArchiveBase, ISerializeArchiveBase
    {
        public SerializeArchiveIn()
        {
            tb = new TBinaryProtocol(this);
        }
        /// <summary>
        /// thirft序列化
        /// </summary>
        /// <param name="val"></param>
        public void HandleThirft<T>(ref T val) where T : TBase
        {
            val.Write(tb);
        }
    }
}