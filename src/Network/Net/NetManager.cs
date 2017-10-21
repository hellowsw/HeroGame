using System;
using System.Collections.Generic;
using System.Threading;
using Network.Log;
using Network.Timer;

namespace Network.Net
{
    public class NetManager : Singleton<NetManager>, ILinkListenner
    {
        Map<UInt64, NetLinkBase> links = new Map<UInt64, NetLinkBase>();
        List<UInt64> linksNeedRemove = new List<UInt64>();
        
        Thread sendPrepareThread = null;
        public NetManager()
        {
        }

        public void Init()
        {
            sendPrepareThread = new Thread(SendWork);
            sendPrepareThread.Start();
        }

        #region IServerListenner
        public void OnConnected(UInt64 linkID, bool isOk)
        {
            if (!isOk)
                linksNeedRemove.Add(linkID);
        }

        public void OnDisconnected(UInt64 linkID)
        {
        }

        public void OnDestory(UInt64 linkID)
        {
            linksNeedRemove.Add(linkID);
            NetLinkBase link = Get(linkID);
            if (link == null)
                return;
            RemoveSessionIdx(link);
        }

        public void OnSendRequest(NetLinkBase link)
        {
            lock (prepareRequest)
            {
                prepareRequest.Add(link);
            }
        }

        public void OnReuseSessionSuccess(UInt64 linkID, UInt64 otherSessionKey) { }
        public void OnReuseSessionFailed(UInt64 linkID, UInt64 otherSessionKey) { }
        #endregion

        //----------------------------------------
        #region 连接管理
        NetLinkBase Get(UInt64 linkID)
        {
            NetLinkBase ret = null;
            links.TryGetValue(linkID, out ret);
            return ret;
        }

        public bool IsConnected(UInt64 linkID)
        {
            NetLinkBase link = Get(linkID);
            if (link == null)
                return false;
            return link.IsConnected();
        }

        public void Disconnect(UInt64 linkID)
        {
            NetLinkBase link = Get(linkID);
            if (link == null)
                return;
            link.Disconnect();
        }

        // 每10毫秒执行一次网络处理
        UInt64 executeTick = 0;

        public void Execute()
        {
            UInt64 now = TickerPolicy.RealTicker.GetTick();
            if (executeTick > now)
                return;
            executeTick = now + 10;

            // 接收的进入连接
            for (int i = 0; i < acceptedLinks.Count; i++)
            {
                NetServerLink newLink = acceptedLinks[i];
                links.Add(newLink.LinkID, newLink);
            }
            acceptedLinks.Clear();

            isExecuting = true;
            var iter = links.GetEnumerator();
            for (; iter.MoveNext(); )
            {
                //iter.Current.Value.DoSendRequest();
                iter.Current.Value.HandlePackets();
            }
            isExecuting = false;

            // 发起连接的客户端
            for (int i = 0; i < connectingClients.Count; i++)
            {
                NetClientLink newClient = connectingClients[i];
                if (newClient.Connect())
                    links.Add(newClient.LinkID, newClient);
            }
            connectingClients.Clear();

            // 需删除的连接
            for (int i = 0; i < linksNeedRemove.Count; i++ )
            {
                UInt64 linkID = linksNeedRemove[i];
                links.Remove(linkID);
            }
            linksNeedRemove.Clear();
        }

        public void Clear()
        {
            sendValid = false;
            var iter = links.GetEnumerator();
            for (; iter.MoveNext(); )
            {
                iter.Current.Value.Disconnect();
            }

            UInt64 waitTime = TickerPolicy.SysTicker.GetTick();
            while (links.Count > 0)
            {
                if ((TickerPolicy.SysTicker.GetTick() - waitTime) > 5000)
                    break;
                Execute();
                Thread.Sleep(1);
            }

            sendPrepareThread.Join();
        }

        public void DisconnectEx(NetLinkBase link)
        {
            if (link == null)
                return;
            link.Disconnect();
        }
        #endregion

