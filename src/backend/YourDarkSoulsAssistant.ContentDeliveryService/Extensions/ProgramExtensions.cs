using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using YourDarkSoulsAssistant.ContentDeliveryService.Infrastructure.Context;

namespace YourDarkSoulsAssistant.ContentDeliveryService.Extensions;

public static class ProgramExtensions
{
    private static string _defaultConnection = null!;
    
    private static void ValidateEnvironmentVariables(IConfiguration config)
    {
        _defaultConnection = config.GetConnectionString("DefaultConnection") ?? _defaultConnection;

        if (string.IsNullOrEmpty(_defaultConnection))
        {
            throw new InvalidOperationException(
                "❌ Critical Error: There are missing variables\n" +
                "Check:\n" +
                " - ConnectionStrings__DefaultConnection\n");
        }
    }
    
    public static void AddDatabaseAndIdentity(this IServiceCollection services, IConfiguration config)
    {
        ValidateEnvironmentVariables(config);
        
        services.AddDbContext<ContentDeliveryDBContext>(options =>
            options.UseNpgsql(_defaultConnection));
    }

    public static void AddWebConfiguration(this IServiceCollection services)
    {
        services.AddControllers();
    }
    
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        await DataInitializer.InitializeAsync(app.Services);
    }
    
    public static void AddOpenApiDocumentation(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info.Title = "Your Dark Souls Assistant Content Service API";
                document.Info.Version = "v1";
                document.Info.Description = "API for managing content";
                
                document.Servers = new List<OpenApiServer> {
                    new() { Url = "/api/content", Description = "API Gateway Route" }
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
            options.WithTitle("Dark Souls User API")
                .WithTheme(ScalarTheme.Mars)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
                .WithOpenApiRoutePattern("/openapi/v1.json");
        });
        return app;
    }
}
