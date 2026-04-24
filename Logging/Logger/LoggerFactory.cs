using System;
using Logging.Logger.Enums;
using Logging.Logger.Implementations;

namespace Logging.Logger
{
    public static class LoggerFactory
    {
        public static ILog Create(string logType)
        {
            if (!Enum.TryParse<LogType>(logType, true, out var parsed))
            {
                throw new ArgumentException($"Unsupported logger type: {logType}");
            }

            return Create(parsed);
        }

        public static ILog Create(LogType logType)
        {
            return logType switch
            {
                LogType.Console => new ConsoleLogger(),
                LogType.Safe => new SafeLogger(),
                _ => throw new ArgumentOutOfRangeException(nameof(logType), logType, $"Unsupported logger type: {logType}")
            };
        }
    }
}