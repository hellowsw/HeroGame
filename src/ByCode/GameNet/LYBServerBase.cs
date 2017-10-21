using Network.Net;
using Network.Timer;
using LuaInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;
using UnityDLL.LuaCustom;

namespace UnityDLL
{
    public class LYBServerBase : ILinkListenner
    {
        public bool AutoReconnect = true;
        protected string serverName;
        protected string ip;
        protected ushort port;
        private LYBClientLink server;

        protected const int MaxReconnectTimes = 10; //重连次数
        protected int connectTryCount = MaxReconnectTimes; //当前次数
        //protected double timeOut = 5000; //超时时间
        protected double reconnectInterval = 3000; //重连间隔

        private bool isConnecting = false;

        ulong requestReconnectTimerID = 0;
        //ulong connectTimeOutTimerID = 0;

        public LYBClientLink Server
        {
            get
            {
                return server;
            }
        }

        public LYBServerBase(string serverName)
        {
            this.serverName = serverName;
        }
        public bool IsServerLink(ulong linkID)
        {
            return Server != null &&
                Server.LinkID == linkID;
        }

        internal bool IsConnecting()
        {
            return isConnecting;
        }

        internal bool IsConnected()
        {
            return Server != null && Server.IsConnected();
        }

        protected virtual void Reset()
        {
            connectTryCount = MaxReconnectTimes;
        }
        
        protected void RaiseScriptEvent(string eventFuncName)
        {
            LuaTable table = Lua.lua.GetTable("NetInterface");
            Lua.Call(table, eventFuncName, serverName);
        }

        internal virtual void Connect(string ip, ushort port)
        {
            if (IsConnected() || IsConnecting())
            {
                Debug.Log("Connect IsConnected:" + IsConnected() + ", IsConnecting:" + IsConnecting());
                return;
            }
            if (connectTryCount == 0)
            {
                Debug.Log("Connect connectTryCount is zero.");
                //刘睿 2016-11-5 改
                RaiseScriptEvent("OnConnectFailed");
                //-----
                return;
            }

            ClearTimers();
            isConnecting = true;
            //TODO connectTimeOutTimerID = TimerManager.Instance.AddTimer(timeOut, ConnectTimeOut, null);
            this.ip = ip;
            this.port = port;

            if (AutoReconnect)
                connectTryCount--;

            

            server = new LYBClientLink(this);
            Server.MsgHandler = LYBMsgDispatcher.Instance.OnMessage;
            Server.SpecMsgHandler = LYBMsgDispatcher.Instance.OnSpecMessage;
            Server.Connect(ip, port.ToString());
            Debug.Log("准备连接到服务器(" + ip + ":" + port.ToString() + "). LinkId:" + Server.LinkID);
        }

        private void DoReconnect(object[] parameters)
        {
            Connect(ip, port);
        }

        private void ConnectTimeOut(object[] parameters)
        {
            isConnecting = false;
            ClearTimers();

            if (Server != null &&
                Server.IsConnected())
                return;

            Debug.LogWarning("创建连接超时.");

            if (AutoReconnect)
                Connect(ip, port);
            else
                RaiseScriptEvent("OnConnectTimeout");
        }

        internal virtual bool Send<T>(T msg) where T : LYBMsgHeader
        {
            bool ret = false;
            if (Server != null && Server.Send(msg))
                ret = true;
            //Debug.Log("发送消息T first:" + msg.firstProtol + "\tsecond:" + msg.secondProtol + "\tLinkId:" + Server.LinkID);
            MsgPool.Instance.Free(msg);
            return ret;
        }

        internal virtual bool Send(LYBSerializerIn msgBuff)
        {
            if (Server != null && Server.Send(msgBuff.Buff, msgBuff.Size))
            {
                //Debug.LogError("发送消息：0 " + msgBuff.Buff[0] + "\t1 " + msgBuff.Buff[1] + "\t2 " + msgBuff.Buff[2] + "\t3 " + msgBuff.Buff[3] + "\t4 " + msgBuff.Buff[4] + "\tLinkId:" + Server.LinkID);
                return true;
            }
            return false;
        }

