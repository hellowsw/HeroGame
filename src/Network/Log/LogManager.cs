using System.Collections.Generic;

namespace Network.Log
{
    public enum LogLevels
    {
        DEBUG,
        INFO,
        WARNING,
        ERROR,
    }

    public class LogManager
    {
        static LogManager instance;
        static public LogManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new LogManager();
                return instance;
            }
        }

        byte logSwitches = 0;
        public void SwitchLog(LogLevels level, bool enable)
        {
            if (enable)
                logSwitches = (byte)(logSwitches | (0x1 << (int)level));
            else
                logSwitches = (byte)(logSwitches & (~(0x1 << (int)level)));
        }

        public bool IsLogEnable(LogLevels level)
        {
            return (logSwitches & (0x1 << (int)level)) != 0;
        }

        List<string> infoLogs = new List<string>();
        List<string> debugLogs = new List<string>();
        List<string> warningLogs = new List<string>();
        List<string> errorLogs = new List<string>();

        public void LogInfo(string strFormat, params object[] parameters)
        {
            if (!IsLogEnable(LogLevels.INFO))
                return;
            lock (infoLogs)
            {
                infoLogs.Add(string.Format(strFormat, parameters));
            }
        }

        public void LogDebug(string strFormat, params object[] parameters)
        {
            if (!IsLogEnable(LogLevels.DEBUG))
                return;
            lock (debugLogs)
            {
                debugLogs.Add(string.Format(strFormat, parameters));
            }
        }

        public void LogWarning(string strFormat, params object[] parameters)
        {
            if (!IsLogEnable(LogLevels.WARNING))
                return;
            lock (warningLogs)
            {
                warningLogs.Add(string.Format(strFormat, parameters));
            }
        }

        public void LogError(string strFormat, params object[] parameters)
        {
            if (!IsLogEnable(LogLevels.ERROR))
                return;
            lock (errorLogs)
            {
                errorLogs.Add(string.Format(strFormat, parameters));
            }
        }

        public delegate void LogHandler(string logText);
        public LogHandler InfoLogHandler = null;
        public LogHandler DebugLogHandler = null;
        public LogHandler WarningLogHandler = null;
        public LogHandler ErrorLogHandler = null;
        public void Execute()
        {
            if (InfoLogHandler != null)
            {
                lock (infoLogs)
                {
                    for (int i = 0; i < infoLogs.Count; ++i)
                    {
                        InfoLogHandler(infoLogs[i]);
                    }
                    infoLogs.Clear();
                }
            }

            if (DebugLogHandler != null)
            {
                lock (debugLogs)
                {
                    for (int i = 0; i < debugLogs.Count; ++i)
                    {
                        DebugLogHandler(debugLogs[i]);
                    }
                    debugLogs.Clear();
                }
            }

            if (WarningLogHandler != null)
            {
                lock (warningLogs)
                {
                    for (int i = 0; i < warningLogs.Count; ++i)
                    {
                        WarningLogHandler(warningLogs[i]);
                    }
                    warningLogs.Clear();
                }
            }

            if (ErrorLogHandler != null)
            {
                lock (errorLogs)
                {
                    for (int i = 0; i < errorLogs.Count; ++i)
                    {
                        ErrorLogHandler(errorLogs[i]);
                    }
                    errorLogs.Clear();
                }
            }
        }
    }
}
