using ElderlyCareSupport.Admin.Logging.ILogger;

namespace ElderlyCareSupport.Admin.Logging;

public sealed class LoggingFactory : ILoggerFactory
{
    private readonly Serilog.ILogger _logger;

    public LoggingFactory(Serilog.ILogger logger)
    {
        _logger = logger;
    }

    public void LogInfo(string message)
    {
        _logger.Information(message);
    }

    public void LogWarning(string message)
    {
        _logger.Warning(message);
    }

    public void LogError(string message, Exception exception)
    {
        _logger.Error(exception, message);
    }
}