using Microsoft.OpenApi;
using Scalar.AspNetCore;
using YourDarkSoulsAssistant.GameItemsCatalogService.Services;

namespace YourDarkSoulsAssistant.GameItemsCatalogService.Extensions;

public static class ProgramExtensions
{
    public static void AddWebConfiguration(this IServiceCollection services)
    {
        services.AddControllers();
    }
    
    public static void AddHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient<OutsideGameItemsService>()
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
            {
                ConnectTimeout = TimeSpan.FromMinutes(10),
            })
            .SetHandlerLifetime(Timeout.InfiniteTimeSpan);
    }
    
    public static void AddOpenApiDocumentation(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info.Title = "Your Dark Souls Assistant Game Items Service API";
                document.Info.Version = "v1";
                document.Info.Description = "API for managing game items";
                
                document.Servers = new List<OpenApiServer> {
                    new() { Url = "/api/gameitems", Description = "API Game Items Route" }
                };
                
                return Task.CompletedTask;
            });
        });
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
    
    public static IApplicationBuilder UseDarkSoulsScalar(this WebApplication app, IConfiguration config)
    {
        app.MapScalarApiReference(options =>
        {
            options.WithTitle("Your Dark Souls Assistant Game Items API")
                .WithTheme(ScalarTheme.Mars)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
                .WithOpenApiRoutePattern("/openapi/v1.json");
        });
        return app;
    }
}
