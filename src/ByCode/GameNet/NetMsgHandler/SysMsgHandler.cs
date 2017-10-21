using Network.Net;
using Network.DataDefs;
using System;
using UnityEngine;
using Network.DataDefs.NetMsgDef;

namespace UnityDLL.NetMsgHandler
{
    static class SysMsgHandler
    {
        public static void Init()
        {
            LYBMsgDispatcher.Instance.RegMsgHandler(LYBGlobalConsts.TICK, OnSATickRsp);
            LYBMsgDispatcher.Instance.RegMsgHandler(LYBGlobalConsts.PONG, OnSAPongRsp);
        }

        private static void OnSATickRsp(UInt64 linkID, TMemoryBufferEx packetBuff)
        {
            SATickMsg msg = null;
            if (!LYBMsgDispatcher.GetMsg(packetBuff, ref msg))
                return;
            Debug.LogFormat("Server Tick: {0}", msg.server_tick_);
        }

        private static void OnSAPongRsp(UInt64 linkID, TMemoryBufferEx packetBuff)
        {
            SAPongMsg msg = null;
            if (!LYBMsgDispatcher.GetMsg(packetBuff, ref msg))
                return;
            Debug.LogFormat("Last Pong Recv Tick: {0}", msg.last_recv_tick_);
        }
    }
}