        #region 发送请求管理
        bool sendValid = true;
        List<NetLinkBase> prepareRequest = new List<NetLinkBase>();
        void SendWork()
        {
            bool needWait = false;
            while (true)
            {
                if (needWait)
                {
                    needWait = false;
                    Thread.Sleep(1);
                }

                if (prepareRequest.Count == 0)
                {
                    if (sendValid)
                    {
                        needWait = true;
                        continue;
                    }
                    break;
                }

                NetLinkBase netLink = null;
                lock (prepareRequest)
                {
                    netLink = prepareRequest[0];
                    prepareRequest.RemoveAt(0);
                }

                try
                {
                    // 获取新数据进行发送
                    netLink.DoSendRequest();
                }
                catch (Exception ex)
                {
                    LogManager.Instance.LogInfo(ex.Message);
                    LogManager.Instance.LogInfo(ex.StackTrace);
                    throw ex;
                }
            }
        }
        #endregion

        #region 连接索引管理
        Map<UInt64, NetClientLink> clientLinkSessionIdxes = new Map<UInt64, NetClientLink>();
        Map<UInt64, NetServerLink> serverLinkSessionIdxes = new Map<UInt64, NetServerLink>();
        internal void RemoveSessionIdx(NetLinkBase link)
        {
            if (link is NetClientLink)
                clientLinkSessionIdxes.Remove(link.SessionKey);
            else if (link is NetServerLink)
                serverLinkSessionIdxes.Remove(link.SessionKey);
        }

        internal void AddSessionIdx(ulong sessionKey, NetLinkBase link)
        {
            if (link is NetClientLink)
                clientLinkSessionIdxes[sessionKey] = (NetClientLink)link;
            else if (link is NetServerLink)
                serverLinkSessionIdxes[sessionKey] = (NetServerLink)link;
        }

        internal NetClientLink GetClientLinkSession(ulong sessionKey)
        {
            NetClientLink ret = null;
            clientLinkSessionIdxes.TryGetValue(sessionKey, out ret);
            return ret;
        }

        internal NetServerLink GetServerLinkSession(ulong sessionKey)
        {
            NetServerLink ret = null;
            serverLinkSessionIdxes.TryGetValue(sessionKey, out ret);
            return ret;
        }
        #endregion

        #region 客户端连接创建
        public UInt64 Connect(string ip, ushort port, ILinkListenner listenner, bool reusable, bool needCheckValid)
        {
            NetClientLink newClient = ConnectEx(ip, port, listenner, reusable, needCheckValid);
            if (newClient == null)
                return 0;
            return newClient.LinkID;
        }

        // 用以避免在处理消息的过程中创建连接添加到clients, 导致遍历中的迭代器失效
        List<NetClientLink> connectingClients = new List<NetClientLink>();
        List<NetServerLink> acceptedLinks = new List<NetServerLink>();
        bool isExecuting = false;
        public NetClientLink ConnectEx(string ip, ushort port, ILinkListenner listenner, bool reusable, bool needCheckValid)
        {
            NetClientLink newClient = new NetClientLink(reusable, needCheckValid);
            newClient.Listenners.Add(this);

            if (listenner != null)
                newClient.Listenners.Add(listenner);
            newClient.MsgHandler = MsgDispatcher.Instance.OnMessage;
            newClient.SendReqHandler = OnSendRequest;
            newClient.PrepareConnect(ip, port);

            // 消息处理中, 不实际执行连接
            if (isExecuting)
            {
                connectingClients.Add(newClient);
                return newClient;
            }

            if (!newClient.Connect())
                return null;
            links.Add(newClient.LinkID, newClient);
            return newClient;
        }
        #endregion

        #region 服务器及连接管理
        public NetServer CreateServer(string ip, ushort port, ILinkListenner listenner, bool reusable, bool needCheckValid)
        {
            NetServer ret = new NetServer(ip, port,
                listenner, reusable,
                needCheckValid, OnAccepted,
                MsgDispatcher.Instance.OnMessage);
            if (!ret.Start())
                return null;
            return ret;
        }

        public void OnAccepted(NetServerLink link)
        {
            link.Listenners.Add(this);
            acceptedLinks.Add(link);
        }
        #endregion
    }
}
