//#define _OUT_NET_
#define _IN_NET_
//#define _IN_NET_EDITOR_

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace GameCommon
{
    public class GameConst
    {
        public const int STANDARD_WIDTH = 480;
        public const int STANDARD_HEIGHT = 854; 
        public const bool ShowFPS = false;                       //显示FPS


  #if _OUT_NET_
        public const bool DebugMode = false;                     //调试模式-用于内部测试
        public const bool OuterNet = true;                       //外网
        public static int LoginProgram = 1;                      //使用登录器
        public const bool UpdateMode = false;                    //更新模式-默认关闭 
#elif _IN_NET_
        public const bool DebugMode = false;                     //调试模式-用于内部测试
        public const bool OuterNet = false;                       //外网
        public static int LoginProgram = 0;                      //使用登录器
        public const bool UpdateMode = false;                    //更新模式-默认关闭 
#elif _IN_NET_EDITOR_
        public const bool DebugMode = true;                     //调试模式-用于内部测试
        public const bool OuterNet = false;                       //外网
        public static int LoginProgram = 0;                      //使用登录器
        public const bool UpdateMode = false;                    //更新模式-默认关闭 
#endif

        //平台标示
        public const int PlATFORM_FLAG_FLASH = 1;               //Flash
        public const int PlATFORM_FLAG_WINDOWS = 2;             //PC端
        public const int PLATFORM_FLAG_ANDROID = 3;             //Android
        public const int PLATFORM_FLAG_IOS = 4;                 //IOS

        public static bool LuaBundleMode                        //lua使用bundle
        { 
            get
            {
                return !Application.isEditor;
            }
        }                     

        public const string LuaTempDir = "Temp/";               //临时目录
        public const string AppName = "斗鱼达人";               //应用程序名称
        public const string ProgramVersion = "1.3.0";           //程序版本
        public const string AppPrefix = AppName + "_";          //应用程序前缀
        public const string ExtName = "";                       //素材扩展名

#if MOBILE_DEBUG
        public const string DownloadUrl = "http://192.168.1.215/fish/download_android/";      //更新地址
        public const int GameFrameRate = 60;
        public const int Mobile = 1;
        public static int Cur_Platform_Flag = PLATFORM_FLAG_ANDROID;
#elif UNITY_IPHONE
        public const string DownloadUrl = "http://192.168.1.215/fish/download_android/";      //更新地址
        public const int GameFrameRate = 60;
        public const int Mobile = 1;
        public static int Cur_Platform_Flag = PLATFORM_FLAG_IOS;
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
        public const string DownloadUrl = "http://192.168.1.215/fish/download_st/";      //更新地址
        public const int GameFrameRate = 60;
        public const int Mobile = 0;
        public static int Cur_Platform_Flag = PlATFORM_FLAG_WINDOWS;
#elif UNITY_ANDROID
        public const string DownloadUrl = "http://192.168.1.215/fish/download_android/";      //更新地址
        public const int GameFrameRate = 60;
        public const int Mobile = 1;
        public static int Cur_Platform_Flag = PLATFORM_FLAG_ANDROID;
#endif

        public static string RWPath  { get {

#if UNITY_ANDROID
                return string.Format("/data/data/{0}/files/", Application.bundleIdentifier);
#elif UNITY_IPHONE
                return Application.persistentDataPath + "/";
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
                return Application.persistentDataPath + "/";
#endif
            }
        }

        public static string StreamingPath
        {
            get
            {
                return Application.streamingAssetsPath + "/";
            }
        }

        public static string FrameworkRoot
        {
            get
            {
                return Application.dataPath + "/" + AppName;
            }
        }
    }
}
