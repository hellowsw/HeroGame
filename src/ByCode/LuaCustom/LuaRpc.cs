using System;
using LuaInterface;
using Network.DataDefs.NetMsgDef;
using Network.Net;

namespace UnityDLL.LuaCustom
{
    // 脚本RPC交互处理
    // 除脚本RPC处理函数外, 禁止直接使用GetL

    public class LuaRpc
    {
        #region public methods
        public static void Init(LuaState state)
        {
            if (state != Lua.lua)
            {
                UnityEngine.Debug.LogException(
                    new Exception("lua state not correct!"));
            }

            luaopen_LuaRpc(Lua.lua.GetL());
        }
        // Server => Client

        public static void OnLuaRpcCall(byte unused1, byte unused2, TBinaryData luaBinData)
        {
            var ls = Lua.lua.GetL();
            LuaDLL.lua_getglobal(ls, "OnLuaRpcCall");
            if (!LuaDLL.lua_isfunction(ls, -1))
            {
                throw new LuaException("函数OnLuaRpcCall不存在");
            }
            LuaDLL.lua_pushinteger(ls, unused1);
            LuaDLL.lua_pushinteger(ls, unused2);
            LuaSerialize.PushTblFromBlob(ls, luaBinData);
            if (LuaDLL.lua_pcall(ls, 3, 0, 0) != 0)
            {
                string error = LuaDLL.lua_tostring(ls, -1);
                throw new LuaException(error);
            }
        }
        #endregion

        #region private

        static string rpcModuleName = "RpcProxy";

        //static int LUA_REGISTRYINDEX = -10000;
        //static int LUA_ENVIRONINDEX = -10001;
        static int LUA_GLOBALSINDEX = -10002;

        // Client => Server
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int lua_SendLuaRpcCall(IntPtr L)
        {
            int params_count = LuaDLL.lua_gettop(L);
            if (params_count < 3 || params_count > 4)
            {
                throw new LuaException("SendLuaRpcCall:参数数量错误");
            }

            if (LuaDLL.lua_isnumber(L, 1) == 0 ||
                LuaDLL.lua_isnumber(L, 2) == 0 ||
                LuaDLL.lua_istable(L, 3) == false)
            {
                throw new LuaException("SendLuaRpcCall:参数类型错误");
            }

            var msgCG = MsgPool.Instance.Get<SQLuaCustomMsg>();

            msgCG.first_id_ = (byte)LuaDLL.lua_tointeger(L, 1);
            msgCG.second_id_ = (byte)LuaDLL.lua_tointeger(L, 2);
            int filterIdx = 0;
            if (params_count == 4)
            {
                filterIdx = -1; // 过滤器在栈顶
            }

            msgCG.lua_bin_data_ = LuaSerialize.TblSaveToBlob(L, filterIdx);
            GameServer.Instance.Send(msgCG);
            return 1;
        }
        static int luaopen_LuaRpc(IntPtr L)
        {
            LuaDLL.lua_getglobal(L, rpcModuleName);
            if (!LuaDLL.lua_istable(L, -1))
            {
                LuaDLL.lua_pop(L, 1);
                LuaDLL.lua_pushstring(L, rpcModuleName);
                LuaDLL.lua_newtable(L);
                LuaDLL.lua_rawset(L, LUA_GLOBALSINDEX);

                LuaDLL.lua_getglobal(L, rpcModuleName);
            }

            LuaDLL.lua_pushstring(L, "Call");
            LuaDLL.lua_pushcfunction(L, lua_SendLuaRpcCall);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pop(L, 1);
            return 1;
        }
        #endregion
    }
}
