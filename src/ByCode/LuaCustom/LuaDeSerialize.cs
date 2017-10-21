using System;
using LuaInterface;
using System.Runtime.InteropServices;
using Network.Net;
using Network.Log;
using Network.DataDefs;

namespace UnityDLL.LuaCustom
{
    public partial class LuaSerialize
    {

        #region public methods
        // Caller is C#, C# BinData => Lua Table
        public static int PushTblFromBlob(LuaState state, TBinaryData data)
        {
            if (state != Lua.lua)
            {
                UnityEngine.Debug.LogException(
                    new Exception("lua state not correct!"));
            }

            return PushTblFromBlob(Lua.lua.GetL(), data);
        }
        public static int PushTblFromBlob(IntPtr L, TBinaryData data)
        {
            int topIdx = LuaDLL.lua_gettop(L);
            LuaSerializeHelper.Encoding = encodingGBK;
            if (!LuaSerializeHelper.LoadTable(L, data))
            {
                LuaDLL.lua_settop(L, topIdx);

                LogManager.Instance.LogDebug("lua_push_tbl_from_blob参数内容不合法.");

                LuaDLL.lua_newtable(L);
                return 0;
            }

            return 1;
        }
        #endregion

        #region private
        static System.Text.Encoding encodingGBK = System.Text.Encoding.GetEncoding(LYBGlobalConsts.ENCODE);

        // Caller is lua, BinData in lua => Lua Table
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int lua_TblFromBlob(IntPtr L)
        {
            int paramsCount = LuaDLL.lua_gettop(L);
            if (paramsCount < 1)
            {
                throw new LuaException("lua_tbl_load_from_blob_in_lua: 参数个数错误.");
            }

            TBinaryData srcBuff = null;
            var valType = LuaDLL.lua_type(L, 1);
            if (valType == LuaTypes.LUA_TSTRING)
            {
                int size = 0;
                var tblBlob = LuaDLL.lua_tolstring(L, 1, out size);
                binDataBuff.Reset();
                binDataBuff.Reserve(size);
                Marshal.Copy(tblBlob, binDataBuff.GetBuffer(), 0, size);
                binDataBuff.Write(null, 0, size);
                srcBuff = binDataBuff;
            }
            else if (valType == LuaTypes.LUA_TUSERDATA)
            {
                srcBuff = (TBinaryData)ToLua.ToObject(L, 1);
            }
            else
            {
                throw new LuaException("lua_tbl_load_from_blob_in_lua：参数错误.");
            }

            LuaSerializeHelper.Encoding = System.Text.Encoding.UTF8;
            if (paramsCount > 1)
            {
                var encodeType = LuaDLL.lua_tointeger(L, 2);
                LuaDLL.lua_pop(L, 1);
                switch (encodeType)
                {
                    case 0:
                        LuaSerializeHelper.Encoding = System.Text.Encoding.UTF8;
                        break;
                    case 1:
                        LuaSerializeHelper.Encoding = encodingGBK;
                        break;
                    case 2:
                        LuaSerializeHelper.Encoding = System.Text.Encoding.Unicode;
                        break;
                    default:
                        throw new LuaException("lua_tbl_load_from_blob_in_lua：用于指定文本编码的参数2错误.");
                }
            }
            LuaDLL.lua_pop(L, 1);

            int topIdx = LuaDLL.lua_gettop(L);
            if (!LuaSerializeHelper.LoadTable(L, srcBuff))
            {
                LuaDLL.lua_settop(L, topIdx);
                throw new LuaException("lua_push_tbl_from_blob参数内容不合法.");
            }

            return 1;
        }
        #endregion
    }
}
