using System.Text;
using FernandoLibrary.Presentation.Common;
using FernandoLibrary.Presentation.Common.ValidatePermissions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using vortexUserConfig.UsersConfig.Presentation.Common.ListQuery;
using vortexUserConfig.UsersConfig.Presentation.Common.ValidatePermissions;
using vortexUserConfig.UsersConfig.Presentation.Services.JwtConfig;

namespace FernandoLibrary;


public static class ServiceContainer 
{
    public static IServiceCollection AddVortexUserConfig(this IServiceCollection services, IConfiguration config)
    {

        //Dependency Injection
        {
            services.AddScoped<JwtToken>();
        }

        services.AddScoped<ListUsersRepository>();
        services.AddScoped<ListRolesRepository>();
        services.AddScoped<ListPermissionRepository>();
        
        services.AddScoped<OneUserRepository>();  
        services.AddScoped<OneRoleRepository>();
        services.AddScoped<OnePermissionRepository>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Bearer", options =>
            {
                var key = Encoding.UTF8.GetBytes(config.GetSection("Authentication:Key").Value!);
                string issuer = config.GetSection("Authentication:Issuer").Value!;
                string audience = config.GetSection("Authentication:Audience").Value!;

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
        
        return services;
    }
}