﻿using UnityEngine;
using System;
using LuaInterface;

public class System_ArrayWrap 
{
    public static void Register(LuaState L)
    {
        L.BeginClass(typeof(Array), typeof(System.Object));
        L.RegFunction(".geti", get_Item);
        L.RegFunction(".seti", set_Item);
        L.RegFunction("ToTable", ToTable);        
		L.RegFunction("GetLength", GetLength);
		L.RegFunction("GetLongLength", GetLongLength);
		L.RegFunction("GetLowerBound", GetLowerBound);
		L.RegFunction("GetValue", GetValue);
		L.RegFunction("SetValue", SetValue);
		L.RegFunction("GetEnumerator", GetEnumerator);
		L.RegFunction("GetUpperBound", GetUpperBound);
		L.RegFunction("CreateInstance", CreateInstance);
		L.RegFunction("BinarySearch", BinarySearch);
		L.RegFunction("Clear", Clear);
		L.RegFunction("Clone", Clone);
		L.RegFunction("Copy", Copy);
		L.RegFunction("IndexOf", IndexOf);
		L.RegFunction("Initialize", Initialize);
		L.RegFunction("LastIndexOf", LastIndexOf);
		L.RegFunction("Reverse", Reverse);
		L.RegFunction("Sort", Sort);
		L.RegFunction("CopyTo", CopyTo);
		L.RegFunction("ConstrainedCopy", ConstrainedCopy);
		L.RegFunction("__tostring", Lua_ToString);
		L.RegVar("Length", get_Length, null);
		L.RegVar("LongLength", get_LongLength, null);
		L.RegVar("Rank", get_Rank, null);
		L.RegVar("IsSynchronized", get_IsSynchronized, null);
		L.RegVar("SyncRoot", get_SyncRoot, null);
		L.RegVar("IsFixedSize", get_IsFixedSize, null);
		L.RegVar("IsReadOnly", get_IsReadOnly, null);
        L.EndClass();        
    }

