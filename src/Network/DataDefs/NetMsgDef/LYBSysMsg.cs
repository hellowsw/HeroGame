using Network.DataDefs;
using Network.Net;

namespace Network.DataDefs.NetMsgDef
{

    // Ping Pong Tick 特殊消息
    public class SATickMsg : LYBMsgHeader
    {
        public static uint opCode = LYBGlobalConsts.TICK;

        public uint server_tick_;   // uServerTime 服务器时间 (服务器时刻)
        public override void Serialize(ILYBSerializerBase ar)
        {
            ar.Skip(sizeof(uint)); //Skip opCode
            ar.Handle(ref server_tick_);
        }
    }

    //用于保存ping的耗时，从而可以获取网络状态
    public class SAPongMsg : LYBMsgHeader
    {
        public static uint opCode = LYBGlobalConsts.PONG;

        /** 最近一次接收时间 */
        public int last_recv_tick_; // lastRecvTime 最近一次接收时间 (时刻)
        public override void Serialize(ILYBSerializerBase ar)
        {
            ar.Skip(sizeof(uint)); //Skip opCode
            ar.Handle(ref last_recv_tick_);
        }
    }
}
