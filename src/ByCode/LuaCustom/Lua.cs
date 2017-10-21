using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;
using System.IO;
using GameCommon;
using GameLogic;

namespace UnityDLL.LuaCustom
{
    //===============================================================================
    public class FFLuaState : LuaState
    {
        //---------------------------------------------------------------------------
        public IntPtr GetL()
        {
            return L;
        }
    }

    //===============================================================================
    public static class Lua
    {
        #region Public Properties
        //---------------------------------------------------------------------------
        public static string ZbsDebuggerAddress = string.Empty;

        private static LuaLoader loader;

        //---------------------------------------------------------------------------
        public static FFLuaState lua
        {
            get { return FFLuaState; }
        }

        //---------------------------------------------------------------------------
        public static Func<string, byte[]> FileReader;

        //---------------------------------------------------------------------------
        public static Func<Type, LuaFunction, Delegate> CreateDelegateHandler = null;
        #endregion

        #region Public Methods
        //---------------------------------------------------------------------------
        public static void Prepare()
        {
            loader = new LuaLoader();
            loader.beZip = GameConst.LuaBundleMode;
            FFLuaState = new FFLuaState();

            InitLuaPath();
            InitLuaBundle();

            if (!string.IsNullOrEmpty(ZbsDebuggerAddress))
            {
                OpenZbsDebugger(ZbsDebuggerAddress);
            }

            LuaInterface.Debugger.useLog = false;
            LuaInterface.Debugger.logger = new LuaLogger();

            FFLuaState.LuaSetTop(0);
        }

        //---------------------------------------------------------------------------
        public static void Initialize()
        {
            FFLuaState.Start();
            SupportClass();
        }

        public static void Write(string filename, string content)
        {
            StreamWriter sw = new StreamWriter(filename);
            sw.Write(content);
            sw.Flush();
            sw.Dispose();
            sw.Close();
        }

        //---------------------------------------------------------------------------
        public static void Shutdown()
        {
            if (FFLuaState != null)
            {
                FFLuaState.Dispose();
                FFLuaState = null;
            }
        }

        //---------------------------------------------------------------------------
        public static bool Load(string path)
        {
            FFLuaState.DoFile(path);

            return true;
        }

        //---------------------------------------------------------------------------
        public static void Execute(string code)
        {
            FFLuaState.DoString(code);
        }

        //---------------------------------------------------------------------------
        public static LuaFunction GetFunction(string name)
        {
            return FFLuaState.GetFunction(name);
        }

        //---------------------------------------------------------------------------
        public static object Call(string name, params object[] args)
        {
            return Call(FFLuaState.GetFunction(name), args);
        }

        //---------------------------------------------------------------------------
        public static T Call<T>(string name, params object[] args)
        {
            return (T)Call(FFLuaState.GetFunction(name), args);
        }

        //---------------------------------------------------------------------------
        public static object Call(LuaTable table, string name, params object[] args)
        {
            return Call(table.GetLuaFunction(name), args);
        }

        //---------------------------------------------------------------------------
        public static T Call<T>(LuaTable table, string name, params object[] args)
        {
            return (T)Call(table.GetLuaFunction(name), args);
        }