    static bool GetPrimitiveValue(IntPtr L, object obj, Type t, int index)
    {
        bool flag = true;

        if (t == typeof(System.Single))
        {
            float[] array = obj as float[];
            float ret = array[index];
            LuaDLL.lua_pushnumber(L, ret);            
        }
        else if (t == typeof(System.Int32))
        {
            int[] array = obj as int[];
            int ret = array[index];
            LuaDLL.lua_pushinteger(L, ret);
        }
        else if (t == typeof(System.Double))
        {
            double[] array = obj as double[];
            double ret = array[index];
            LuaDLL.lua_pushnumber(L, ret);
        }
        else if (t == typeof(System.Boolean))
        {
            bool[] array = obj as bool[];
            bool ret = array[index];
            LuaDLL.lua_pushboolean(L, ret);
        }
        else if (t == typeof(System.Int64))
        {
            long[] array = obj as long[];
            long ret = array[index];
            LuaDLL.tolua_pushint64(L, ret);
        }
        else if (t == typeof(System.SByte))
        {
            sbyte[] array = obj as sbyte[];
            sbyte ret = array[index];
            LuaDLL.lua_pushnumber(L, ret);
        }
        else if (t == typeof(System.Byte))
        {
            byte[] array = obj as byte[];
            byte ret = array[index];
            LuaDLL.lua_pushnumber(L, ret);
        }
        else if (t == typeof(System.Int16))
        {
            short[] array = obj as short[];
            short ret = array[index];
            LuaDLL.lua_pushnumber(L, ret);
        }
        else if (t == typeof(System.UInt16))
        {
            ushort[] array = obj as ushort[];
            ushort ret = array[index];
            LuaDLL.lua_pushnumber(L, ret);
        }
        else if (t == typeof(System.Char))
        {
            char[] array = obj as char[];
            char ret = array[index];
            LuaDLL.lua_pushnumber(L, ret);
        }
        else if (t == typeof(System.UInt32))
        {
            uint[] array = obj as uint[];
            uint ret = array[index];
            LuaDLL.lua_pushnumber(L, ret);
        }
        else
        {
            flag = false;
        }

        return flag;
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_Item(IntPtr L)
    {
        try
        {
            Array obj = ToLua.ToObject(L, 1) as Array;

            if (obj == null)
            {
                throw new LuaException("trying to index an invalid object reference");                
            }

            int index = (int)LuaDLL.lua_tointeger(L, 2);

            if (index >= obj.Length)
            {
                throw new LuaException("array index out of bounds: " + index + " " + obj.Length);                
            }

            Type t = obj.GetType().GetElementType();

            if (t.IsValueType)
            {
                if (t.IsPrimitive)
                {
                    if (GetPrimitiveValue(L, obj, t, index))
                    {
                        return 1;
                    }
                }
                else if (t == typeof(Vector3))
                {
                    Vector3[] array = obj as Vector3[];
                    Vector3 ret = array[index];
                    ToLua.Push(L, ret);                    
                    return 1;
                }
                else if (t == typeof(Quaternion))
                {
                    Quaternion[] array = obj as Quaternion[];
                    Quaternion ret = array[index];
                    ToLua.Push(L, ret);
                    return 1;                    
                }
                else if (t == typeof(Vector2))
                {
                    Vector2[] array = obj as Vector2[];
                    Vector2 ret = array[index];
                    ToLua.Push(L, ret);
                    return 1;                    
                }
                else if (t == typeof(Vector4))
                {
                    Vector4[] array = obj as Vector4[];
                    Vector4 ret = array[index];
                    ToLua.Push(L, ret);
                    return 1;                    
                }
                else if (t == typeof(Color))
                {
                    Color[] array = obj as Color[];
                    Color ret = array[index];
                    ToLua.Push(L, ret);
                    return 1;                    
                }
            }            

            object val = obj.GetValue(index);
            ToLua.Push(L, val);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    static bool SetPrimitiveValue(IntPtr L, object obj, Type t, int index)
    {
        bool flag = true;

        if (t == typeof(System.Single))
        {
            float[] array = obj as float[];
            float val = (float)LuaDLL.luaL_checknumber(L, 3);
            array[index] = val;            
        }
        else if (t == typeof(System.Int32))
        {
            int[] array = obj as int[];
            int val = (int)LuaDLL.luaL_checkinteger(L, 3);
            array[index] = val;
        }
        else if (t == typeof(System.Double))
        {
            double[] array = obj as double[];
            double val = LuaDLL.luaL_checknumber(L, 3);
            array[index] = val;
        }
        else if (t == typeof(System.Boolean))
        {
            bool[] array = obj as bool[];
            bool val = LuaDLL.luaL_checkboolean(L, 3);
            array[index] = val;
        }
        else if (t == typeof(System.Int64))
        {
            long[] array = obj as long[];
            long val = LuaDLL.tolua_toint64(L, 3);
            array[index] = val;
        }
        else if (t == typeof(System.SByte))
        {
            sbyte[] array = obj as sbyte[];
            sbyte val = (sbyte)LuaDLL.luaL_checknumber(L, 3);
            array[index] = val;
        }
        else if (t == typeof(System.Byte))
        {
            byte[] array = obj as byte[];
            byte val = (byte)LuaDLL.luaL_checknumber(L, 3);
            array[index] = val;
        }
        else if (t == typeof(System.Int16))
        {
            short[] array = obj as short[];
            short val = (short)LuaDLL.luaL_checknumber(L, 3);
            array[index] = val;
        }
        else if (t == typeof(System.UInt16))
        {
            ushort[] array = obj as ushort[];
            ushort val = (ushort)LuaDLL.luaL_checknumber(L, 3);
            array[index] = val;
        }
        else if (t == typeof(System.Char))
        {
            char[] array = obj as char[];
            char val = (char)LuaDLL.luaL_checknumber(L, 3);
            array[index] = val;
        }
        else if (t == typeof(System.UInt32))
        {
            uint[] array = obj as uint[];
            uint val = (uint)LuaDLL.luaL_checknumber(L, 3);
            array[index] = val;
        }
        else
        {
            flag = false;
        }

        return flag;
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int set_Item(IntPtr L)
    {
        try
        {
            Array obj = ToLua.ToObject(L, 1) as Array;

            if (obj == null)
            {
                throw new LuaException("trying to index an invalid object reference");
            }

            int index = (int)LuaDLL.lua_tointeger(L, 2);            
            Type t = obj.GetType().GetElementType();

            if (t.IsValueType)
            {
                if (t.IsPrimitive)
                {
                    if (SetPrimitiveValue(L, obj, t, index))
                    {
                        return 0;
                    }
                }
                else if (t == typeof(Vector3))
                {
                    Vector3[] array = obj as Vector3[];
                    Vector3 val = ToLua.ToVector3(L, 3);
                    array[index] = val;
                    return 0;
                }
                else if (t == typeof(Quaternion))
                {
                    Quaternion[] array = obj as Quaternion[];
                    Quaternion val = ToLua.ToQuaternion(L, 3);
                    array[index] = val;
                    return 0;
                }
                else if (t == typeof(Vector2))
                {
                    Vector2[] array = obj as Vector2[];
                    Vector2 val = ToLua.ToVector2(L, 3);
                    array[index] = val;
                    return 0;
                }
                else if (t == typeof(Vector4))
                {
                    Vector4[] array = obj as Vector4[];
                    Vector4 val = ToLua.ToVector4(L, 3);
                    array[index] = val;
                    return 0;
                }
                else if (t == typeof(Color))
                {
                    Color[] array = obj as Color[];
                    Color val = ToLua.ToColor(L, 3);
                    array[index] = val;
                    return 0;
                }
            }

            if (!TypeChecker.CheckType(L, t, 3))
            {                                
                return LuaDLL.luaL_typerror(L, 3, LuaMisc.GetTypeName(t));
            }

            object v = ToLua.CheckVarObject(L, 3, t);
            v = TypeChecker.ChangeType(v, t);
            obj.SetValue(v, index);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_Length(IntPtr L)
    {
        try
        {
            Array obj = ToLua.ToObject(L, 1) as Array;

            if (obj == null)
            {
                throw new LuaException("trying to index an invalid object reference");                
            }

            LuaDLL.lua_pushinteger(L, obj.Length);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int ToTable(IntPtr L)
    {
        try
        {
            Array obj = ToLua.ToObject(L, 1) as Array;

            if (obj == null)
            {
                throw new LuaException("trying to index an invalid object reference");                
            }

            LuaDLL.lua_createtable(L, obj.Length, 0);
            Type t = obj.GetType().GetElementType();

            if (t.IsValueType)
            {
                if (t.IsPrimitive)
                {
                    if (t == typeof(System.Single))
                    {
                        float[] array = obj as float[];

                        for (int i = 0; i < array.Length; i++)
                        {
                            float ret = array[i];
                            LuaDLL.lua_pushnumber(L, ret);
                            LuaDLL.lua_rawseti(L, -2, i + 1);
                        }

                        return 1;
                    }
                    else if (t == typeof(System.Int32))
                    {
                        int[] array = obj as int[];

                        for (int i = 0; i < array.Length; i++)
                        {
                            int ret = array[i];
                            LuaDLL.lua_pushinteger(L, ret);
                            LuaDLL.lua_rawseti(L, -2, i + 1);
                        }

                        return 1;
                    }
                    else if (t == typeof(System.Double))
                    {
                        double[] array = obj as double[];

                        for (int i = 0; i < array.Length; i++)
                        {
                            double ret = array[i];
                            LuaDLL.lua_pushnumber(L, ret);
                            LuaDLL.lua_rawseti(L, -2, i + 1);
                        }

                        return 1;
                    }
                    else if (t == typeof(System.Boolean))
                    {
                        bool[] array = obj as bool[];

                        for (int i = 0; i < array.Length; i++)
                        {
                            bool ret = array[i];
                            LuaDLL.lua_pushboolean(L, ret);
                            LuaDLL.lua_rawseti(L, -2, i + 1);
                        }

                        return 1;
                    }
                    else if (t == typeof(System.Int64))
                    {
                        long[] array = obj as long[];

                        for (int i = 0; i < array.Length; i++)
                        {
                            long ret = array[i];
                            LuaDLL.tolua_pushint64(L, ret);
                            LuaDLL.lua_rawseti(L, -2, i + 1);
                        }

                        return 1;
                    }
                    else if (t == typeof(System.Byte))
                    {
                        byte[] array = obj as byte[];

                        for (int i = 0; i < array.Length; i++)
                        {
                            byte ret = array[i];
                            LuaDLL.lua_pushnumber(L, ret);
                            LuaDLL.lua_rawseti(L, -2, i + 1);
                        }

                        return 1;
                    }
                    else if (t == typeof(System.SByte))
                    {
                        sbyte[] array = obj as sbyte[];

                        for (int i = 0; i < array.Length; i++)
                        {
                            sbyte ret = array[i];
                            LuaDLL.lua_pushnumber(L, ret);
                            LuaDLL.lua_rawseti(L, -2, i + 1);
                        }

                        return 1;
                    }
                    else if (t == typeof(System.Char))
                    {
                        char[] array = obj as char[];

                        for (int i = 0; i < array.Length; i++)
                        {
                            char ret = array[i];
                            LuaDLL.lua_pushnumber(L, ret);
                            LuaDLL.lua_rawseti(L, -2, i + 1);
                        }

                        return 1;
                    }
                    else if (t == typeof(System.UInt32))
                    {
                        uint[] array = obj as uint[];

                        for (int i = 0; i < array.Length; i++)
                        {
                            uint ret = array[i];
                            LuaDLL.lua_pushnumber(L, ret);
                            LuaDLL.lua_rawseti(L, -2, i + 1);
                        }

                        return 1;
                    }
                    else if (t == typeof(System.Int16))
                    {
                        short[] array = obj as short[];

                        for (int i = 0; i < array.Length; i++)
                        {
                            short ret = array[i];
                            LuaDLL.lua_pushnumber(L, ret);
                            LuaDLL.lua_rawseti(L, -2, i + 1);
                        }

                        return 1;
                    }
                    else if (t == typeof(System.UInt16))
                    {
                        ushort[] array = obj as ushort[];

                        for (int i = 0; i < array.Length; i++)
                        {
                            ushort ret = array[i];
                            LuaDLL.lua_pushnumber(L, ret);
                            LuaDLL.lua_rawseti(L, -2, i + 1);
                        }

                        return 1;
                    }
                }
                else if (t == typeof(Vector3))
                {
                    Vector3[] array = obj as Vector3[];

                    for (int i = 0; i < array.Length; i++)
                    {
                        Vector3 ret = array[i];
                        ToLua.Push(L, ret);
                        LuaDLL.lua_rawseti(L, -2, i + 1);
                    }

                    return 1;
                }
                else if (t == typeof(Quaternion))
                {
                    Quaternion[] array = obj as Quaternion[];

                    for (int i = 0; i < array.Length; i++)
                    {
                        Quaternion ret = array[i];
                        ToLua.Push(L, ret);
                        LuaDLL.lua_rawseti(L, -2, i + 1);
                    }

                    return 1;
                }
                else if (t == typeof(Vector2))
                {
                    Vector2[] array = obj as Vector2[];

                    for (int i = 0; i < array.Length; i++)
                    {
                        Vector2 ret = array[i];
                        ToLua.Push(L, ret);
                        LuaDLL.lua_rawseti(L, -2, i + 1);
                    }

                    return 1;
                }
                else if (t == typeof(Vector4))
                {
                    Vector4[] array = obj as Vector4[];

                    for (int i = 0; i < array.Length; i++)
                    {
                        Vector4 ret = array[i];
                        ToLua.Push(L, ret);
                        LuaDLL.lua_rawseti(L, -2, i + 1);
                    }

                    return 1;
                }
                else if (t == typeof(Color))
                {
                    Color[] array = obj as Color[];

                    for (int i = 0; i < array.Length; i++)
                    {
                        Color ret = array[i];
                        ToLua.Push(L, ret);
                        LuaDLL.lua_rawseti(L, -2, i + 1);
                    }

                    return 1;
                }
            }


            for (int i = 0; i < obj.Length; i++)
            {
                object val = obj.GetValue(i);
                ToLua.Push(L, val);
                LuaDLL.lua_rawseti(L, -2, i + 1);
            }

            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int GetLength(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 2);
            System.Array obj = (System.Array)ToLua.CheckObject(L, 1, typeof(System.Array));
            int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
            int o = obj.GetLength(arg0);
            LuaDLL.lua_pushinteger(L, o);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int GetLongLength(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 2);
            System.Array obj = (System.Array)ToLua.CheckObject(L, 1, typeof(System.Array));
            int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
            long o = obj.GetLongLength(arg0);
            LuaDLL.lua_pushnumber(L, o);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int GetLowerBound(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 2);
            System.Array obj = (System.Array)ToLua.CheckObject(L, 1, typeof(System.Array));
            int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
            int o = obj.GetLowerBound(arg0);
            LuaDLL.lua_pushinteger(L, o);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int GetValue(IntPtr L)
    {
        try
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(long)))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                long arg0 = (long)LuaDLL.lua_tonumber(L, 2);
                object o = obj.GetValue(arg0);
                ToLua.Push(L, o);
                return 1;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(long), typeof(long)))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                long arg0 = (long)LuaDLL.lua_tonumber(L, 2);
                long arg1 = (long)LuaDLL.lua_tonumber(L, 3);
                object o = obj.GetValue(arg0, arg1);
                ToLua.Push(L, o);
                return 1;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(int), typeof(int)))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                int arg0 = (int)LuaDLL.lua_tonumber(L, 2);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
                object o = obj.GetValue(arg0, arg1);
                ToLua.Push(L, o);
                return 1;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(long), typeof(long), typeof(long)))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                long arg0 = (long)LuaDLL.lua_tonumber(L, 2);
                long arg1 = (long)LuaDLL.lua_tonumber(L, 3);
                long arg2 = (long)LuaDLL.lua_tonumber(L, 4);
                object o = obj.GetValue(arg0, arg1, arg2);
                ToLua.Push(L, o);
                return 1;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(int), typeof(int), typeof(int)))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                int arg0 = (int)LuaDLL.lua_tonumber(L, 2);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 4);
                object o = obj.GetValue(arg0, arg1, arg2);
                ToLua.Push(L, o);
                return 1;
            }
            else if (TypeChecker.CheckParamsType(L, typeof(long), 2, count - 1))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                long[] arg0 = ToLua.ToParamsNumber<long>(L, 2, count - 1);
                object o = obj.GetValue(arg0);
                ToLua.Push(L, o);
                return 1;
            }
            else if (TypeChecker.CheckParamsType(L, typeof(int), 2, count - 1))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                int[] arg0 = ToLua.ToParamsNumber<int>(L, 2, count - 1);
                object o = obj.GetValue(arg0);
                ToLua.Push(L, o);
                return 1;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: System.Array.GetValue");
            }
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int SetValue(IntPtr L)
    {
        try
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object), typeof(long)))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                object arg0 = ToLua.ToVarObject(L, 2, obj.GetType().GetElementType());
                long arg1 = (long)LuaDLL.lua_tonumber(L, 3);
                obj.SetValue(arg0, arg1);
                return 0;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object), typeof(int), typeof(int)))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                object arg0 = ToLua.ToVarObject(L, 2, obj.GetType().GetElementType());
                int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 4);
                obj.SetValue(arg0, arg1, arg2);
                return 0;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object), typeof(long), typeof(long)))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                object arg0 = ToLua.ToVarObject(L, 2, obj.GetType().GetElementType());
                long arg1 = (long)LuaDLL.lua_tonumber(L, 3);
                long arg2 = (long)LuaDLL.lua_tonumber(L, 4);
                obj.SetValue(arg0, arg1, arg2);
                return 0;
            }
            else if (count == 5 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object), typeof(int), typeof(int), typeof(int)))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                object arg0 = ToLua.ToVarObject(L, 2, obj.GetType().GetElementType());
                int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 4);
                int arg3 = (int)LuaDLL.lua_tonumber(L, 5);
                obj.SetValue(arg0, arg1, arg2, arg3);
                return 0;
            }
            else if (count == 5 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object), typeof(long), typeof(long), typeof(long)))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                object arg0 = ToLua.ToVarObject(L, 2, obj.GetType().GetElementType());
                long arg1 = (long)LuaDLL.lua_tonumber(L, 3);
                long arg2 = (long)LuaDLL.lua_tonumber(L, 4);
                long arg3 = (long)LuaDLL.lua_tonumber(L, 5);
                obj.SetValue(arg0, arg1, arg2, arg3);
                return 0;
            }
            else if (TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object)) && TypeChecker.CheckParamsType(L, typeof(long), 3, count - 2))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                object arg0 = ToLua.ToVarObject(L, 2, obj.GetType().GetElementType());
                long[] arg1 = ToLua.ToParamsNumber<long>(L, 3, count - 2);
                obj.SetValue(arg0, arg1);
                return 0;
            }
            else if (TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object)) && TypeChecker.CheckParamsType(L, typeof(int), 3, count - 2))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                object arg0 = ToLua.ToVarObject(L, 2, obj.GetType().GetElementType());
                int[] arg1 = ToLua.ToParamsNumber<int>(L, 3, count - 2);
                obj.SetValue(arg0, arg1);
                return 0;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: System.Array.SetValue");
            }
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int GetEnumerator(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 1);
            System.Array obj = (System.Array)ToLua.CheckObject(L, 1, typeof(System.Array));
            System.Collections.IEnumerator o = obj.GetEnumerator();
            ToLua.Push(L, o);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int GetUpperBound(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 2);
            System.Array obj = (System.Array)ToLua.CheckObject(L, 1, typeof(System.Array));
            int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
            int o = obj.GetUpperBound(arg0);
            LuaDLL.lua_pushinteger(L, o);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int CreateInstance(IntPtr L)
    {
        try
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(System.Type), typeof(int)))
            {
                System.Type arg0 = (System.Type)ToLua.ToObject(L, 1);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
                System.Array o = System.Array.CreateInstance(arg0, arg1);
                ToLua.Push(L, o);
                return 1;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Type), typeof(int[]), typeof(int[])))
            {
                System.Type arg0 = (System.Type)ToLua.ToObject(L, 1);
                int[] arg1 = ToLua.CheckNumberArray<int>(L, 2);
                int[] arg2 = ToLua.CheckNumberArray<int>(L, 3);
                System.Array o = System.Array.CreateInstance(arg0, arg1, arg2);
                ToLua.Push(L, o);
                return 1;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Type), typeof(int), typeof(int)))
            {
                System.Type arg0 = (System.Type)ToLua.ToObject(L, 1);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                System.Array o = System.Array.CreateInstance(arg0, arg1, arg2);
                ToLua.Push(L, o);
                return 1;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(System.Type), typeof(int), typeof(int), typeof(int)))
            {
                System.Type arg0 = (System.Type)ToLua.ToObject(L, 1);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                int arg3 = (int)LuaDLL.lua_tonumber(L, 4);
                System.Array o = System.Array.CreateInstance(arg0, arg1, arg2, arg3);
                ToLua.Push(L, o);
                return 1;
            }
            else if (TypeChecker.CheckTypes(L, 1, typeof(System.Type)) && TypeChecker.CheckParamsType(L, typeof(long), 2, count - 1))
            {
                System.Type arg0 = (System.Type)ToLua.ToObject(L, 1);
                long[] arg1 = ToLua.ToParamsNumber<long>(L, 2, count - 1);
                System.Array o = System.Array.CreateInstance(arg0, arg1);
                ToLua.Push(L, o);
                return 1;
            }
            else if (TypeChecker.CheckTypes(L, 1, typeof(System.Type)) && TypeChecker.CheckParamsType(L, typeof(int), 2, count - 1))
            {
                System.Type arg0 = (System.Type)ToLua.ToObject(L, 1);
                int[] arg1 = ToLua.ToParamsNumber<int>(L, 2, count - 1);
                System.Array o = System.Array.CreateInstance(arg0, arg1);
                ToLua.Push(L, o);
                return 1;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: System.Array.CreateInstance");
            }
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int BinarySearch(IntPtr L)
    {
        try
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                object arg1 = ToLua.ToVarObject(L, 2, arg0.GetType().GetElementType());
                int o = System.Array.BinarySearch(arg0, arg1);
                LuaDLL.lua_pushinteger(L, o);
                return 1;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object), typeof(System.Collections.IComparer)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                object arg1 = ToLua.ToVarObject(L, 2, arg0.GetType().GetElementType());
                System.Collections.IComparer arg2 = (System.Collections.IComparer)ToLua.ToObject(L, 3);
                int o = System.Array.BinarySearch(arg0, arg1, arg2);
                LuaDLL.lua_pushinteger(L, o);
                return 1;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(int), typeof(int), typeof(object)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                object arg3 = ToLua.ToVarObject(L, 4, arg0.GetType().GetElementType());
                int o = System.Array.BinarySearch(arg0, arg1, arg2, arg3);
                LuaDLL.lua_pushinteger(L, o);
                return 1;
            }
            else if (count == 5 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(int), typeof(int), typeof(object), typeof(System.Collections.IComparer)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                object arg3 = ToLua.ToVarObject(L, 4, arg0.GetType().GetElementType());
                System.Collections.IComparer arg4 = (System.Collections.IComparer)ToLua.ToObject(L, 5);
                int o = System.Array.BinarySearch(arg0, arg1, arg2, arg3, arg4);
                LuaDLL.lua_pushinteger(L, o);
                return 1;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: System.Array.BinarySearch");
            }
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Clear(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 3);
            System.Array arg0 = (System.Array)ToLua.CheckObject(L, 1, typeof(System.Array));
            int arg1 = (int)LuaDLL.luaL_checknumber(L, 2);
            int arg2 = (int)LuaDLL.luaL_checknumber(L, 3);
            System.Array.Clear(arg0, arg1, arg2);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Clone(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 1);
            System.Array obj = (System.Array)ToLua.CheckObject(L, 1, typeof(System.Array));
            object o = obj.Clone();
            ToLua.Push(L, o);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Copy(IntPtr L)
    {
        try
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(System.Array), typeof(long)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                System.Array arg1 = (System.Array)ToLua.ToObject(L, 2);
                long arg2 = (long)LuaDLL.lua_tonumber(L, 3);
                System.Array.Copy(arg0, arg1, arg2);
                return 0;
            }
            else if (count == 5 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(long), typeof(System.Array), typeof(long), typeof(long)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                long arg1 = (long)LuaDLL.lua_tonumber(L, 2);
                System.Array arg2 = (System.Array)ToLua.ToObject(L, 3);
                long arg3 = (long)LuaDLL.lua_tonumber(L, 4);
                long arg4 = (long)LuaDLL.lua_tonumber(L, 5);
                System.Array.Copy(arg0, arg1, arg2, arg3, arg4);
                return 0;
            }
            else if (count == 5 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(int), typeof(System.Array), typeof(int), typeof(int)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
                System.Array arg2 = (System.Array)ToLua.ToObject(L, 3);
                int arg3 = (int)LuaDLL.lua_tonumber(L, 4);
                int arg4 = (int)LuaDLL.lua_tonumber(L, 5);
                System.Array.Copy(arg0, arg1, arg2, arg3, arg4);
                return 0;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: System.Array.Copy");
            }
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int IndexOf(IntPtr L)
    {
        try
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                object arg1 = ToLua.ToVarObject(L, 2, arg0.GetType().GetElementType());
                int o = System.Array.IndexOf(arg0, arg1);
                LuaDLL.lua_pushinteger(L, o);
                return 1;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object), typeof(int)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                object arg1 = ToLua.ToVarObject(L, 2, arg0.GetType().GetElementType());
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                int o = System.Array.IndexOf(arg0, arg1, arg2);
                LuaDLL.lua_pushinteger(L, o);
                return 1;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object), typeof(int), typeof(int)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                object arg1 = ToLua.ToVarObject(L, 2, arg0.GetType().GetElementType());
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                int arg3 = (int)LuaDLL.lua_tonumber(L, 4);
                int o = System.Array.IndexOf(arg0, arg1, arg2, arg3);
                LuaDLL.lua_pushinteger(L, o);
                return 1;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: System.Array.IndexOf");
            }
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Initialize(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 1);
            System.Array obj = (System.Array)ToLua.CheckObject(L, 1, typeof(System.Array));
            obj.Initialize();
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int LastIndexOf(IntPtr L)
    {
        try
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                object arg1 = ToLua.ToVarObject(L, 2, arg0.GetType().GetElementType());
                int o = System.Array.LastIndexOf(arg0, arg1);
                LuaDLL.lua_pushinteger(L, o);
                return 1;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object), typeof(int)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                object arg1 = ToLua.ToVarObject(L, 2, arg0.GetType().GetElementType());
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                int o = System.Array.LastIndexOf(arg0, arg1, arg2);
                LuaDLL.lua_pushinteger(L, o);
                return 1;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object), typeof(int), typeof(int)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                object arg1 = ToLua.ToVarObject(L, 2, arg0.GetType().GetElementType());
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                int arg3 = (int)LuaDLL.lua_tonumber(L, 4);
                int o = System.Array.LastIndexOf(arg0, arg1, arg2, arg3);
                LuaDLL.lua_pushinteger(L, o);
                return 1;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: System.Array.LastIndexOf");
            }
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Reverse(IntPtr L)
    {
        try
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 1 && TypeChecker.CheckTypes(L, 1, typeof(System.Array)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                System.Array.Reverse(arg0);
                return 0;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(int), typeof(int)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                System.Array.Reverse(arg0, arg1, arg2);
                return 0;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: System.Array.Reverse");
            }
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Sort(IntPtr L)
    {
        try
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 1 && TypeChecker.CheckTypes(L, 1, typeof(System.Array)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                System.Array.Sort(arg0);
                return 0;
            }
            else if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(System.Collections.IComparer)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                System.Collections.IComparer arg1 = (System.Collections.IComparer)ToLua.ToObject(L, 2);
                System.Array.Sort(arg0, arg1);
                return 0;
            }
            else if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(System.Array)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                System.Array arg1 = (System.Array)ToLua.ToObject(L, 2);
                System.Array.Sort(arg0, arg1);
                return 0;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(System.Array), typeof(System.Collections.IComparer)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                System.Array arg1 = (System.Array)ToLua.ToObject(L, 2);
                System.Collections.IComparer arg2 = (System.Collections.IComparer)ToLua.ToObject(L, 3);
                System.Array.Sort(arg0, arg1, arg2);
                return 0;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(int), typeof(int)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                System.Array.Sort(arg0, arg1, arg2);
                return 0;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(int), typeof(int), typeof(System.Collections.IComparer)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                System.Collections.IComparer arg3 = (System.Collections.IComparer)ToLua.ToObject(L, 4);
                System.Array.Sort(arg0, arg1, arg2, arg3);
                return 0;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(System.Array), typeof(int), typeof(int)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                System.Array arg1 = (System.Array)ToLua.ToObject(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                int arg3 = (int)LuaDLL.lua_tonumber(L, 4);
                System.Array.Sort(arg0, arg1, arg2, arg3);
                return 0;
            }
            else if (count == 5 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(System.Array), typeof(int), typeof(int), typeof(System.Collections.IComparer)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                System.Array arg1 = (System.Array)ToLua.ToObject(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                int arg3 = (int)LuaDLL.lua_tonumber(L, 4);
                System.Collections.IComparer arg4 = (System.Collections.IComparer)ToLua.ToObject(L, 5);
                System.Array.Sort(arg0, arg1, arg2, arg3, arg4);
                return 0;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: System.Array.Sort");
            }
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int CopyTo(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 3);
            System.Array obj = (System.Array)ToLua.CheckObject(L, 1, typeof(System.Array));
            System.Array arg0 = (System.Array)ToLua.CheckObject(L, 2, typeof(System.Array));
            long arg1 = (long)LuaDLL.luaL_checknumber(L, 3);
            obj.CopyTo(arg0, arg1);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int ConstrainedCopy(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 5);
            System.Array arg0 = (System.Array)ToLua.CheckObject(L, 1, typeof(System.Array));
            int arg1 = (int)LuaDLL.luaL_checknumber(L, 2);
            System.Array arg2 = (System.Array)ToLua.CheckObject(L, 3, typeof(System.Array));
            int arg3 = (int)LuaDLL.luaL_checknumber(L, 4);
            int arg4 = (int)LuaDLL.luaL_checknumber(L, 5);
            System.Array.ConstrainedCopy(arg0, arg1, arg2, arg3, arg4);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Lua_ToString(IntPtr L)
    {
        object obj = ToLua.ToObject(L, 1);

        if (obj != null)
        {
            LuaDLL.lua_pushstring(L, obj.ToString());
        }
        else
        {
            LuaDLL.lua_pushnil(L);
        }

        return 1;
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_LongLength(IntPtr L)
    {
        object o = null;

        try
        {
            o = ToLua.ToObject(L, 1);
            System.Array obj = (System.Array)o;
            long ret = obj.LongLength;
            LuaDLL.lua_pushnumber(L, ret);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index LongLength on a nil value" : e.Message);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_Rank(IntPtr L)
    {
        object o = null;

        try
        {
            o = ToLua.ToObject(L, 1);
            System.Array obj = (System.Array)o;
            int ret = obj.Rank;
            LuaDLL.lua_pushinteger(L, ret);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index Rank on a nil value" : e.Message);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_IsSynchronized(IntPtr L)
    {
        object o = null;

        try
        {
            o = ToLua.ToObject(L, 1);
            System.Array obj = (System.Array)o;
            bool ret = obj.IsSynchronized;
            LuaDLL.lua_pushboolean(L, ret);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index IsSynchronized on a nil value" : e.Message);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_SyncRoot(IntPtr L)
    {
        object o = null;

        try
        {
            o = ToLua.ToObject(L, 1);
            System.Array obj = (System.Array)o;
            object ret = obj.SyncRoot;
            ToLua.Push(L, ret);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index SyncRoot on a nil value" : e.Message);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_IsFixedSize(IntPtr L)
    {
        object o = null;

        try
        {
            o = ToLua.ToObject(L, 1);
            System.Array obj = (System.Array)o;
            bool ret = obj.IsFixedSize;
            LuaDLL.lua_pushboolean(L, ret);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index IsFixedSize on a nil value" : e.Message);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_IsReadOnly(IntPtr L)
    {
        object o = null;

        try
        {
            o = ToLua.ToObject(L, 1);
            System.Array obj = (System.Array)o;
            bool ret = obj.IsReadOnly;
            LuaDLL.lua_pushboolean(L, ret);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index IsReadOnly on a nil value" : e.Message);
        }
    }
}
