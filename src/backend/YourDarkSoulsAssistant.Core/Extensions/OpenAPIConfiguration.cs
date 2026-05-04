using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace YourDarkSoulsAssistant.Core.Extensions;

public static class OpenAPIConfiguration
{
    public static void AddOpenApiDocumentationRoot(this IServiceCollection services)
    {
        services.AddOpenApi("users"); 
        services.AddOpenApi("content");
        services.AddOpenApi("gameitems");
        services.AddOpenApi("guides");
    }
    
    public static void AddOpenApiDocumentationService(
        this IServiceCollection services,
        string title, string description, 
        string url, string urlDescription,
        bool addSecurityScheme = false)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info.Title = title;
                document.Info.Version = "v1";
                document.Info.Description = description;
                
                document.Servers = new List<OpenApiServer> {
                    new() { Url = url, Description = urlDescription }
                };
                
                if (addSecurityScheme) AddSecurityScheme(document);
                
                return Task.CompletedTask;
            });
        });
    }
    
    private static void AddSecurityScheme(OpenApiDocument document)
    {
        document.Components ??= new OpenApiComponents();
                
        var schemeName = "Bearer";
        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter JWT Token"
        };
        
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes[schemeName] = securityScheme;
        
        document.Security ??= [];
        document.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference(schemeName, document)] = []
        });
    }
}