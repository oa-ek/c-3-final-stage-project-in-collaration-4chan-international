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
        var jwtDevToken = config["Jwt:DevToken"];
        var jwtIssuer = config["Jwt:Issuer"] ?? "YourDarkSoulsAssistantServer";
        var jwtAudience = config["Jwt:Audience"] ?? "YourDarkSoulsAssistantClient";
        
        if (string.IsNullOrWhiteSpace(jwtKey) || jwtKey.Length < 32 ||
            string.IsNullOrWhiteSpace(jwtDevToken))
        {
            throw new InvalidOperationException(
                "❌ Critical Error: Missing or invalid JWT variables\n" +
                "Check:\n" +
                " - Jwt:Key (minimum 32 symbols)\n" +
                " - Jwt:DevToken (for dev)\n");
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
    
    
    public static IApplicationBuilder UseSecretHeaderCheck(this WebApplication app, IConfiguration config)
    {
        var expectedSecret = config["GatewaySecret"];

        return app.Use(async (context, next) =>
        {
            var path = context.Request.Path;
            
            if (path.StartsWithSegments("/openapi") || 
                path.StartsWithSegments("/scalar"))
            {
                await next();
                return;
            }
            
            if (!context.Request.Headers.TryGetValue("X-Gateway-Secret", out var actualSecret) || 
                actualSecret != expectedSecret)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("403 Forbidden: Direct access is not allowed.");
                return;
            }

            await next();
        });
    }
}