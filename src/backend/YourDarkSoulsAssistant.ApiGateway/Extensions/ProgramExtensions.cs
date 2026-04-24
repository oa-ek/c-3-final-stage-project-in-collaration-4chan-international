using System.Text;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi;
using Yarp.ReverseProxy.Transforms;

namespace YourDarkSoulsAssistant.ApiGateway.Extensions;

public static class ProgramExtensions
{
    public static void AddWebConfiguration(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownIPNetworks.Clear();
            options.KnownProxies.Clear();
        });
        
        var frontendUrl = config["App:FrontendURL"];

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy => 
            { 
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader(); 
            });
            
            options.AddPolicy("StrictFrontend", policy => 
            { 
                if (!string.IsNullOrEmpty(frontendUrl))
                {
                    policy.WithOrigins(frontendUrl)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                }
            });
        });
    }
    
    public static void AddOpenApiDocumentation(this IServiceCollection services)
    {
        services.AddOpenApi("user"); 
        services.AddOpenApi("guides"); // Розкоментуєш, коли створиш Guides Service
    }

    public static void AddSecretHeaders(this IServiceCollection services, IConfiguration config)
    {
        // Дістаємо секрет зі змінних оточення (або ставимо заглушку, якщо його немає)
        var gatewaySecret = config["GatewaySecret"] ?? "SecretNotFound";

        services.AddReverseProxy()
            .LoadFromConfig(config.GetSection("ReverseProxy"))
            .AddServiceDiscoveryDestinationResolver()
            // ДОДАЄМО ТРАНСФОРМАЦІЮ ЧЕРЕЗ КОД
            .AddTransforms(context =>
            {
                // Це правило автоматично застосується до User Service, Guides Service, CDN тощо!
                context.AddRequestHeader("X-Gateway-Secret", gatewaySecret);
            });
    }
}