        #region IServerListenner
        public virtual void OnConnected(ulong linkID, bool isOk)
        {
            if (!IsServerLink(linkID))
                return;

            isConnecting = false;
            ClearTimers();

            if (isOk)
            {
                RaiseScriptEvent("OnConnectSuccess");
            }
            else
            {
                if (AutoReconnect)
                {
                     Reconnect(-1);
                    return;
                }
                RaiseScriptEvent("OnConnectFailed");
            }
        }

        public void Reconnect(double reconnectInterval)
        {
            //TODO
            //if (connectTimeOutTimerID != 0)
            //{
            //    TimerManager.Instance.DeleteTimer(connectTimeOutTimerID);
            //    connectTimeOutTimerID = 0;
            //}

            reconnectInterval = reconnectInterval < 0 ? this.reconnectInterval : reconnectInterval;

            Debug.LogFormat("{0}毫秒后重新连接到服务器({1}:{2}).", reconnectInterval, ip, port);
            requestReconnectTimerID = TimerManager.Instance.AddTimer(reconnectInterval, DoReconnect, null);
        }

        public void OnDisconnected(ulong linkID)
        {
            if (IsServerLink(linkID))
            {
                var reason = Server.GetDisconnectReason();
                Debug.LogErrorFormat("失去连接. Reason: {0}", reason);
                if (AutoReconnect)
                {
                    //刘睿改 2016-11-5
                    RaiseScriptEvent("OnDisconnectedNormal");
                    Reconnect(-1);
                    return;
                }

                // 不可调用Close, 需保留部分登录信息
                isConnecting = false;
                ClearTimers();
                server = null;

                switch (reason)
                {
                    case DisconnectReason.Active:
                        break;
                    case DisconnectReason.Normal:
                        RaiseScriptEvent("OnDisconnectedNormal");
                        break;
                    case DisconnectReason.Timeout:
                        RaiseScriptEvent("OnDisconnectedTimeout");
                        break;
                }
            }
            else
            {
                Debug.Log("过时的连接断开:OnDisconnected:" + linkID + "\tServerBase is nil:" + (Server == null));
            }
        }

        public virtual void OnDestory(ulong linkID)
        {
            Debug.Log("连接已销毁." + linkID);
        }

        public void OnReuseSessionFailed(ulong linkID, ulong otherSessionKey) { }

        public void OnReuseSessionSuccess(ulong linkID, ulong otherSessionKey) { }

        public void OnSendRequest(NetLinkBase thisServer) { }
        #endregion

        protected void ClearTimers()
        {
            //TODO
            //TimerManager.Instance.DeleteTimer(connectTimeOutTimerID);
            TimerManager.Instance.DeleteTimer(requestReconnectTimerID);
            //connectTimeOutTimerID = 0;
            requestReconnectTimerID = 0;
        }

        public virtual void Close()
        {
            Debug.Log("LYBServerBase Close");
            isConnecting = false;
            ClearTimers();

            if (Server != null)
            {
                Debug.Log("LYBServerBase Close ServerLinkId:" + Server.LinkID);
                if (Server.IsConnected())
                {
                    Debug.Log("LYBServerBase IsConnected ServerLinkId:" + Server.LinkID);
                    Server.Disconnect();
                }
                else
                {
                    Debug.Log("LYBServerBase IsConnected false ServerLinkId:" + Server.LinkID);
                    Server.Release();
                }
            }
            server = null;
            Debug.Log("LYBServerBase Close set server to null");
        }

        internal int GetPing()
        {
            if (Server == null)
                return -1;
            return Server.GetPing();
        }

        internal int GetServerTime()
        {
            if (Server == null)
                return -1;
            return Server.GetServerTime();
        }
        public void Execute()
        {
            if (Server != null)
                Server.Execute();
        }
    }
}
