using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Network.Log;

namespace Network.Net
{
    public class NetClientLink : NetLinkBase
    {
        bool isPreparedConnect = false;
        string ip;
        ushort port;

        public NetClientLink(bool reusable, bool needCheckValid)
            : base(reusable, needCheckValid)
        {

        }

        public bool Connect()
        {
            if (!isPreparedConnect)
                return false;
            return Connect(ip, port);
        }

        public bool Connect(string ip, int port)
        {
            try
            {
                if (socket != null)
                    return true;
                var ipa = IPAddress.Parse(ip);
                socket = new Socket(ipa.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.BeginConnect(ipa, port, OnConnectComplete, socket);
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.LogDebug("Net:Connect:ErrorCode:" + ex.Message);
                return false;
            }
            return true;
        }

        public void PrepareConnect(string ip, ushort port)
        {
            this.ip = ip;
            this.port = port;
            isPreparedConnect = true;
        }

        protected void OnConnectComplete(IAsyncResult ar)
        {
            try
            {
                if (socket == null)
                    return;
                socket.EndConnect(ar);

                //socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
                //socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, 0);
                socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);

                isConnected = true;
                PushFlagPacket(FlagPacketTypes.FPT_CONNECT_OK);
                DoOnceRecv();
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogDebug("Net:OnConnectComplete:Error: " + ex);
                PushFlagPacket(FlagPacketTypes.FPT_CONNECT_FAILED);
            }
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
