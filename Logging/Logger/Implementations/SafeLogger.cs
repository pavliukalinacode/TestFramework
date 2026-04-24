using Logging.Logger.Enums;

namespace Logging.Logger.Implementations
{
    /// <summary>
    /// Logger used in environments where logging must be suppressed (e.g., CI/CD).
    /// This implementation intentionally performs no logging to avoid accidental exposure of sensitive data (request/response payloads, tokens, etc.).
    /// </summary>
    public class SafeLogger : ILog
    {
        public void Log(string message, LogLevel level = LogLevel.Info)
        {
            // Intentionally does nothing with potential to add some strict logging
        }
    }
}