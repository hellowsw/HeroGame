using Network.Net;
using Network.DataDefs.NetMsgDef;
using System;
using UnityEngine;
using LuaInterface;
using UnityDLL.LuaCustom;
using GameCommon;
using GameNet;
using GameLogic;
using GameLogic.Common;

namespace UnityDLL.NetMsgHandler
{
    static class LoginMsgHandler
    {
        public static void Init()
        {
            LYBMsgDispatcher.Instance.RegMsgHandler(SALoginMsg.opCode, OnLoginRsp);
            LYBMsgDispatcher.Instance.RegMsgHandler(SAChListMsg.opCode, OnCharactorListRsp);
            LYBMsgDispatcher.Instance.RegMsgHandler(SACreatePlayerMsg.opCode, OnCreatePlayerRsp);
            LYBMsgDispatcher.Instance.RegMsgHandler(SADeletePlayerMsg.opCode, OnDeletePlayerRsp);
            LYBMsgDispatcher.Instance.RegMsgHandler(SASelPlayerMsg.opCode, OnSelPlayerRsp);
            LYBMsgDispatcher.Instance.RegMsgHandler(SARebindMsg.opCode, OnRebindServerRsp);
            LYBMsgDispatcher.Instance.RegMsgHandler(SACheckRebindMsg.opCode, OnCheckRebindRsp);

            LYBMsgDispatcher.Instance.RegMsgHandler(SAPreventIndulge.opCode, OnPreventIndulge);
        }

        private static void OnLoginRsp(UInt64 linkID, TMemoryBufferEx packetBuff)
        {
            SALoginMsg msg = null;
            if (!LYBMsgDispatcher.GetMsg(packetBuff, ref msg))
                return;

            var result = (Ret_Login)msg.result_;

            //Debug.Log("SALoginMsg:" + msg.result_);
            LuaTable table = Lua.lua.GetTable("NetInterface");
            switch (result)
            {
                case Ret_Login.ERC_LOGIN_SUCCESS:
                    Debug.Log("账号验证成功, LoginKey:" + msg.login_key_);
                    Lua.Call(table, "OnLoginSuccess");
                    break;
                default:
                    Debug.LogError("登录失败:" + result);
                    AccountServer.Instance.Close();
                    Lua.Call(table, "OnLoginFaild", (double)result);
                    return;
            }

            Debug.Log("帐号登录成功");
        }

        private static void OnCharactorListRsp(UInt64 linkID, TMemoryBufferEx packetBuff)
        {
            SAChListMsg msg = null;
            if (!LYBMsgDispatcher.GetMsg(packetBuff, ref msg))
                return;

            // 琅琊榜：有角色列表下发时表明需创建角色
            // 相关UI处理（暂无， 此处立即创建角色）
            //var func = Lua.GetFunction("OnRequireCreatePlayer");
            //func.Call();
            //刘睿改   2016-9-22
            //var func = Lua.GetFunction("OnRequireCreatePlayer");
            //func.Call();
            AccountServer.Instance.CreatePlayer(1, 1, null);
        }

        private static void OnCreatePlayerRsp(UInt64 linkID, TMemoryBufferEx packetBuff)
        {
            SACreatePlayerMsg msg = null;
            if (!LYBMsgDispatcher.GetMsg(packetBuff, ref msg))
                return;
            // 角色创建结果通知到脚本
            //刘睿 2016-11-4改
            var msgCG = MsgPool.Instance.Get<SQSelPlayerMsg>();
            msgCG.selected_idx_ = msg.idx_;
            AccountServer.Instance.Send(msgCG);
        }
        /// <summary>
        /// //待删
        /// </summary>
        public class SQSelPlayerMsg : LYBMsgHeader
        {
            public static ushort opCode = MakeOpCode(SPMPS.EPRO_SYSTEM_MESSAGE, SCMPS.EPRO_SELECT_CHARACTER);
            public SQSelPlayerMsg() : base(opCode) { }

            public byte selected_idx_;   // selIdx           
            public override void Serialize(ILYBSerializerBase ar)
            {
                base.Serialize(ar);
                ar.Handle(ref selected_idx_);
            }
        }

        private static void OnDeletePlayerRsp(UInt64 linkID, TMemoryBufferEx packetBuff)
        {
            SADeletePlayerMsg msg = null;
            if (!LYBMsgDispatcher.GetMsg(packetBuff, ref msg))
                return;
            switch ((Ret_DeletePlayer)msg.result_)
            {
                case Ret_DeletePlayer.ERC_SUCCESS:
                    Debug.LogFormat("角色{0}已成功删除.", msg.id_);
                    break;

                case Ret_DeletePlayer.ERC_UNHANDLE:
                    //MessageBox.Show(g_Language.getContent("SADeletePlayer_1"));
                    Debug.LogError("删除角色未知错误");
                    break;
            }
        }

