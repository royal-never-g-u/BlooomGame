/**
 * 日志系统
 */

using System;
using System.Reflection;

namespace GameBaseFramework.Base
{
    /// <summary>
    /// Log接口
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message);
        /// <summary>
        /// LogWarning
        /// </summary>
        /// <param name="message"></param>
        public void LogWarning(string message);
        /// <summary>
        /// LogError
        /// </summary>
        /// <param name="message"></param>
        public void LogError(string message);
    }

    /// <summary>
    /// Unity Debug
    /// </summary>
    public class UnityDebugConsole : ILogger
    {
        /// <summary>
        /// 参数
        /// </summary>
        private object[] _args = new[] { "" };
        /// <summary>
        /// UnityEngine.Debug Log反射方法
        /// </summary>
        private MethodInfo _log;
        /// <summary>
        /// UnityEngine.Debug LogWarning反射方法
        /// </summary>
        private MethodInfo _logWarning;
        /// <summary>
        /// UnityEngine.Debug LogError反射方法
        /// </summary>
        private MethodInfo _logError;

        /// <summary>
        /// 构造
        /// </summary>
        public UnityDebugConsole()
        {
            var type = Type.GetType("UnityEngine.Debug, UnityEngine");
            _log = type.GetMethod("Log", new Type[] { typeof(object) });
            _logWarning = type.GetMethod("LogWarning", new Type[] { typeof(object) });
            _logError = type.GetMethod("LogError", new Type[] { typeof(object) });
        }

        /// <summary>
        /// Log
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            _args[0] = message;
            _log.Invoke(null, _args);
        }

        /// <summary>
        /// LogWarning
        /// </summary>
        /// <param name="message"></param>
        public void LogWarning(string message)
        {
            _args[0] = message;
            _logWarning.Invoke(null, _args);
        }

        /// <summary>
        /// LogError
        /// </summary>
        /// <param name="message"></param>
        public void LogError(string message)
        {
            _args[0] = message;
            _logError.Invoke(null, _args);
        }
    }

    /// <summary>
    /// System Debug
    /// </summary>
    public class SystemDebugConsole : ILogger
    {
        /// <summary>
        /// Log
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
#if TESTQUIZ
            Console.ForegroundColor = ConsoleColor.Black;
#else
            Console.ForegroundColor = ConsoleColor.White;
#endif
            Console.WriteLine(message);
        }

        /// <summary>
        /// LogWarning
        /// </summary>
        /// <param name="message"></param>
        public void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
        }

        /// <summary>
        /// LogError
        /// </summary>
        /// <param name="message"></param>
        public void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
        }
    }

    public static class Debuger
    {
        /// <summary>
        /// 控制日志输入
        /// </summary>
        public static bool EnableLog = false;
        /// <summary>
        /// Console
        /// </summary>
        private static ILogger _console;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="console"></param>
        public static void Init(ILogger console)
        {
            _console = console;
        }

        /// <summary>
        /// Log
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            if (EnableLog)
            {
                _console?.Log(message);
            }
        }

        /// <summary>
        /// 格式化输出
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void LogFormat(string message, params object[] args)
        {
            if (message != null)
            {
                Log(string.Format(message, args));
            }
        }

        /// <summary>
        /// LogWarning
        /// </summary>
        /// <param name="message"></param>
        public static void LogWarning(string message, params object[] args)
        {
            if (EnableLog)
            {
                _console?.LogWarning(string.Format(message, args));
            }
        }

        /// <summary>
        /// LogError
        /// </summary>
        /// <param name="message"></param>
        public static void LogError(string message, params object[] args)
        {
            if (EnableLog)
            {
                _console?.LogError(string.Format(message, args));
            }
        }
    }
}
