namespace Network.Net
{
    public class SessionState
    {
        public const byte
            SESSION_UNCONNECT = 0,
            SESSION_WAIT_CONNECT = 1,
            SESSION_CONNECTED = 2,
            SESSION_WAIT_DISCONNECT = 3,
            SESSION_DISCONNECTED = 4;
    }

    public enum FlagPacketTypes
    {
        FPT_CONNECT_OK,
        FPT_CONNECT_FAILED,
        FPT_DISCONNECT,
        FPT_DESTORY,
        FPT_REUSE_SUCCESS,
        FPT_REUSE_FAILED,
    }

    public class PacketErrorCode
    {
        public const byte
            PACKET_OK = 0,
            PACKET_ALREADY_RECVD = 1,
            PAKCET_LENGTH_ERROR = 2,
            PACKET_SEQ_NUMBER_ERROR = 3,
            PACKET_DECRYPT_ERROR = 4,
            PAKCET_UNCOMPRESS_ERROR = 5,
            PACKET_KEY_LENGTH_ERROR = 6,
            PACKET_KEY_POS_ERROR = 7;
    }

    public class FlagCode
    {
        public const byte
            FLAG_COMPRESSED = 0x1,
            FLAG_CRYPTED = 0x2,
            FLAG_PATKET_SESSION_KEY = 0x4,
            FLAG_PACKET_REUSE_SESSION = 0x8,
            FLAG_PACKET_REUSE_SESSION_RSP = 0x10,
            FLAG_PATKET_CONNECT_CHECK = 0x20,
            FLAG_PACKET_REUSE_RESEND_DATA = 0x80,
            FLAG_PACKET_EVENT = 0xFE;// 客户端专用, 区别功能事件
    }

    public unsafe struct PacketHeader
    {
        public const int MAX_PACKET_LENGTH = 1024 * 1024;
        public const int MAX_KEY_LENGTH = 256;
        public const int MAX_VAR_KEY_LENGTH = 3;
        public const int INVALID_KEY_LENGTH = -1;

        public int bodyLen;
        public ushort seqNo;
        public byte flag;		//加密压缩标志
        public byte reserve;
        public uint crcVal;
        public int keyPos;
        public fixed byte key[MAX_VAR_KEY_LENGTH + 1];

        public byte CheckHeaderBase()
        {
            if (bodyLen > MAX_PACKET_LENGTH)
                return PacketErrorCode.PAKCET_LENGTH_ERROR;

            return PacketErrorCode.PACKET_OK;
        }

        public bool NeedEncrypt()
        {
            return (flag & FlagCode.FLAG_CRYPTED) != 0;
        }

        public bool NeedCompress()
        {
            return (flag & FlagCode.FLAG_COMPRESSED) != 0;
        }
        
        public unsafe int GetKey(byte* body, ref int bodySize, ref byte[] keyOut)
        {
            // 获取正确的key
            int keySize = 4;
            if (body == null ||
                keyPos > bodySize ||
                keyOut.Length < (MAX_VAR_KEY_LENGTH + 1))
                return INVALID_KEY_LENGTH;

            int i = 0;
            fixed (byte* ptr = key)
            {
                for (i = 0; i < (MAX_VAR_KEY_LENGTH + 1); ++i)
                    keyOut[i] = ptr[i];
            }

            if (keyPos == 0)
                return keySize;

            int leftKeySize = bodySize - keyPos;
            keySize += leftKeySize;

            if (keySize > MAX_KEY_LENGTH)
                return INVALID_KEY_LENGTH;

            bodySize -= leftKeySize;

            for (i = 0; i < leftKeySize; ++i)
                keyOut[MAX_VAR_KEY_LENGTH + 1 + i] = body[keyPos + i];
            return keySize;
        }
    }
}
