using Logging.Logger.Enums;

namespace Logging.Logger
{
    /// <summary>
    /// Defines a logging abstraction.
    /// Allows interchangeable logging implementations.
    /// </summary>
    public interface ILog
    {
        void Log(string message, LogLevel level = LogLevel.Info);
    }
}