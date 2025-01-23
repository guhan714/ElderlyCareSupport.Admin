using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace ElderlyCareSupport.Admin.WebApi.Configuration;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(o =>
        {
            o.CustomSchemaIds(id => id.FullName!.Replace("+", "-"));
            var auth = new OpenApiSecurityScheme()
            {
                Name = "JWT Authentication",
                Description = "Enter JWT Token",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT"
            };
            
            o.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, auth);

            var securityRequirement = new OpenApiSecurityRequirement
            {

                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    []
                }
            };
            
            o.AddSecurityRequirement(securityRequirement);
        });

        return services;
    }
}