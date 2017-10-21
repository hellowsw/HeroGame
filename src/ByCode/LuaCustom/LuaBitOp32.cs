using LuaInterface;
using System;

namespace UnityDLL.LuaCustom
{
    public class LuaBitOp32
    {
        #region public methods
        public static void Init(LuaState state)
        {
            if (state != Lua.lua)
            {
                UnityEngine.Debug.LogException(
                    new Exception("lua state not correct!"));
            }

            luaopen_LuaBitOp32(Lua.lua.GetL());
        }
        #endregion

        #region private

        static string rpcModuleName = "BitOp32";

        //static int LUA_REGISTRYINDEX = -10000;
        //static int LUA_ENVIRONINDEX = -10001;
        static int LUA_GLOBALSINDEX = -10002;
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int lua_BitAnd(IntPtr L)
        {
            int params_count = LuaDLL.lua_gettop(L);
            if (params_count !=  2)
            {
                throw new LuaException("lua_BitAnd:参数数量错误");
            }

            if (LuaDLL.lua_isnumber(L, 1) == 0 ||
                LuaDLL.lua_isnumber(L, 2) == 0)
            {
                throw new LuaException("lua_BitAnd:参数类型错误");
            }

            var lval = (int)LuaDLL.lua_tonumber(L, 1);
            var rval = (int)LuaDLL.lua_tonumber(L, 2);
            LuaDLL.lua_pushinteger(L, lval & rval);
            return 1;
        }
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int lua_BitOr(IntPtr L)
        {
            int params_count = LuaDLL.lua_gettop(L);
            if (params_count != 2)
            {
                throw new LuaException("lua_BitOr:参数数量错误");
            }

            if (LuaDLL.lua_isnumber(L, 1) == 0 ||
                LuaDLL.lua_isnumber(L, 2) == 0)
            {
                throw new LuaException("lua_BitOr:参数类型错误");
            }

            var lval = (int)LuaDLL.lua_tonumber(L, 1);
            var rval = (int)LuaDLL.lua_tonumber(L, 2);
            LuaDLL.lua_pushinteger(L, lval | rval);
            return 1;
        }
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int lua_BitNot(IntPtr L)
        {
            int params_count = LuaDLL.lua_gettop(L);
            if (params_count != 1)
            {
                throw new LuaException("lua_BitNot:参数数量错误");
            }

            if (LuaDLL.lua_isnumber(L, 1) == 0)
            {
                throw new LuaException("lua_BitNot:参数类型错误");
            }

            var lval = (int)LuaDLL.lua_tonumber(L, 1);
            LuaDLL.lua_pushinteger(L, ~lval);
            return 1;
        }
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int lua_BitLShift(IntPtr L)
        {
            int params_count = LuaDLL.lua_gettop(L);
            if (params_count != 2)
            {
                throw new LuaException("lua_BitOr:参数数量错误");
            }

            if (LuaDLL.lua_isnumber(L, 1) == 0 ||
                LuaDLL.lua_isnumber(L, 2) == 0)
            {
                throw new LuaException("lua_BitOr:参数类型错误");
            }

            var lval = (int)LuaDLL.lua_tonumber(L, 1);
            var rval = (int)LuaDLL.lua_tonumber(L, 2);
            LuaDLL.lua_pushinteger(L, lval << rval);
            return 1;
        }
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int lua_BitRShift(IntPtr L)
        {
            int params_count = LuaDLL.lua_gettop(L);
            if (params_count != 2)
            {
                throw new LuaException("lua_BitOr:参数数量错误");
            }

            if (LuaDLL.lua_isnumber(L, 1) == 0 ||
                LuaDLL.lua_isnumber(L, 2) == 0)
            {
                throw new LuaException("lua_BitOr:参数类型错误");
            }

            var lval = (int)LuaDLL.lua_tonumber(L, 1);
            var rval = (int)LuaDLL.lua_tonumber(L, 2);
            LuaDLL.lua_pushinteger(L, lval >> rval);
            return 1;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int lua_BitGet(IntPtr L)
        {
            int params_count = LuaDLL.lua_gettop(L);
            if (params_count != 2)
            {
                throw new LuaException("lua_BitGet:参数数量错误");
            }

            if (LuaDLL.lua_isnumber(L, 1) == 0 ||
                LuaDLL.lua_isnumber(L, 2) == 0)
            {
                throw new LuaException("lua_BitGet:参数类型错误");
            }

            var lval = (int)LuaDLL.lua_tonumber(L, 1);
            var rval = (int)LuaDLL.lua_tonumber(L, 2);
            LuaDLL.lua_pushinteger(L, (lval & (1 << rval)) !=  0 ? 1 : 0);
            return 1;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int lua_BitSet(IntPtr L)
        {
            int params_count = LuaDLL.lua_gettop(L);
            if (params_count != 2)
            {
                throw new LuaException("lua_BitSet:参数数量错误");
            }

            if (LuaDLL.lua_isnumber(L, 1) == 0 ||
                LuaDLL.lua_isnumber(L, 2) == 0)
            {
                throw new LuaException("lua_BitSet:参数类型错误");
            }

            var lval = (int)LuaDLL.lua_tonumber(L, 1);
            var rval = (int)LuaDLL.lua_tonumber(L, 2);
            LuaDLL.lua_pushinteger(L, lval | (1 << rval));
            return 1;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int lua_BitClear(IntPtr L)
        {
            int params_count = LuaDLL.lua_gettop(L);
            if (params_count != 2)
            {
                throw new LuaException("lua_BitClear:参数数量错误");
            }

            if (LuaDLL.lua_isnumber(L, 1) == 0 ||
                LuaDLL.lua_isnumber(L, 2) == 0)
            {
                throw new LuaException("lua_BitClear:参数类型错误");
            }

            var lval = (int)LuaDLL.lua_tonumber(L, 1);
            var rval = (int)LuaDLL.lua_tonumber(L, 2);
            LuaDLL.lua_pushinteger(L, lval & ~(1 << rval));
            return 1;
        }

        static int luaopen_LuaBitOp32(IntPtr L)
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

            LuaDLL.lua_pushstring(L, "And");
            LuaDLL.lua_pushcfunction(L, lua_BitAnd);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pushstring(L, "Or");
            LuaDLL.lua_pushcfunction(L, lua_BitOr);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pushstring(L, "Not");
            LuaDLL.lua_pushcfunction(L, lua_BitNot);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pushstring(L, "LShift");
            LuaDLL.lua_pushcfunction(L, lua_BitLShift);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pushstring(L, "RShift");
            LuaDLL.lua_pushcfunction(L, lua_BitRShift);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pushstring(L, "Set");
            LuaDLL.lua_pushcfunction(L, lua_BitSet);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pushstring(L, "Get");
            LuaDLL.lua_pushcfunction(L, lua_BitGet);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pushstring(L, "Clear");
            LuaDLL.lua_pushcfunction(L, lua_BitClear);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pop(L, 1);
            return 1;
        }
        #endregion
    }
}
