﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class Network_Net_LYBMsgSerializerOutWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(Network.Net.LYBMsgSerializerOut), typeof(Network.Net.LYBSerializerOut));
		L.RegFunction("ReadRpcStreamAlign4B", ReadRpcStreamAlign4B);
		L.RegFunction("New", _CreateNetwork_Net_LYBMsgSerializerOut);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateNetwork_Net_LYBMsgSerializerOut(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				Network.Net.LYBMsgSerializerOut obj = new Network.Net.LYBMsgSerializerOut();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: Network.Net.LYBMsgSerializerOut.New");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReadRpcStreamAlign4B(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			Network.Net.LYBMsgSerializerOut obj = (Network.Net.LYBMsgSerializerOut)ToLua.CheckObject(L, 1, typeof(Network.Net.LYBMsgSerializerOut));
			Network.Net.LYBStreamOut o = obj.ReadRpcStreamAlign4B();
			ToLua.PushObject(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

