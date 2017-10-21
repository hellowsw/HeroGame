using System;
using LuaInterface;
using Network.Net;

namespace UnityDLL.LuaCustom
{
    public partial class LuaSerialize
    {
        #region public methods
        public static void Init(LuaState state)
        {
            if (state != Lua.lua)
            {
                UnityEngine.Debug.LogException(
                    new Exception("lua state not correct!"));
            }

            luaopen_TblSerialize(Lua.lua.GetL());
        }
        // Use in C#
        public static TBinaryData TblSaveToBlob(IntPtr L, int filterIdx)
        {
            string filter = null;
            binDataBuff.Reset();

            LuaTypes valType = LuaTypes.LUA_TNONE;
            if (filterIdx == -1)
            {
                valType = LuaDLL.lua_type(L, -2);
            }
            else
            {
                valType = LuaDLL.lua_type(L, -1);
            }
            if (valType != LuaTypes.LUA_TTABLE)
            {
                throw new LuaException("TblSaveToBlob.参数不正确");
            }
            if (filterIdx == -1)
            {
                filter = LuaDLL.lua_tostring(L, -1);
                LuaDLL.lua_pop(L, 1);
            }

            if (!LuaSerializeHelper.SaveTable(L, binDataBuff, filter))
            {
                throw new LuaException("TblSaveToBlob.参数不正确, table中含有userdata");
            }
            LuaDLL.lua_pop(L, 1);

            return binDataBuff;
        }
        #endregion

        #region private
        static TBinaryData tempBuffer = new TBinaryData();
        static TBinaryData binDataBuff = new TBinaryData();
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int lua_TblSaveToBlob(IntPtr L)
        {
            binDataBuff.Reset();

            int paramsCount = LuaDLL.lua_gettop(L);
            if (paramsCount < 1 || paramsCount > 3)
            {
                LuaDLL.tolua_error(L, "lua_TblSaveToBlob: 参数个数错误.");
                return 0;
            }
            var valType = LuaDLL.lua_type(L, 1);
            if (valType != LuaTypes.LUA_TTABLE)
            {
                LuaDLL.tolua_error(L, "lua_TblSaveToBlob: 参数不正确.");
                return 0;
            }

            bool newBlob = false;
            string filter = null;
            if (paramsCount == 2)
            {
                newBlob = LuaDLL.lua_toboolean(L, -1);
                LuaDLL.lua_pop(L, 1);
            }
            if (paramsCount == 3)
            {
                filter = LuaDLL.lua_tostring(L, -1);
                LuaDLL.lua_pop(L, 1);
            }

            var destBuff = binDataBuff;
            if (newBlob)
                destBuff = new TBinaryData();

            if (!LuaSerializeHelper.SaveTable(L, destBuff, filter))
            {
                LuaDLL.tolua_error(L, "lua_TblSaveToBlob: 参数不正确，table中含有userdata");
                return 0;
            }
            LuaDLL.lua_pop(L, 1);

            ToLua.PushObject(L, destBuff);

            return 1;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int lua_TblSaveToBlobTest(IntPtr L)
        {
            var retCount = lua_TblSaveToBlob(L);

            PushTblFromBlob(L, binDataBuff);
            retCount++;

            return retCount;
        }

        static int luaopen_TblSerialize(IntPtr L)
        {
            LuaDLL.lua_getglobal(L, "table");

            LuaDLL.lua_pushstring(L, "toblob");
            LuaDLL.lua_pushcfunction(L, lua_TblSaveToBlob);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pushstring(L, "loadblob");
            LuaDLL.lua_pushcfunction(L, lua_TblFromBlob);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pushstring(L, "toblobtest");
            LuaDLL.lua_pushcfunction(L, lua_TblSaveToBlobTest);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pop(L, 1);
            return 1;
        }
        #endregion
    }
}