        //---------------------------------------------------------------------------
        public static Dictionary<string, object> GetDictionary(string name)
        {
            LuaTable table = FFLuaState.GetTable(name);
            if (table == null)
            {
                return null;
            }

            LuaDictTable dictTable = table.ToDictTable();
            if (dictTable == null)
            {
                return null;
            }

            Dictionary<string, object> ret = new Dictionary<string, object>();
            foreach (DictionaryEntry item in dictTable)
            {
                ret.Add((string)item.Key, item.Value);
            }

            return ret;
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// 初始化Lua代码加载路径
        /// </summary>
        static void InitLuaPath()
        {
            if (!GameConst.LuaBundleMode)
            {
                string rootPath = GameConst.FrameworkRoot;
                lua.AddSearchPath(rootPath + "/Lua");
                lua.AddSearchPath(rootPath + "/ToLua/Lua");
            }
            else
            {
                Debug.LogError("streamingPath:" + GameConst.StreamingPath + "lua");
                Debug.LogError("rwPath:" + GameConst.RWPath + "lua");
//#if UNITY_STANDALONE
                lua.AddSearchPath(GameConst.StreamingPath + "lua");
//#endif
                lua.AddSearchPath(GameConst.RWPath + "lua");
            }
        }

        /// <summary>
        /// 初始化LuaBundle
        /// </summary>
        static void InitLuaBundle()
        {
            if (loader.beZip)
            {
                ResourceManager.Instance.FileForeach("lua", (file) =>
                {
                    loader.AddBundle("lua/" + file);
                });
            }
        }

        //---------------------------------------------------------------------------
        private static object Call(LuaFunction func, object[] args)
        {
            if (func == null)
            {
                return null;
            }

            object[] rets = func.Call(args);
            if (rets == null || rets.Length == 0)
            {
                return null;
            }

            if (rets.Length == 1)
            {
                return rets[0];
            }

            return rets;
        }

        //---------------------------------------------------------------------------
        private static void SupportClass()
        {
            string doString =
                @"local _class = {}

                function class(super)
                    local class_type = {}
                    class_type.ctor = false
                    class_type.super = super
                    class_type.New = function(...) 
                            local obj = {}
                            setmetatable(obj,{ __index = _class[class_type] })
                            do
                                local create
                                create  =  function(c,...)
                                    if c.super then
                                        create(c.super,...)
                                    end
                                    if c.ctor then
                                        c.ctor(obj,...)
                                    end
                                end
 
                                create(class_type,...)
                            end
                            return obj
                        end
                    local vtbl = {}
                    _class[class_type] = vtbl
 
                    setmetatable(class_type,{__newindex = 
                        function(t,k,v)
                            vtbl[k] = v
                        end
                    })
 
                    if super then
                        setmetatable(vtbl,{__index = 
                            function(t,k)
                                local ret = _class[super][k]
                                vtbl[k] = ret
                                return ret
                            end
                        })
                    end
 
                    return class_type
                end";

            FFLuaState.DoString(doString);
        }

        //---------------------------------------------------------------------------
        private static void OpenLuaSocket()
        {
            lua.BeginPreLoad();
            lua.RegFunction("socket.core", LuaOpen_Socket_Core);
            lua.RegFunction("mime.core", LuaOpen_Mime_Core);
            lua.EndPreLoad();
        }

        //---------------------------------------------------------------------------
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        private static int LuaOpen_Socket_Core(IntPtr L)
        {
            return LuaDLL.luaopen_socket_core(L);
        }

        //---------------------------------------------------------------------------
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        private static int LuaOpen_Mime_Core(IntPtr L)
        {
            return LuaDLL.luaopen_mime_core(L);
        }

        //---------------------------------------------------------------------------
        private static void OpenZbsDebugger(string ip)
        {
#if UNITY_EDITOR
            string zbsDir = Environment.GetEnvironmentVariable("ZBS_PATH");
            if (string.IsNullOrEmpty(zbsDir))
            {
                Debug.LogWarning("Environment variable 'ZBS_PATH' not found, " +
                    "The zbs debugger is disabled.");
                return;
            }

            FFLuaState.OpenLibs(LuaDLL.luaopen_pb);
            FFLuaState.OpenLibs(LuaDLL.luaopen_struct);
            FFLuaState.OpenLibs(LuaDLL.luaopen_lpeg);

#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
            FFLuaState.OpenLibs(LuaDLL.luaopen_bit);
#endif
            OpenLuaSocket();
            string lualibsDir = zbsDir.Replace('\\', '/') + "/lualibs";
            FFLuaState.AddSearchPath(lualibsDir);
            string mobdebugDir = zbsDir.Replace('\\', '/') + "/lualibs/mobdebug";
            FFLuaState.AddSearchPath(mobdebugDir);

            lua.LuaDoString(string.Format("DebugServerIp = '{0}'", ip));
#endif
        }
#endregion

#region Internal Fields
        //---------------------------------------------------------------------------
        private static FFLuaState FFLuaState = null;
#endregion

#region Internal Declarations
        //===========================================================================
        private class LuaLogger : LuaInterface.ILogger
        {
            //-----------------------------------------------------------------------------
            public void Log(string msg, string stack, LogType type)
            {
                switch (type)
                {
                    case LogType.Log:
                        Debug.Log(msg);
                        break;
                    case LogType.Warning:
                        Debug.LogWarning(msg);
                        break;
                    case LogType.Error:
                        Debug.LogError(msg);
                        break;
                    case LogType.Exception:
                        Debug.LogException(new System.Exception(msg));
                        break;
                    default:
                        Debug.LogException(new System.Exception(msg));
                        break;
                }
            }
        }
#endregion
    }

}