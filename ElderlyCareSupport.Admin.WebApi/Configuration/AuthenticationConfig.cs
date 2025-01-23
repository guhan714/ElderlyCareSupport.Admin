using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ElderlyCareSupport.Admin.WebApi.Configuration;

public static class AuthenticationConfig
{
    public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!)),
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    ClockSkew = TimeSpan.Zero
                };
            });
        return services;
    }


    public static IApplicationBuilder UseAuthenticationConfiguration(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        return app;
    }
}