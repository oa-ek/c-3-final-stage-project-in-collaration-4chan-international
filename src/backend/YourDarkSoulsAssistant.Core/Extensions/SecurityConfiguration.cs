using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Yarp.ReverseProxy.Transforms;

namespace YourDarkSoulsAssistant.Core.Extensions;

public static class SecurityConfiguration
{
    public static void AddSecretHeaders(this IServiceCollection services, IConfiguration config)
    {
        var gatewaySecret = config["GatewaySecret"] ?? "SecretNotFound";

        services.AddReverseProxy()
            .LoadFromConfig(config.GetSection("ReverseProxy"))
            .AddServiceDiscoveryDestinationResolver()
            .AddTransforms(context =>
            {
                context.AddRequestHeader("X-Gateway-Secret", gatewaySecret);
            });
    }
    
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var jwtKey = config["Jwt:Key"];
        var jwtIssuer = config["Jwt:Issuer"];
        var jwtAudience = config["Jwt:Audience"];
        
        if (string.IsNullOrWhiteSpace(jwtKey) || 
            jwtKey.Length < 32 ||
            string.IsNullOrWhiteSpace(jwtIssuer) ||
            string.IsNullOrWhiteSpace(jwtAudience))
        {
            throw new InvalidOperationException(
                "❌ Critical Error: Missing or invalid JWT variables\n" +
                "Check:\n" +
                " - Jwt:Key (minimum 32 symbols)\n" +
                " - Jwt:Issuer\n" +
                " - Jwt:Audience");
        }
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
                };
            });
        
        // services.AddAuthorization(options =>
        // {
        //     options.AddPolicy("AppOnly", policy => 
        //         policy.RequireRole("SystemApp", "Admin"));
        // });
    }
    
    public static IApplicationBuilder UseSecretHeaderCheck(this IApplicationBuilder app, IConfiguration config)
    {
        var secretValue = config["Parameters:GatewaySecret"];

        app.Use(async (context, next) =>
        {
            var path = context.Request.Path;
            
            if (path.StartsWithSegments("/health") || 
                path.StartsWithSegments("/alive") ||
                path.StartsWithSegments("/scalar") ||
                path.StartsWithSegments("/openapi"))
            {
                await next(context);
                return;
            }
            
            if (!context.Request.Headers.TryGetValue("X-Gateway-Secret", out var extractedSecret))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsJsonAsync(new { Error = "Відсутній заголовок шлюзу." });
                return;
            }

            if (!string.Equals(extractedSecret, secretValue))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsJsonAsync(new { Error = "Невірний заголовок шлюзу." });
                return;
            }

            await next(context);
        });

        return app;
    }
}
