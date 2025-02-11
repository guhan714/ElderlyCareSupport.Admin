using System.IO.Compression;
using ElderlyCareSupport.Admin.WebApi.Filters;
using ElderlyCareSupport.Admin.WebApi.Middleware;
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
        serviceCollection.AddScoped<MacFilterAttribute>();
        serviceCollection.AddScoped<GlobalErrorHandler>();
        serviceCollection.AddScoped<AuthorizationMiddleware>();
        return serviceCollection;
    }

    public static IApplicationBuilder UseInfra(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalErrorHandler>();
        app.UseMiddleware<AuthorizationMiddleware>();
        app.UseResponseCompression();
        return app;
    }
}