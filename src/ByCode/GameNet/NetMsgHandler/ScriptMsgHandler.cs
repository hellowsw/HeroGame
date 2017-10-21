using Network.Net;
using Network.DataDefs.NetMsgDef;
using LuaInterface;
using System;
using UnityEngine;
using UnityDLL.LuaCustom;

namespace UnityDLL.NetMsgHandler
{
    class ScriptMsgHandler
    {
        public static void Init()
        {
            LYBMsgDispatcher.Instance.RegMsgHandler(SALuaCustomMsg.opCode, OnRpcCallLua);
            LYBMsgDispatcher.Instance.RegMsgHandler(SAScriptProcessactionMsg.opCode, OnProcessActionLua);
            LYBMsgDispatcher.Instance.RegMsgHandler(SSyncTaskDataMsg.opCode, OnSyncTaskData);
            LYBMsgDispatcher.Instance.RegMsgHandler(SAItemMsg.opCode, OnItemUpdate);
          //  LYBMsgDispatcher.Instance.RegMsgHandler(SAClickObjectMsg.opCode, OnClickObjectRsp);
            //LYBMsgDispatcher.Instance.RegMsgHandler(SABackMsg.opCode, OnBackMsg);
        }

        private static void OnItemUpdate(UInt64 linkID, TMemoryBufferEx packetBuff)
        {
            SAItemMsg msg = null;
            if (!LYBMsgDispatcher.GetMsg(packetBuff, ref msg, false))
                return;

            LuaTable table = Lua.lua.GetTable("NetInterface");
            Lua.Call(table, "OnGoldChange", msg.MoneyType, msg.Money);
        }

        private static void OnRpcCallLua(UInt64 linkID, TMemoryBufferEx packetBuff)
        {
            SALuaCustomMsg msg = null;
            if (!LYBMsgDispatcher.GetMsg(packetBuff, ref msg, false))
                return;
            LuaRpc.OnLuaRpcCall(
                msg.flags2_, msg.flags3_, msg.lua_bin_data_);
        }

        private static void OnProcessActionLua(UInt64 linkID, TMemoryBufferEx packetBuff)
        {
            SAScriptProcessactionMsg msg = null;
            if (!LYBMsgDispatcher.GetMsg(packetBuff, ref msg))
                return;
            Debug.Log(msg.action_);
        }

        private static void OnSyncTaskData(UInt64 linkID, TMemoryBufferEx packetBuff)
        {
            SSyncTaskDataMsg msg = null;
            if (!LYBMsgDispatcher.GetMsg(packetBuff, ref msg, false))
                return;

            var ls = Lua.lua.GetL();
            LuaDLL.lua_getglobal(ls, "OnSyncTaskData");
            if (!LuaDLL.lua_isfunction(ls, -1))
            {
                throw new LuaException("函数OnOnSyncTaskData不存在");
            }
            LuaSerialize.PushTblFromBlob(ls, msg.lua_bin_data_);
            if (LuaDLL.lua_pcall(ls, 1, 0, 0) != 0)
            {
                string error = LuaDLL.lua_tostring(ls, -1);
                throw new LuaException(error);
            }
        }

        private static void OnBackMsg(UInt64 linkID, TMemoryBufferEx packetBuff)
        {
            SABackMsg msg = null;
            Debug.Log("MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM");
            if (!LYBMsgDispatcher.GetMsg(packetBuff, ref msg))
                return;
           
        }
        
    }
}
