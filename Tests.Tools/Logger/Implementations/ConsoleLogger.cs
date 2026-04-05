using System;
using Tests.Tools.Logger.Enums;

namespace Tests.Tools.Logger.Implementations
{
    /// <summary>
    /// Logger used for local execution.
    /// Writes all log messages to the console.
    /// </summary>
    internal class ConsoleLogger : ILog
    {
        public void Log(string message, LogLevel level = LogLevel.Info)
        {
            Console.WriteLine($"[{level}] {message}");
        }
    }
}