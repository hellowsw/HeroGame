using Network.Log;
using Network.Timer;
using LuaInterface;
using System;

namespace UnityDLL.LuaCustom
{
    public static class LuaTimer
    {
        #region public methods
        public static void Init(LuaState state)
        {
            if (state != Lua.lua)
            {
                UnityEngine.Debug.LogException(
                    new Exception("lua state not correct!"));
            }

            luaopen_LuaTimer(Lua.lua.GetL());
        }
        #endregion

        #region private
        static string rpcModuleName = "TimerProxy";
        static int LUA_GLOBALSINDEX = -10002;
        static int luaopen_LuaTimer(IntPtr L)
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

            LuaDLL.lua_pushstring(L, "AddTimer");
            LuaDLL.lua_pushcfunction(L, lua_AddTimer);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pushstring(L, "RemoveTimer");
            LuaDLL.lua_pushcfunction(L, lua_RemoveTimer);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pop(L, 1);
            return 1;
        }
        static int lua_AddTimer(IntPtr L)
        {
            int params_count = LuaDLL.lua_gettop(L);
            if (params_count != 2)
            {
                throw new LuaException("lua_AddTimer:参数数量错误");
            }

            if (LuaDLL.lua_isnumber(L, 1) == 0 ||
                LuaDLL.lua_isboolean(L, 2) == false)
            {
                throw new LuaException("lua_AddTimer:参数类型错误");
            }

            var delayMs = LuaDLL.lua_tonumber(L, 1);
            var loop = LuaDLL.lua_toboolean(L, 2);
            IntPtr nullIdPtr = (IntPtr)0;

            Timer timer = null;
            var parameters = TimerParamPool.Instance.Get(1);
            if (loop)
                timer = TimerManager.Instance.CreateLoopTimer(delayMs, OnTimerOver, parameters);
            else
                timer = TimerManager.Instance.CreateTimer(delayMs, OnTimerOver, parameters);

            TimerManager.Instance.AddTimer(timer);
            timer.parameters[0] = timer.GetIDPtr();
            LuaDLL.lua_pushlightuserdata(L, timer.GetIDPtr());
            return 1;
        }
        static int lua_RemoveTimer(IntPtr L)
        {
            int params_count = LuaDLL.lua_gettop(L);
            if (params_count != 1)
            {
                throw new LuaException("lua_RemoveTimer:参数数量错误");
            }

            if (LuaDLL.lua_islightuserdata(L, 1) == false)
            {
                throw new LuaException("lua_RemoveTimer:参数类型错误");
            }
            IntPtr id = LuaDLL.lua_touserdata(L, 1);
            ulong timer_id = Timer.IDFromPtr(id);
            if(TimerManager.Instance.DeleteTimer(timer_id) == false)
            {
                LogManager.Instance.LogError("DeleteTimer Error : timer:0x" + id.ToString("X").ToLower());
            }
            return 0;
        }

        static LuaFunction luaFuncOnTimer = null;
        static void OnTimerOver(params object[] parameters)
        {
            if (luaFuncOnTimer == null)
            {
                luaFuncOnTimer = Lua.GetFunction("__CSTimer_OnTimer");
            }
            IntPtr idPtr = (IntPtr)parameters[0];

            luaFuncOnTimer.BeginPCall();
            luaFuncOnTimer.Push(idPtr);
            luaFuncOnTimer.PCall();
            luaFuncOnTimer.EndPCall();
        }
        #endregion
    }
}