        private static void OnSelPlayerRsp(UInt64 linkID, TMemoryBufferEx packetBuff)
        {
            SASelPlayerMsg msg = null;
            if (!LYBMsgDispatcher.GetMsg(packetBuff, ref msg))
                return;

            switch ((Ret_SelPlayer)msg.result_)
            {
                case Ret_SelPlayer.ERC_SELPLAYER_SUCCESS:
                    return;

                case Ret_SelPlayer.ERC_GETCHDATA_TIMEOUT:
                    //MessageBox.Show(g_Language.getContent("SASelPlayerMsg_1"));
                    break;
                case Ret_SelPlayer.ERC_SEND_GETCHDATA_FAIL:
                    //MessageBox.Show(g_Language.getContent("SASelPlayerMsg_2"));
                    break;
                case Ret_SelPlayer.ERC_GETCHDATA_FAIL:
                    //MessageBox.Show(g_Language.getContent("SASelPlayerMsg_3"));
                    break;
                case Ret_SelPlayer.ERC_PUTTOREGION_FAIL:
                    //MessageBox.Show(g_Language.getContent("SASelPlayerMsg_4"));
                    break;
                case Ret_SelPlayer.ERC_PLAYER_FULL:
                    //MessageBox.Show(g_Language.getContent("SASelPlayerMsg_5"));
                    break;
            }
        }

        static uint port;
        static uint checkId;
        static LYBStream data;

        private static void OnRebindServerRsp(UInt64 linkID, TMemoryBufferEx packetBuff)
        {
            Debug.Log("++++++++++++++++++OnRebindServerRsp");
            SARebindMsg msg = null;
            if (!LYBMsgDispatcher.GetMsg(packetBuff, ref msg))
                return;
            Debug.LogFormat("SARebindMsg  IP:{0}   Port:{1}   zID:{2}   isLogin:{3}", msg.ip_, msg.port_, msg.zID, msg.isLogin);
            AccountServer.Instance.Close();
            System.Net.IPAddress ip = new System.Net.IPAddress(msg.ip_);
            GameServer.Instance.CheckedId = msg.id_;

            if (msg.isLogin == 2)
            {
                port = msg.port_;
                checkId = msg.id_;
                data = msg.stream_data_;
                LuaTable table = Lua.lua.GetTable("NetInterface");
                Lua.Call(table, "OnSARebindMsg");
            }
            else if (msg.isLogin == 1)
            {
                GameServer.Instance.Close();
                GameServer.Instance.SetSessionInfo(msg.id_, msg.stream_data_);
                GameServer.Instance.Connect(LoginConfig.IP, (ushort)msg.port_);
                GameServer.Instance.IsRebinding = true;
            }
        }

        private static void OnCheckRebindRsp(UInt64 linkID, TMemoryBufferEx packetBuff)
        {
            SACheckRebindMsg msg = null;
            if (!LYBMsgDispatcher.GetMsg(packetBuff, ref msg))
                return;
            Debug.LogFormat("SACheckRebindMsg: {0},port:{1}", msg.result_, port);

            //1 表示主动断开你 不让重练
            //2 表示可以重定向成功 可以向游戏服务器发消息了
            switch (msg.result_)
            {
                case 2:
                    GameServer.Instance.IsLogined = true;
                    GameServer.Instance.IsRebinding = false;
                    break;
                case 1:
                    Debug.Log("服务器不允许登录:" + port);
                    if (port != 0)
                    {
                        GameServer.Instance.Close();
                        GameServer.Instance.SetSessionInfo(checkId, data);
                        GameServer.Instance.Connect(LoginConfig.IP, (ushort)port);
                        GameServer.Instance.IsRebinding = true;
                        port = 0;
                        checkId = 0;
                        data = null;
                        Debug.Log("重连");
                    }
                    else
                    {
                        GameServer.Instance.Close();
                        GameServer.Instance.OnBeKicked();
                        Debug.Log("离线");
                    }
                    break;
                default:
                    break;
            }
        }

        private static void OnPreventIndulge(UInt64 linkID, TMemoryBufferEx packetBuff)
        {
            SAPreventIndulge msg = null;
            if (!LYBMsgDispatcher.GetMsg(packetBuff, ref msg))
                return;

            // 琅琊榜：防沉迷
        }
    }
}
