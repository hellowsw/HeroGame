/*
 * Lyb中的Lua序列化处理
 */
using Network.Net;
using LuaInterface;
using System;
using System.Runtime.InteropServices;
using System.Text;
using Network.DataDefs;

namespace UnityDLL.LuaCustom
{
    public enum LuakvType
    {
        NULL = -1,
        String = 0,
        Integer,
        Number,
        Boolean,
        Array,
        Table
    }

    public struct ValInfo
    {
        public int type;
        public int lenType;

        public int iVal;
        public double dbVal;
        public byte[] strBytes;
        public TBinaryData tempData;

        public int tblIdx;
        public string tblFilter;
    }

    public static class LuaSerializeHelper
    {
        #region 序列化
        const byte tblEnd = 0x4A;

        static void Write(IntPtr L, TBinaryData buff, ValInfo info)
        {
            switch((LuakvType)info.type)
            {
                case LuakvType.String:
                    {
                        var len = info.strBytes.Length;
                        buff.Write(info.strBytes, 0, len);
                        if (len == 0 || info.lenType == 3)
                            buff.WriteByte(0); // 为不定长的字符串以及空字符串写入结束符!
                        break;
                    }
                case LuakvType.Integer:
                    buff.WriteBytes(info.iVal, info.lenType + 1);
                    break;
                case LuakvType.Number:
                    buff.WriteDouble(info.dbVal);
                    break;
                case LuakvType.Boolean:
                    break;
                case LuakvType.Array:
                    {
                        var len = info.tempData.Size;
                        buff.WriteBytes(len, info.lenType + 1);
                        buff.Write(info.tempData.Buff, 0, len);
                        break;
                    }
                case LuakvType.Table:
                    if (SaveTable(L, buff, info.tblFilter) == false)
                        throw new Exception("Lua序列化, 序列化表失败.");
                    buff.WriteByte(tblEnd);
                    break;
                default:
                    throw new Exception("Lua序列化, 未支持的类型");
            }
        }

        static void Write(IntPtr L, TBinaryData buff, ValInfo keyInfo, ValInfo valInfo)
        {
            // value_len 2位
            // key_len 2位
            // val_type 3位
            // key_type 1位
            var resultByte = valInfo.lenType << 6;
            resultByte |= keyInfo.lenType << 4;
            resultByte |= valInfo.type << 1;
            resultByte |= keyInfo.type;
            buff.WriteByte((byte)resultByte);

            Write(L, buff, keyInfo);
            Write(L, buff, valInfo);
        }
        
        static ValInfo GetInfo(string val)
        {
            ValInfo info = default(ValInfo);
            info.type = (int)LuakvType.String;

            var buff = Encoding.GetBytes(val);
            
            info.strBytes = buff;

            var lenType = buff.Length;
            
            if (lenType >= 4)
                lenType = 3;
            else
                lenType--;
            info.lenType = lenType;
            return info;
        }

        static ValInfo GetInfo(int val)
        {
            ValInfo info = default(ValInfo);
            info.type = (int)LuakvType.Integer;

            var lenType = 3;
            if ((val & 0xffffff00) == 0) lenType = 0;
            else if ((val & 0xffff0000) == 0) lenType = 1;
            else if ((val & 0xff000000) == 0) lenType = 2;
            info.iVal = val;
            info.lenType = lenType;

            return info;
        }

        static ValInfo GetInfo(double val)
        {
            ValInfo info = default(ValInfo);
            info.type = (int)LuakvType.Number;
            info.dbVal = val;
            info.lenType = 3;
            return info;
        }

        static ValInfo GetInfo(bool val)
        {
            ValInfo info = default(ValInfo);
            info.type = (int)LuakvType.Boolean;
            info.lenType = val ? 1 : 0;
            return info;
        }

        static ValInfo GetInfo(TBinaryData binaryData)
        {
            ValInfo info = GetInfo(binaryData.Size);
            info.type = (int)LuakvType.Array;
            info.tempData = binaryData;
            return info;
        }

        static ValInfo GetInfo(int tblIdx, string tblFilter)
        {
            ValInfo info = default(ValInfo);
            info.type = (int)LuakvType.Table;
            info.tblIdx = tblIdx;
            info.tblFilter = tblFilter;
            return info;
        }

        // 仅允许整数键值与字串键值
        static bool GetKeyInfo(ref ValInfo info, IntPtr L, TBinaryData binBuff, int valIdx, string filter)
        {
            var keyType = LuaDLL.lua_type(L, valIdx);
            switch (keyType)
            {
                case LuaTypes.LUA_TSTRING:
                    var strKey = LuaDLL.lua_tostring(L, valIdx);
                    if (filter != null && strKey == filter)
                        return false;
                    info = GetInfo(strKey);
                    break;
                case LuaTypes.LUA_TNUMBER:
                    double dbKey = LuaDLL.lua_tonumber(L, valIdx);
                    if (dbKey == (int)dbKey)
                        info = GetInfo((int)dbKey);
                    else
                        info = GetInfo(dbKey);
                    break;
                default:
                    return false;
            }
            return true;
        }

