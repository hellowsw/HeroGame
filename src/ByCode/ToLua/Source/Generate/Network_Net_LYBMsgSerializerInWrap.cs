﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class Network_Net_LYBMsgSerializerInWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(Network.Net.LYBMsgSerializerIn), typeof(Network.Net.LYBSerializerIn));
		L.RegFunction("WriteRpcStreamAlign4B", WriteRpcStreamAlign4B);
		L.RegFunction("New", _CreateNetwork_Net_LYBMsgSerializerIn);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateNetwork_Net_LYBMsgSerializerIn(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				Network.Net.LYBMsgSerializerIn obj = new Network.Net.LYBMsgSerializerIn();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: Network.Net.LYBMsgSerializerIn.New");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int WriteRpcStreamAlign4B(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			Network.Net.LYBMsgSerializerIn obj = (Network.Net.LYBMsgSerializerIn)ToLua.CheckObject(L, 1, typeof(Network.Net.LYBMsgSerializerIn));
			Network.Net.LYBStream arg0 = (Network.Net.LYBStream)ToLua.CheckObject(L, 2, typeof(Network.Net.LYBStream));
			obj.WriteRpcStreamAlign4B(arg0);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

