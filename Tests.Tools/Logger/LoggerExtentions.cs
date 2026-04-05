using System;
using Tests.Tools.Logger.Enums;

namespace Tests.Tools.Logger
{
    public static class LoggerExtensions
    {
        public static void Debug(this ILog logger, string message)
        {
            logger.Log(message, LogLevel.Debug);
        }

        public static void Info(this ILog logger, string message)
        {
            logger.Log(message, LogLevel.Info);
        }

        public static void Warning(this ILog logger, string message)
        {
            logger.Log(message, LogLevel.Warning);
        }

        public static void Error(this ILog logger, string message)
        {
            logger.Log(message, LogLevel.Error);
        }

        public static void Error(this ILog logger, Exception exception, string? message = null)
        {
            var fullMessage = string.IsNullOrWhiteSpace(message)
                ? exception.Message
                : $"{message}. Exception: {exception.Message}";

            logger.Log(fullMessage, LogLevel.Error);
        }
    }
}