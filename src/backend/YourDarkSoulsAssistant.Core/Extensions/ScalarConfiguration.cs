using Microsoft.AspNetCore.Builder;
using Scalar.AspNetCore;

namespace YourDarkSoulsAssistant.Core.Extensions;

public static class ScalarConfiguration
{
    public static void UseScalarApiReferences(this WebApplication app)
    {
        app.MapScalarApiReference(options =>
        {
            options.WithTitle("Dark Souls Assistant - Master API")
                .WithTheme(ScalarTheme.Mars)
                .WithOpenApiRoutePattern("/api/{documentName}/openapi/v1.json")
                .AddDocument("articles", "Articles Service")
                .AddDocument("users", "User Service")
                .AddDocument("content", "Content Delivery Service")
                .AddDocument("catalog", "Catalog Service")
                .AddDocument("guides", "Guides Management Service")
                .AddDocument("builds", "Builds Management Service");
        });
    }
    
    public static IApplicationBuilder UseDarkSoulsScalar(
        this WebApplication app, 
        string title,
        bool addSecurityScheme = false)
    {
        var devToken = app.Configuration["Jwt:DevToken"];
    
        app.MapScalarApiReference(options =>
        {
            options.WithTitle(title)
                .WithTheme(ScalarTheme.Mars)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
                .WithOpenApiRoutePattern("/openapi/v1.json");
            
            if (addSecurityScheme)
            {
                options.AddPreferredSecuritySchemes("Bearer");
            
                if (!string.IsNullOrEmpty(devToken))
                {
                    options.AddHttpAuthentication("Bearer", auth =>
                    {
                        auth.Token = devToken;
                        auth.Description = "Enter your bearer token";
                    });
                }
            }
        });
    
        return app;
    }
}
