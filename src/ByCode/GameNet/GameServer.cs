using Network.DataDefs.NetMsgDef;
using Network.Net;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

/************************************************************************
 * 
 * 登录管理器
 * 1.连接维护
 * 2.登录验证
 * 3.断线重入
 * 
 * ************************************************************************/

namespace UnityDLL
{
    public class GameServer : LYBServerBase
    {
        private static GameServer instance;
        public static GameServer Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameServer();
                return instance;
            }
        }

        public uint CheckedId
        {
            get
            {
                return checkedId;
            }

            set
            {
                checkedId = value;
            }
        }

        #region 连接管理
        private uint checkedId = 0;
        private LYBStream checkedStreamData = null;
        public bool IsRebinding = false;
        public bool IsLogined = false;
        public bool IsAppShutdown = false;

        public GameServer()
            : base("GS")
        {
            //AutoReconnect = true;
        }

        public void SetSessionInfo(uint checkGid, LYBStream checkStreamData)
        {
            CheckedId = checkGid;
            checkedStreamData = checkStreamData;
        }

        public override void Close()
        {
            Debug.Log("GameServer Close");
            if (Server != null)
            {
                Debug.Log("ServerLinkId:" + Server.LinkID);
            }
            if (IsConnected())
            {
                LogOut();
                if (IsAppShutdown)
                {
                    // 因LYB底层没有发送回调, 需在此尽量确保底层消息发出, 又不影响玩家感受
                    System.Threading.Thread.Sleep(50);
                }
            }
            base.Close();
            Reset();
        }

        protected override void Reset()
        {
            IsLogined = false;
            CheckedId = 0;
            checkedStreamData = null;
            base.Reset();
        }

        public override void OnConnected(UInt64 linkID, bool isOk)
        {
            var isServerLink = IsServerLink(linkID);
            base.OnConnected(linkID, isOk);

            if (!isServerLink || !isOk)
                return;

            Debug.Log("成功连接到游戏服务器：" + linkID);
            RebindServer(CheckedId, checkedStreamData);

            //if (IsLogined == false)
            //    RebindServer(checkedId, checkedStreamData);
            //else
            //    //...
        }

        public override void OnDestory(UInt64 linkID)
        {
            base.OnDestory(linkID);
            if (IsServerLink(linkID))
                Reset();
        }

        internal override void Connect(string ip, ushort port)
        {
            Assert.IsTrue(CheckedId != 0);
            base.Connect(ip, port);
        }

        internal override bool Send<T>(T msg)
        {
            if (IsRebinding)
            {
                if (msg.firstProtol != SPMPS.EPRO_REBIND_MESSAGE || msg.secondProtol != SCMPS.EPRO_REBIND_REGION_SERVER)
                {
                    Debug.Log("SendT IsRebinding中，不能发其他消息");
                    Debug.Log("first:" + msg.firstProtol + "\tsecond:" + msg.secondProtol + "\tLinkId:" + Server.LinkID);
                    return false;
                }
            }
            //Debug.Log("GameServer SendMsg");
            return base.Send(msg);
        }

        internal override bool Send(LYBSerializerIn msgBuff)
        {
            if (IsRebinding)
            {
                if (msgBuff.Buff[0] != SPMPS.EPRO_REBIND_MESSAGE || msgBuff.Buff[1] != SCMPS.EPRO_REBIND_REGION_SERVER)
                {
                    Debug.Log("Send IsRebinding中，不能发其他消息");
                    Debug.Log("0 " + msgBuff.Buff[0] + "\t1 " + msgBuff.Buff[1] + "\t2 " + msgBuff.Buff[2] + "\t3 " + msgBuff.Buff[3] + "\t4 " + msgBuff.Buff[4] + "\tLinkId:" + Server.LinkID);
                    return false;
                }
            }
            //Debug.Log("GameServer SendMsg In");
            return base.Send(msgBuff);
        }

        #endregion

        #region 主动发送接口

        //MD5 md5Service = MD5.Create();
        public bool RebindServer(uint id, LYBStream streamData)
        {
            var msgCG = MsgPool.Instance.Get<SQRebindMsg>();

            msgCG.id_ = id;
            msgCG.stream_data_ = streamData;
            IsRebinding = Send(msgCG);
            return IsRebinding;
        }

        internal void ReLogin()
        {
            RebindServer(CheckedId, checkedStreamData);
        }

        #region 临时支持(Login)
        public string GetOsName()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.WP8Player:
                    return "WP8";
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    return "Windows";
                default:
                    return "Other";
            }
        }

        string SplitMobileOsVer(string osver)
        {
            //string osver = "iPhone OS 7.1.1";//"Android OS 4.2.2 / Api-17 (dsfasdfasdf)";
            int startIdx = osver.LastIndexOf("OS ") + 3;
            int endIdx = osver.IndexOf(' ', startIdx);
            if (endIdx == -1)
                endIdx = osver.Length;
            return osver.Substring(startIdx, endIdx - startIdx);
        }

        public string GetOsVer()
        {
            string osver = null;
            int startIdx = 0;
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    return SplitMobileOsVer(SystemInfo.operatingSystem);
                case RuntimePlatform.Android:
                    return SplitMobileOsVer(SystemInfo.operatingSystem);
                case RuntimePlatform.WP8Player:
                    return SystemInfo.operatingSystem;
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    osver = SystemInfo.operatingSystem;
                    startIdx = osver.IndexOf('(') + 1;
                    return osver.Substring(startIdx, osver.IndexOf(')') - startIdx);
                default:
                    return "Other";
            }
        }

        public string GetAdudid()
        {
            return "";
        }

        public string GetResolution()
        {
            return Screen.currentResolution.width + "*" + Screen.currentResolution.height;
        }

        #endregion
        public bool LogOut()
        {
            if (CheckedId == 0)
                return false;
            Debug.Log("LogOut");
            var msgCG = MsgPool.Instance.Get<SQLogoutMsg>();
            msgCG.id_ = CheckedId;
            msgCG.logout_type_ = SQLogoutMsg.EST_LOGOUT_NORMAL;
            return Send(msgCG);
        }

        #endregion

        #region 被动触发接口

        #endregion

        #region Move Msg

        #endregion


        internal bool RequestObjInfo(uint id)
        {
            //if (id == GameScene.LocalPlayerID)
            //    return true;
            //Debug.LogFormat("RequestObjInfo: {0}", id);
            var msgCG = MsgPool.Instance.Get<SQObjectInfoMsg>();
            msgCG.id_ = id;
            msgCG.request_type_ = SQObjectInfoMsg.GIT_ALL;
            return Send(msgCG);
        }

        // 服务器主动断开 不让重连
        internal void OnBeKicked()
        {
            RaiseScriptEvent("OnBeKicked");
        }

        private static LYBSerializerIn luaMsgSerializer = new LYBSerializerIn();
        public static LYBSerializerIn GetMsgWriteStream()
        {
            luaMsgSerializer.ResetStream();
            luaMsgSerializer.Reset();
            return luaMsgSerializer;
        }

        public static int _GetServerTime()
        {
            return Instance.GetServerTime();
        }

        public static int _GetPing()
        {
            return Instance.GetPing();
        }

        public static bool Send_(LYBSerializerIn msgBuff)
        {
            return Instance.Send(msgBuff);
        }

        public static void Logout_()
        {
            if (Instance.IsConnected())
            {
                Instance.LogOut();
            }
        }

        public static void CloseSocket()
        {
            Instance.Close();
        }

        public static bool IsConnected_()
        {
            return Instance.IsConnected();
        }
    }
}
