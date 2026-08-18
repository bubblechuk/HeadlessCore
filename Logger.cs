using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessCore
{
    public enum LogLevel
    {
        Debug = 0,
        Info = 1,
        Warning = 2,
        Error = 3,
        None = 4
    }

    public static class CoreLogger
    {
        public static LogLevel MinimumLevel { get; set; } = LogLevel.Info;

        public static event Action<LogLevel, string>? OnLog;

        public static void LogDebug(string message) => Log(LogLevel.Debug, message, ConsoleColor.DarkGray);
        public static void LogInfo(string message) => Log(LogLevel.Info, message, ConsoleColor.Gray);
        public static void LogWarning(string message) => Log(LogLevel.Warning, message, ConsoleColor.Yellow);
        public static void LogError(string message) => Log(LogLevel.Error, message, ConsoleColor.Red);

        public static void Log(LogLevel level, string message, ConsoleColor color = ConsoleColor.White)
        {
            if (level < MinimumLevel || MinimumLevel == LogLevel.None) return;

            string formattedMessage = $"[{DateTime.Now:HH:mm:ss}] [{level.ToString().ToUpper()}] {message}";

            OnLog?.Invoke(level, formattedMessage);

            Console.ForegroundColor = color;
            Console.WriteLine(formattedMessage);
            Console.ResetColor();
        }
    }
}
