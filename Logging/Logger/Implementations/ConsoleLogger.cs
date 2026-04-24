using System;
using Logging.Logger.Enums;

namespace Logging.Logger.Implementations
{
    /// <summary>
    /// Logger implementation that writes messages to the console.
    /// Used primarily for local debugging and test execution.
    /// </summary>
    public class ConsoleLogger : ILog
    {
        public void Log(string message, LogLevel level = LogLevel.Info)
        {
            Console.WriteLine($"[{level}] {message}");
        }
    }
}