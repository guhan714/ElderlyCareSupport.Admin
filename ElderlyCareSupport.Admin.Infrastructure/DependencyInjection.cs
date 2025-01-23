using ElderlyCareSupport.Admin.Application.IRepository;
using ElderlyCareSupport.Admin.Application.IService;
using ElderlyCareSupport.Admin.Application.Service;
using ElderlyCareSupport.Admin.Infrastructure.Persistence;
using ElderlyCareSupport.Admin.Infrastructure.Persistence.RealTime;
using ElderlyCareSupport.Admin.Infrastructure.Persistence.Users;
using ElderlyCareSupport.Admin.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElderlyCareSupport.Admin.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        
        var connectionString = configuration.GetConnectionString("ElderDB");
        services.AddSignalR();
        services.AddScoped<TokenProvider>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IKeycloakAdminRepository, AuthenticationRepository>();
        services.AddScoped<IDbConnectionFactory>(provider => 
            new DbConnectionFactory(connectionString!));
        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseSignalR(a => a.MapHub<UserHub>("/userHub"));
        return app;
    }
}