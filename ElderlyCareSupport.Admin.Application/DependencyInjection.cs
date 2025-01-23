using ElderlyCareSupport.Admin.Application.IService;
using ElderlyCareSupport.Admin.Application.Service;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ElderlyCareSupport.Admin.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddScoped<IKeycloakAdminService, AdminAuthentication>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IResponseCreator>(provider => 
            new ResponseFactory());
        
        return services;
    }
}

