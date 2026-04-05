using Tests.Tools.Logger.Enums;

namespace Tests.Tools.Logger
{
    public interface ILog
    {
        void Log(string message, LogLevel level = LogLevel.Info);
    }
}