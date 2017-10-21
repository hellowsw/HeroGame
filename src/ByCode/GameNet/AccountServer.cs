using Network.Net;
using Network.Timer;
using System;
using UnityEngine;
using Network.DataDefs;
using Network.DataDefs.NetMsgDef;
using GameCommon;
using GameNet;

namespace UnityDLL
{
    public class AccountServer : LYBServerBase
    {
        private static AccountServer instance;
        public static AccountServer Instance
        {
            get
            {
                if (instance == null)
                    instance = new AccountServer();
                return instance;
            }
        }
        
        public AccountServer()
            : base("AS")
        {
            //AutoReconnect = true;
        }

        public void Connect(OnConnectedHandler connectedHandler)
        {
            AfterConnected = connectedHandler;
            base.Connect(ip, port);
        }

        public void Connect(string ip, ushort port, OnConnectedHandler connectedHandler)
        {
            AfterConnected = connectedHandler;
            base.Connect(ip, port);
        }

        public override void OnConnected(UInt64 linkID, bool isOk)
        {
            var isServerLink = IsServerLink(linkID);
            base.OnConnected(linkID, isOk);

            if (!isServerLink || !isOk)
                return;

            Debug.Log("已连接到账号服务器.");

            if (AfterConnected != null)
                AfterConnected();
        }

        public delegate void OnConnectedHandler();
        OnConnectedHandler AfterConnected;
        public uint serverID = 0;
        public int channelID = 0;

        private string accountName = null;

        public void SetServerInfo(string serverIDStr, string ip, ushort port)
        {
            serverID = Convert.ToUInt32(serverIDStr);
            this.ip = ip;
            this.port = port;
        }
        public bool CheckLogin(string channel_str, string account_name, string passwd)
        {
            accountName = account_name;
            var msgCA = MsgPool.Instance.Get<SQLoginMsg>();
            msgCA.account_ = account_name;
            msgCA.passwd_ = passwd;
            msgCA.server_id_ = serverID;
            msgCA.device_uid_ = 0;    //设备唯一标识在手机上需作扩展
            msgCA.login_type = GameConst.Cur_Platform_Flag;

            //// 跨服登录用
            //msgCA.crossServerId = 0;
            //msgCA.crossMid = 0;

            return Send(msgCA);
        }

        public bool CreatePlayer(int job, int sex, string name)
        {
            if (name == null)
                name = accountName;
            var msgCA = MsgPool.Instance.Get<SQCreatePlayerMsg>();
            if (msgCA.player_data_ == null)
                msgCA.player_data_ = new CreateFixProperty();

            CreateFixProperty newPlayer = msgCA.player_data_;
            newPlayer.name_ = name;
            newPlayer.prefix_ = (byte)(job << 5 | sex);

            msgCA.idx_ = 1;

            return Send(msgCA);
        }

        public bool RegisterCDKey(string account_name, string cdkey)
        {
            return true;
        }

        public void StartLogin(string userName, string password)
        {
            Connect(ip, port, ()=> {
                CheckLogin(null, userName, password);
            });
        }

        public static void ResetTryCount()
        {
            Instance.connectTryCount = MaxReconnectTimes;
        }
    }
}
