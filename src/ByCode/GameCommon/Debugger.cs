using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameCommon
{
    public class Debugger
    {
        public static bool EnableLog = true;

        public static bool EnableLogWarning = true;

        public static bool EnableLogError = true;

        public static void Log(string log, params object[] objs)
        {
            if (EnableLog)
                UnityEngine.Debug.Log(string.Format("[" + System.DateTime.Now.ToString() + "] " + log, objs));
        }

        public static void LogWarning(string log, params object[] objs)
        {
            if (EnableLogWarning)
                UnityEngine.Debug.LogWarning(string.Format("[" + System.DateTime.Now.ToString() + "] " + log, objs));
        }

        public static void LogError(string log, params object[] objs)
        {
            if (EnableLogError)
                UnityEngine.Debug.LogError(string.Format("[" + System.DateTime.Now.ToString() + "] " + log, objs));
        }
    }
}
