using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Network.Net
{
    public class NetServerLink : NetLinkBase
    {
        public NetServerLink(Socket socket, bool reusable, bool needCheckValid)
            : base(reusable, needCheckValid)
        {
            this.socket = socket;
            this.isConnected = true;

            //socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            //socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, 0);
            socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
            DoOnceRecv();
        }
        

        protected override void OnConnected(bool isOk)
        {
            base.OnConnected(isOk);
            for (int i = 0; i < Listenners.Count; ++i)
            {
                Listenners[i].OnConnected(linkID, isOk);
            }
        }

        protected override void OnDisconnect()
        {
            base.OnDisconnect();
            for (int i = 0; i < Listenners.Count; ++i)
            {
                Listenners[i].OnDisconnected(linkID);
            }
        }

        protected override void OnDestory()
        {
            base.OnDestory();
            for (int i = 0; i < Listenners.Count; ++i)
            {
                Listenners[i].OnDestory(linkID);
            }
        }

        protected override void OnReuseSessionSuccess(UInt64 otherSessionKey)
        {
            for (int i = 0; i < Listenners.Count; ++i)
            {
                Listenners[i].OnReuseSessionSuccess(linkID, otherSessionKey);
            }
        }

        protected override void OnReuseSessionFailed(UInt64 otherSessionKey)
        {
            for (int i = 0; i < Listenners.Count; ++i)
            {
                Listenners[i].OnReuseSessionFailed(linkID, otherSessionKey);
            }
        }

        protected override void OnReuseSessionRsp(ulong sessionID)
        {
            if (sessionID == 0)
            {
                PushFlagPacket(FlagPacketTypes.FPT_REUSE_FAILED, sessionID);
                return;
            }

            NetClientLink offlineServer = NetManager.Instance.GetClientLinkSession(sessionID);
            if (offlineServer == null)
                return;

            RestoreRecvData(offlineServer);

            PushFlagPacket(FlagPacketTypes.FPT_REUSE_SUCCESS, sessionID);
        }

        protected override void OnSessionKey(UInt64 sessionKey)
        {
            base.OnSessionKey(sessionKey);
            NetManager.Instance.AddSessionIdx(sessionKey, this);
        }
    }
}
