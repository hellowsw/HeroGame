﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class UnityDLL_LYBServerBaseWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UnityDLL.LYBServerBase), typeof(System.Object));
		L.RegFunction("IsServerLink", IsServerLink);
		L.RegFunction("OnConnected", OnConnected);
		L.RegFunction("Reconnect", Reconnect);
		L.RegFunction("OnDisconnected", OnDisconnected);
		L.RegFunction("OnDestory", OnDestory);
		L.RegFunction("OnReuseSessionFailed", OnReuseSessionFailed);
		L.RegFunction("OnReuseSessionSuccess", OnReuseSessionSuccess);
		L.RegFunction("OnSendRequest", OnSendRequest);
		L.RegFunction("Close", Close);
		L.RegFunction("Execute", Execute);
		L.RegFunction("New", _CreateUnityDLL_LYBServerBase);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("AutoReconnect", get_AutoReconnect, set_AutoReconnect);
		L.RegVar("Server", get_Server, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityDLL_LYBServerBase(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				string arg0 = ToLua.CheckString(L, 1);
				UnityDLL.LYBServerBase obj = new UnityDLL.LYBServerBase(arg0);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: UnityDLL.LYBServerBase.New");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsServerLink(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityDLL.LYBServerBase obj = (UnityDLL.LYBServerBase)ToLua.CheckObject(L, 1, typeof(UnityDLL.LYBServerBase));
			ulong arg0 = LuaDLL.tolua_checkuint64(L, 2);
			bool o = obj.IsServerLink(arg0);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnConnected(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			UnityDLL.LYBServerBase obj = (UnityDLL.LYBServerBase)ToLua.CheckObject(L, 1, typeof(UnityDLL.LYBServerBase));
			ulong arg0 = LuaDLL.tolua_checkuint64(L, 2);
			bool arg1 = LuaDLL.luaL_checkboolean(L, 3);
			obj.OnConnected(arg0, arg1);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Reconnect(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityDLL.LYBServerBase obj = (UnityDLL.LYBServerBase)ToLua.CheckObject(L, 1, typeof(UnityDLL.LYBServerBase));
			double arg0 = (double)LuaDLL.luaL_checknumber(L, 2);
			obj.Reconnect(arg0);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnDisconnected(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityDLL.LYBServerBase obj = (UnityDLL.LYBServerBase)ToLua.CheckObject(L, 1, typeof(UnityDLL.LYBServerBase));
			ulong arg0 = LuaDLL.tolua_checkuint64(L, 2);
			obj.OnDisconnected(arg0);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnDestory(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityDLL.LYBServerBase obj = (UnityDLL.LYBServerBase)ToLua.CheckObject(L, 1, typeof(UnityDLL.LYBServerBase));
			ulong arg0 = LuaDLL.tolua_checkuint64(L, 2);
			obj.OnDestory(arg0);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnReuseSessionFailed(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			UnityDLL.LYBServerBase obj = (UnityDLL.LYBServerBase)ToLua.CheckObject(L, 1, typeof(UnityDLL.LYBServerBase));
			ulong arg0 = LuaDLL.tolua_checkuint64(L, 2);
			ulong arg1 = LuaDLL.tolua_checkuint64(L, 3);
			obj.OnReuseSessionFailed(arg0, arg1);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnReuseSessionSuccess(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			UnityDLL.LYBServerBase obj = (UnityDLL.LYBServerBase)ToLua.CheckObject(L, 1, typeof(UnityDLL.LYBServerBase));
			ulong arg0 = LuaDLL.tolua_checkuint64(L, 2);
			ulong arg1 = LuaDLL.tolua_checkuint64(L, 3);
			obj.OnReuseSessionSuccess(arg0, arg1);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnSendRequest(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityDLL.LYBServerBase obj = (UnityDLL.LYBServerBase)ToLua.CheckObject(L, 1, typeof(UnityDLL.LYBServerBase));
			Network.Net.NetLinkBase arg0 = (Network.Net.NetLinkBase)ToLua.CheckObject(L, 2, typeof(Network.Net.NetLinkBase));
			obj.OnSendRequest(arg0);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Close(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityDLL.LYBServerBase obj = (UnityDLL.LYBServerBase)ToLua.CheckObject(L, 1, typeof(UnityDLL.LYBServerBase));
			obj.Close();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Execute(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityDLL.LYBServerBase obj = (UnityDLL.LYBServerBase)ToLua.CheckObject(L, 1, typeof(UnityDLL.LYBServerBase));
			obj.Execute();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AutoReconnect(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityDLL.LYBServerBase obj = (UnityDLL.LYBServerBase)o;
			bool ret = obj.AutoReconnect;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index AutoReconnect on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Server(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityDLL.LYBServerBase obj = (UnityDLL.LYBServerBase)o;
			Network.Net.LYBClientLink ret = obj.Server;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index Server on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_AutoReconnect(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityDLL.LYBServerBase obj = (UnityDLL.LYBServerBase)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.AutoReconnect = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index AutoReconnect on a nil value" : e.Message);
		}
	}
}

