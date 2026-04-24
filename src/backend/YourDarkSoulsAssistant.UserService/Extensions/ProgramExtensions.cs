using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using YourDarkSoulsAssistant.UserService.Infrastructure.Context;
using YourDarkSoulsAssistant.UserService.Infrastructure.Init;
using YourDarkSoulsAssistant.UserService.Models;

namespace YourDarkSoulsAssistant.UserService.Extensions;

public static class ProgramExtensions
{
    private static string _defaultConnection = null!;
    private static string _jwtKey = null!;
    private static string _jwtDevToken = null!;
    
    private static void ValidateEnvironmentVariables(IConfiguration config)
    {
        _defaultConnection = config.GetConnectionString("DefaultConnection") ?? _defaultConnection;
        _jwtKey = config["Jwt:Key"] ?? _jwtKey;
        _jwtDevToken = config["Jwt:DevToken"] ?? _jwtDevToken;

        if (string.IsNullOrEmpty(_defaultConnection) || 
            string.IsNullOrEmpty(_jwtKey) ||
            string.IsNullOrEmpty(_jwtDevToken))
        {
            throw new InvalidOperationException(
                "❌ Critical Error: There are missing variables\n" +
                "Check:\n" +
                " - ConnectionStrings__DefaultConnection\n" +
                " - Jwt__Key (minimum 32 symbols)\n" +
                " - Jwt__DevToken (for dev)\n");
        }
    }
    
    public static void AddDatabaseAndIdentity(this IServiceCollection services, IConfiguration config)
    {
        ValidateEnvironmentVariables(config);
        
        services.AddDbContext<UserDBContext>(options =>
            options.UseNpgsql(_defaultConnection));
        
        services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 12;
            
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._+";
            
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<UserDBContext>()
            .AddDefaultTokenProviders();
    }

    public static void AddWebConfiguration(this IServiceCollection services)
    {
        services.AddControllers();
    }

    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var jwtKey = config["Jwt:Key"];
        var jwtIssuer = config["Jwt:Issuer"] ?? "YourDarkSoulsAssistantServer";
        var jwtAudience = config["Jwt:Audience"] ?? "YourDarkSoulsAssistantClient";

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
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AppOnly", policy => 
                policy.RequireRole("SystemApp", "Admin"));
        });
    }
    
    public static void AddOpenApiDocumentation(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info.Title = "Your Dark Souls Assistant UserService API";
                document.Info.Version = "v1";
                document.Info.Description = "API for managing users";
                
                document.Servers = new List<OpenApiServer> {
                    new() { Url = "/api/user", Description = "API Gateway Route" }
                };
                
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
                
                document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
                document.Components.SecuritySchemes[schemeName] = securityScheme;
                
                document.Security ??= [];
                document.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference(schemeName, document)] = []
                });
                
                return Task.CompletedTask;
            });
        });
    }
    
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        await DataInitializer.InitializeAsync(app.Services);
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
        var devToken = config["Jwt:DevToken"];
        
        app.MapScalarApiReference(options =>
        {
            options.WithTitle("Dark Souls User API")
                .WithTheme(ScalarTheme.Mars)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
                .WithOpenApiRoutePattern("/openapi/v1.json")
                .AddPreferredSecuritySchemes("Bearer");
            
            if (!string.IsNullOrEmpty(devToken))
            {
                options.AddHttpAuthentication("BearerAuth", auth =>
                {
                    auth.Token = devToken;
                    auth.Description = "Enter your bearer token";
                });
            }
        });
        return app;
    }
}
