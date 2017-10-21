﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class UnityEngine_RenderTextureWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UnityEngine.RenderTexture), typeof(UnityEngine.Texture));
		L.RegFunction("GetTemporary", GetTemporary);
		L.RegFunction("ReleaseTemporary", ReleaseTemporary);
		L.RegFunction("Create", Create);
		L.RegFunction("Release", Release);
		L.RegFunction("IsCreated", IsCreated);
		L.RegFunction("DiscardContents", DiscardContents);
		L.RegFunction("MarkRestoreExpected", MarkRestoreExpected);
		L.RegFunction("GetNativeDepthBufferPtr", GetNativeDepthBufferPtr);
		L.RegFunction("SetGlobalShaderProperty", SetGlobalShaderProperty);
		L.RegFunction("GetTexelOffset", GetTexelOffset);
		L.RegFunction("SupportsStencil", SupportsStencil);
		L.RegFunction("New", _CreateUnityEngine_RenderTexture);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("width", get_width, set_width);
		L.RegVar("height", get_height, set_height);
		L.RegVar("depth", get_depth, set_depth);
		L.RegVar("isPowerOfTwo", get_isPowerOfTwo, set_isPowerOfTwo);
		L.RegVar("sRGB", get_sRGB, null);
		L.RegVar("format", get_format, set_format);
		L.RegVar("useMipMap", get_useMipMap, set_useMipMap);
		L.RegVar("generateMips", get_generateMips, set_generateMips);
		L.RegVar("dimension", get_dimension, set_dimension);
		L.RegVar("volumeDepth", get_volumeDepth, set_volumeDepth);
		L.RegVar("antiAliasing", get_antiAliasing, set_antiAliasing);
		L.RegVar("enableRandomWrite", get_enableRandomWrite, set_enableRandomWrite);
		L.RegVar("colorBuffer", get_colorBuffer, null);
		L.RegVar("depthBuffer", get_depthBuffer, null);
		L.RegVar("active", get_active, set_active);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_RenderTexture(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 3)
			{
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 1);
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 2);
				int arg2 = (int)LuaDLL.luaL_checknumber(L, 3);
				UnityEngine.RenderTexture obj = new UnityEngine.RenderTexture(arg0, arg1, arg2);
				ToLua.Push(L, obj);
				return 1;
			}
			else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(int), typeof(int), typeof(int), typeof(UnityEngine.RenderTextureFormat)))
			{
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 1);
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 2);
				int arg2 = (int)LuaDLL.luaL_checknumber(L, 3);
				UnityEngine.RenderTextureFormat arg3 = (UnityEngine.RenderTextureFormat)ToLua.CheckObject(L, 4, typeof(UnityEngine.RenderTextureFormat));
				UnityEngine.RenderTexture obj = new UnityEngine.RenderTexture(arg0, arg1, arg2, arg3);
				ToLua.Push(L, obj);
				return 1;
			}
			else if (count == 5 && TypeChecker.CheckTypes(L, 1, typeof(int), typeof(int), typeof(int), typeof(UnityEngine.RenderTextureFormat), typeof(UnityEngine.RenderTextureReadWrite)))
			{
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 1);
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 2);
				int arg2 = (int)LuaDLL.luaL_checknumber(L, 3);
				UnityEngine.RenderTextureFormat arg3 = (UnityEngine.RenderTextureFormat)ToLua.CheckObject(L, 4, typeof(UnityEngine.RenderTextureFormat));
				UnityEngine.RenderTextureReadWrite arg4 = (UnityEngine.RenderTextureReadWrite)ToLua.CheckObject(L, 5, typeof(UnityEngine.RenderTextureReadWrite));
				UnityEngine.RenderTexture obj = new UnityEngine.RenderTexture(arg0, arg1, arg2, arg3, arg4);
				ToLua.Push(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: UnityEngine.RenderTexture.New");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTemporary(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(int), typeof(int)))
			{
				int arg0 = (int)LuaDLL.lua_tonumber(L, 1);
				int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
				UnityEngine.RenderTexture o = UnityEngine.RenderTexture.GetTemporary(arg0, arg1);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(int), typeof(int), typeof(int)))
			{
				int arg0 = (int)LuaDLL.lua_tonumber(L, 1);
				int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
				int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
				UnityEngine.RenderTexture o = UnityEngine.RenderTexture.GetTemporary(arg0, arg1, arg2);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(int), typeof(int), typeof(int), typeof(UnityEngine.RenderTextureFormat)))
			{
				int arg0 = (int)LuaDLL.lua_tonumber(L, 1);
				int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
				int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
				UnityEngine.RenderTextureFormat arg3 = (UnityEngine.RenderTextureFormat)ToLua.ToObject(L, 4);
				UnityEngine.RenderTexture o = UnityEngine.RenderTexture.GetTemporary(arg0, arg1, arg2, arg3);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 5 && TypeChecker.CheckTypes(L, 1, typeof(int), typeof(int), typeof(int), typeof(UnityEngine.RenderTextureFormat), typeof(UnityEngine.RenderTextureReadWrite)))
			{
				int arg0 = (int)LuaDLL.lua_tonumber(L, 1);
				int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
				int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
				UnityEngine.RenderTextureFormat arg3 = (UnityEngine.RenderTextureFormat)ToLua.ToObject(L, 4);
				UnityEngine.RenderTextureReadWrite arg4 = (UnityEngine.RenderTextureReadWrite)ToLua.ToObject(L, 5);
				UnityEngine.RenderTexture o = UnityEngine.RenderTexture.GetTemporary(arg0, arg1, arg2, arg3, arg4);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 6 && TypeChecker.CheckTypes(L, 1, typeof(int), typeof(int), typeof(int), typeof(UnityEngine.RenderTextureFormat), typeof(UnityEngine.RenderTextureReadWrite), typeof(int)))
			{
				int arg0 = (int)LuaDLL.lua_tonumber(L, 1);
				int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
				int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
				UnityEngine.RenderTextureFormat arg3 = (UnityEngine.RenderTextureFormat)ToLua.ToObject(L, 4);
				UnityEngine.RenderTextureReadWrite arg4 = (UnityEngine.RenderTextureReadWrite)ToLua.ToObject(L, 5);
				int arg5 = (int)LuaDLL.lua_tonumber(L, 6);
				UnityEngine.RenderTexture o = UnityEngine.RenderTexture.GetTemporary(arg0, arg1, arg2, arg3, arg4, arg5);
				ToLua.Push(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.RenderTexture.GetTemporary");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReleaseTemporary(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.RenderTexture arg0 = (UnityEngine.RenderTexture)ToLua.CheckUnityObject(L, 1, typeof(UnityEngine.RenderTexture));
			UnityEngine.RenderTexture.ReleaseTemporary(arg0);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Create(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)ToLua.CheckObject(L, 1, typeof(UnityEngine.RenderTexture));
			bool o = obj.Create();
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Release(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)ToLua.CheckObject(L, 1, typeof(UnityEngine.RenderTexture));
			obj.Release();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsCreated(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)ToLua.CheckObject(L, 1, typeof(UnityEngine.RenderTexture));
			bool o = obj.IsCreated();
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DiscardContents(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.RenderTexture)))
			{
				UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)ToLua.ToObject(L, 1);
				obj.DiscardContents();
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.RenderTexture), typeof(bool), typeof(bool)))
			{
				UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)ToLua.ToObject(L, 1);
				bool arg0 = LuaDLL.lua_toboolean(L, 2);
				bool arg1 = LuaDLL.lua_toboolean(L, 3);
				obj.DiscardContents(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.RenderTexture.DiscardContents");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MarkRestoreExpected(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)ToLua.CheckObject(L, 1, typeof(UnityEngine.RenderTexture));
			obj.MarkRestoreExpected();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetNativeDepthBufferPtr(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)ToLua.CheckObject(L, 1, typeof(UnityEngine.RenderTexture));
			System.IntPtr o = obj.GetNativeDepthBufferPtr();
			LuaDLL.lua_pushlightuserdata(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetGlobalShaderProperty(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)ToLua.CheckObject(L, 1, typeof(UnityEngine.RenderTexture));
			string arg0 = ToLua.CheckString(L, 2);
			obj.SetGlobalShaderProperty(arg0);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTexelOffset(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)ToLua.CheckObject(L, 1, typeof(UnityEngine.RenderTexture));
			UnityEngine.Vector2 o = obj.GetTexelOffset();
			ToLua.Push(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SupportsStencil(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.RenderTexture arg0 = (UnityEngine.RenderTexture)ToLua.CheckUnityObject(L, 1, typeof(UnityEngine.RenderTexture));
			bool o = UnityEngine.RenderTexture.SupportsStencil(arg0);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
			bool o = arg0 == arg1;
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_width(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			int ret = obj.width;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index width on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_height(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			int ret = obj.height;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index height on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_depth(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			int ret = obj.depth;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index depth on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isPowerOfTwo(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			bool ret = obj.isPowerOfTwo;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index isPowerOfTwo on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_sRGB(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			bool ret = obj.sRGB;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index sRGB on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_format(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			UnityEngine.RenderTextureFormat ret = obj.format;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index format on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_useMipMap(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			bool ret = obj.useMipMap;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index useMipMap on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_generateMips(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			bool ret = obj.generateMips;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index generateMips on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_dimension(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			UnityEngine.Rendering.TextureDimension ret = obj.dimension;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index dimension on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_volumeDepth(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			int ret = obj.volumeDepth;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index volumeDepth on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_antiAliasing(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			int ret = obj.antiAliasing;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index antiAliasing on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_enableRandomWrite(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			bool ret = obj.enableRandomWrite;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index enableRandomWrite on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_colorBuffer(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			UnityEngine.RenderBuffer ret = obj.colorBuffer;
			ToLua.PushValue(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index colorBuffer on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_depthBuffer(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			UnityEngine.RenderBuffer ret = obj.depthBuffer;
			ToLua.PushValue(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index depthBuffer on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_active(IntPtr L)
	{
		try
		{
			ToLua.Push(L, UnityEngine.RenderTexture.active);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_width(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			obj.width = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index width on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_height(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			obj.height = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index height on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_depth(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			obj.depth = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index depth on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_isPowerOfTwo(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.isPowerOfTwo = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index isPowerOfTwo on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_format(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			UnityEngine.RenderTextureFormat arg0 = (UnityEngine.RenderTextureFormat)ToLua.CheckObject(L, 2, typeof(UnityEngine.RenderTextureFormat));
			obj.format = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index format on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_useMipMap(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.useMipMap = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index useMipMap on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_generateMips(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.generateMips = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index generateMips on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_dimension(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			UnityEngine.Rendering.TextureDimension arg0 = (UnityEngine.Rendering.TextureDimension)ToLua.CheckObject(L, 2, typeof(UnityEngine.Rendering.TextureDimension));
			obj.dimension = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index dimension on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_volumeDepth(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			obj.volumeDepth = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index volumeDepth on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_antiAliasing(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			obj.antiAliasing = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index antiAliasing on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_enableRandomWrite(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.RenderTexture obj = (UnityEngine.RenderTexture)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.enableRandomWrite = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index enableRandomWrite on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_active(IntPtr L)
	{
		try
		{
			UnityEngine.RenderTexture arg0 = (UnityEngine.RenderTexture)ToLua.CheckUnityObject(L, 2, typeof(UnityEngine.RenderTexture));
			UnityEngine.RenderTexture.active = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

