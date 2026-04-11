using System.Text;
using YourDarkSoulsAssistant.App.Services;
using DarkSoulsBuildsAssistant.Core.Entities.System;
using DarkSoulsBuildsAssistant.Core.Mapping;
using YourDarkSoulsAssistant.Infrastructure.Context;
using YourDarkSoulsAssistant.Infrastructure.Init;
using YourDarkSoulsAssistant.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using YourDarkSoulsAssistant.Repositories;

namespace YourDarkSoulsAssistant.App.Extensions;

public static class ProgramExtensions
{
    public static void ValidateEnvironmentVariables(this WebApplicationBuilder builder)
    {
        var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
        var adminConnection = builder.Configuration.GetConnectionString("AdminConnection");
        var jwtKey = builder.Configuration["Jwt:Key"];
        var apiKey = builder.Configuration["App:ApiKey"]; 

        if (string.IsNullOrEmpty(defaultConnection) || 
            string.IsNullOrEmpty(adminConnection) || 
            string.IsNullOrEmpty(jwtKey) ||
            string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException(
                "❌ КРИТИЧНА ПОМИЛКА: Відсутні необхідні змінні оточення!\n" +
                "Перевірте наявність:\n" +
                " - ConnectionStrings__DefaultConnection\n" +
                " - ConnectionStrings__AdminConnection\n" +
                " - Jwt__Key (мінімум 32 символи)\n" +
                " - App__ApiKey (секретний ключ для додатків)");
        }
    }
    
    public static void AddDatabaseAndIdentity(this IServiceCollection services, IConfiguration config)
    {
        var defaultConnection = config.GetConnectionString("DefaultConnection");

        services.AddDbContext<BuildsAssistantDbContext>(options =>
            options.UseMySQL(defaultConnection!));

        // Твої нові, суворі правила безпеки
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
        .AddEntityFrameworkStores<BuildsAssistantDbContext>()
        .AddDefaultTokenProviders();
    }
    
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AppServicesRegistration();
        services.RepositoriesRegistration();
        services.ServicesRegistration();
        services.AddAutoMapper(config => { config.AddProfile<EquipmentMappingProfile>(); });
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
    
    public static void AddWebConfiguration(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddRazorPages();

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownIPNetworks.Clear();
            options.KnownProxies.Clear();
        });
        
        var frontendUrl = config["App:FrontendUrl"];

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
        services.AddEndpointsApiExplorer(); 
        
        services.AddOpenApi("v1", options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info.Title = "Dark Souls Builds Assistant API";
                document.Info.Version = "v1";
                document.Info.Description = "API for managing Dark Souls character builds";

                document.Servers = new List<OpenApiServer>
                {
                    new() { Url = "https://app-library-system.com" },
                    new() { Url = "http://localhost:5201" }
                };

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Введіть JWT токен"
                };

                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes?.TryAdd("Bearer", securityScheme);

                var requirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        Array.Empty<string>()
                    }
                };

                document.SecurityRequirements?.Add(requirement);
                return Task.CompletedTask;
            });
        });
    }
    
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        var adminConnection = app.Configuration.GetConnectionString("AdminConnection");
        if (!string.IsNullOrEmpty(adminConnection))
        {
            await MasterDbInitializer.RunAsync(adminConnection);
        }
    }
}
