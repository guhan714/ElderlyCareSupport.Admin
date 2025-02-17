using System.IO.Compression;
using System.Threading.RateLimiting;
using ElderlyCareSupport.Admin.WebApi.Filters;
using ElderlyCareSupport.Admin.WebApi.Middleware;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;

namespace ElderlyCareSupport.Admin.WebApi.Configuration;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfraServiceCollection(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient();
        serviceCollection.AddControllers();
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddResponseCompression(
            options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes;
            });

        serviceCollection.Configure<BrotliCompressionProviderOptions>(option =>
        {
            option.Level = CompressionLevel.Optimal;
        });

        serviceCollection.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("Fixed", opt =>
            {
                opt.Window = TimeSpan.FromMinutes(5);
                opt.PermitLimit = 100;
                opt.QueueLimit = 2;
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });
        });

        serviceCollection.AddHealthChecks();
        
        serviceCollection.AddScoped<MacFilterAttribute>();
        serviceCollection.AddScoped<GlobalErrorHandler>();
        serviceCollection.AddScoped<AuthorizationMiddleware>();
        return serviceCollection;
    }

    public static IApplicationBuilder UseInfra(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalErrorHandler>();
        app.UseMiddleware<AuthorizationMiddleware>();
        app.UseRateLimiter();
        app.UseHealthChecks(new PathString("/health"));
        app.UseResponseCompression();
        return app;
    }
}