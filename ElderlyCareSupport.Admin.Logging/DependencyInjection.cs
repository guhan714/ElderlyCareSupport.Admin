using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ILoggerFactory = ElderlyCareSupport.Admin.Logging.ILogger.ILoggerFactory;

namespace ElderlyCareSupport.Admin.Logging;

public static class DependencyInjection
{
    public static IServiceCollection AddLoggingFactory(this IServiceCollection services, IConfiguration configuration)
    {
        
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(dispose: true);
        });
        services.AddScoped<ILoggerFactory, LoggingFactory>();
        services.AddScoped(_ => Log.Logger);
        return services;
    }
}