        static bool GetValInfo(ref ValInfo info, IntPtr L, TBinaryData binBuff, int valIdx, string filter)
        {
            var valType = LuaDLL.lua_type(L, valIdx);
            switch (valType)
            {
                case LuaTypes.LUA_TBOOLEAN:
                    info = GetInfo(LuaDLL.lua_toboolean(L, valIdx));
                    break;

                case LuaTypes.LUA_TSTRING:
                    var strVal = LuaDLL.lua_tostring(L, valIdx);
                    info = GetInfo(strVal);
                    break;

                case LuaTypes.LUA_TNUMBER:
                    double dbVal = LuaDLL.lua_tonumber(L, valIdx);
                    if (dbVal == (int)dbVal)
                        info = GetInfo((int)dbVal);
                    else
                        info = GetInfo(dbVal);
                    break;

                case LuaTypes.LUA_TUSERDATA:
                    TBinaryData binaryData = (TBinaryData)ToLua.ToObject(L, valIdx);
                    info = GetInfo(binaryData);
                    break;

                case LuaTypes.LUA_TTABLE:
                    info = GetInfo(valIdx, filter);
                    break;
                default:
                    return false;
            }

            return true;
        }

        public static bool SaveTable(IntPtr L, TBinaryData binBuff, string filter)
        {
            int tblIdx = LuaDLL.lua_gettop(L);
            LuaDLL.lua_pushnil(L);

            ValInfo keyInfo = default(ValInfo), valInfo = default(ValInfo);
            while (0 != LuaDLL.lua_next(L, tblIdx))
            {
                if (GetKeyInfo(ref keyInfo, L, binBuff, -2, filter))
                {
                    if (GetValInfo(ref valInfo, L, binBuff, -1, filter))
                        Write(L, binBuff, keyInfo, valInfo);
                }
                LuaDLL.lua_pop(L, 1);
            }
            return true;
        }
        #endregion

        #region 反序列化
        /*
         * 返回值: -1 失败 0 结束 1 继续
         */
        static sbyte ReadKeyValInfo(IntPtr L, TBinaryData dataBuff, ref ValInfo keyInfo, ref ValInfo valInfo)
        {
            if (dataBuff.Size < 1)
                return -1;
            uint leadByte = dataBuff.ReadByte();
            if (leadByte == tblEnd)
                return 0;

            keyInfo.type = (int)leadByte & 1;//(B1)
            valInfo.type = (int)((leadByte >> 1) & 7);//(B111)
            keyInfo.lenType = (int)((leadByte >> 4) & 3);//(B11)
            valInfo.lenType = (int)((leadByte >> 6) & 3);//(B11)
            return 1;
        }

        static bool Read(IntPtr L, TBinaryData dataBuff, ValInfo info)
        {
            switch((LuakvType)info.type)
            {
                case LuakvType.String:
                    LuaDLL.lua_pushstring(L, ReadString(dataBuff, info.lenType));
                    break;

                case LuakvType.Integer:
                    LuaDLL.lua_pushinteger(L, ReadInteger(dataBuff, info.lenType));
                    break;

                case LuakvType.Number:
                    LuaDLL.lua_pushnumber(L, dataBuff.ReadDouble());
                    break;

                case LuakvType.Boolean:
                    LuaDLL.lua_pushboolean(L, info.lenType);
                    break;

                case LuakvType.Array:
                    int arraySize = ReadInteger(dataBuff, info.lenType);
                    var newBinaryData = new TBinaryData(arraySize);
                    dataBuff.Read(newBinaryData.Buff, 0, arraySize);
                    newBinaryData.Write(null, 0, arraySize);
                    ToLua.PushObject(L, newBinaryData);
                    break;

                case LuakvType.Table:
                    if (LoadTable(L, dataBuff) == false)
                        throw new Exception("Lua序列化, 反序列化表失败.");
                    break;

                default:
                    throw new Exception("Lua序列化, 未支持的类型");
            }
            return true;
        }
        
        private static string ReadString(TBinaryData dataBuff, int lenType)
        {
            if (lenType < 3)
            {
                var strLen = lenType + 1;
                var buff = dataBuff.GetBuffer();
                var pos = dataBuff.ReadPos;
                dataBuff.Read(null, 0, strLen);
                return Encoding.GetString(buff, pos, strLen);
            }
            else
            {
                var buff = dataBuff.ReadUntil(0);
                return Encoding.GetString(buff.GetBuffer(), 0, buff.Size);
            }
        }

        private static int ReadInteger(TBinaryData dataBuff, int lenType)
        {
            int v = 0;
            if (lenType == 0)
                v = dataBuff.ReadByte();
            else if (lenType == 1)
                v = dataBuff.ReadUShort();
            else if (lenType == 2)
                v = dataBuff.ReadUShort() | (dataBuff.ReadByte() << 16);
            else
                v = dataBuff.ReadInt();
            return v;
        }

        public static Encoding Encoding = Encoding.GetEncoding(LYBGlobalConsts.ENCODE);
        public static bool LoadTable(IntPtr L, TBinaryData dataBuff)
        {
            LuaDLL.lua_newtable(L);

            ValInfo keyInfo = default(ValInfo);
            ValInfo valInfo = default(ValInfo);
            sbyte ret = 0;
            while ((ret = ReadKeyValInfo(L, dataBuff, ref keyInfo, ref valInfo)) == 1)
            {
                if (!Read(L, dataBuff, keyInfo) || !Read(L, dataBuff, valInfo))
                    return false;
                LuaDLL.lua_settable(L, -3);
            }

            return true;
        }
        #endregion
    }
}
