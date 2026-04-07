using System;
using Tests.Tools.Logger.Enums;

namespace Tests.Tools.Logger.Implementations
{
    /// <summary>
    /// Logger implementation that writes messages to the console.
    /// Used primarily for local debugging and test execution.
    /// </summary>
    internal class ConsoleLogger : ILog
    {
        public void Log(string message, LogLevel level = LogLevel.Info)
        {
            Console.WriteLine($"[{level}] {message}");
        }
    }
}