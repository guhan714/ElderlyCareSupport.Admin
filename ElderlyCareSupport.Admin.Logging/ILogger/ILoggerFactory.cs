namespace ElderlyCareSupport.Admin.Logging.ILogger;

public interface ILoggerFactory
{
    void LogInfo(string message);
    void LogWarning(string message);
    void LogError(string message, Exception exception);
}