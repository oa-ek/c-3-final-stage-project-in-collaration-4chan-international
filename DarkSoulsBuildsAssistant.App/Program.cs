using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using DarkSoulsBuildsAssistant.App.Middleware;
using DarkSoulsBuildsAssistant.App.Services;
using DarkSoulsBuildsAssistant.Core.Entities.System;
using DarkSoulsBuildsAssistant.Core.Interfaces.Services.Identity;
using DarkSoulsBuildsAssistant.Infrastructure.Context;
using DarkSoulsBuildsAssistant.Infrastructure.Init;

using OpenApiInfo = Microsoft.OpenApi.Models.OpenApiInfo;
using OpenApiSecurityRequirement = Microsoft.OpenApi.Models.OpenApiSecurityRequirement;
using OpenApiSecurityScheme = Microsoft.OpenApi.Models.OpenApiSecurityScheme;
using OpenApiReference = Microsoft.OpenApi.Models.OpenApiReference;
using SecuritySchemeType = Microsoft.OpenApi.Models.SecuritySchemeType;
using ParameterLocation = Microsoft.OpenApi.Models.ParameterLocation;
using ReferenceType = Microsoft.OpenApi.Models.ReferenceType;
using OpenApiServer = Microsoft.OpenApi.Models.OpenApiServer;
using OpenApiComponents = Microsoft.OpenApi.Models.OpenApiComponents;


var builder = WebApplication.CreateBuilder(args);

// --- 1. КОНФІГУРАЦІЯ ТА БЕЗПЕКА ---

// Отримуємо рядки підключення
var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
var adminConnection = builder.Configuration.GetConnectionString("AdminConnection");

// Отримуємо секретний ключ для JWT
// .NET сам знайде змінну оточення 'Jwt__Key' і підставить її сюди
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "LibraryServer";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "LibraryClient";

// "Fail Fast": Якщо ключів немає — не запускаємося
if (string.IsNullOrEmpty(defaultConnection) || string.IsNullOrEmpty(adminConnection) || string.IsNullOrEmpty(jwtKey))
    throw new InvalidOperationException(
        "❌ КРИТИЧНА ПОМИЛКА: Відсутні необхідні змінні оточення!\n" +
        "Перевірте наявність:\n" +
        " - ConnectionStrings__DefaultConnection\n" +
        " - ConnectionStrings__AdminConnection\n" +
        " - Jwt__Key (мінімум 32 символи)");

// --- 2. СЕРВІСИ ТА БД ---

builder.Services.AddDbContext<BuildsAssistantDbContext>(options =>
    options.UseMySQL(defaultConnection));

builder.Services.AddIdentity<User, Role>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<BuildsAssistantDbContext>()
    .AddDefaultTokenProviders();

// Реєстрація наших сервісів
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenBlacklistService, TokenBlacklistService>();
builder.Services.AddScoped<IUserService, UserService>();

// --- 3. АВТЕНТИФІКАЦІЯ ---

builder.Services.AddAuthentication(options =>
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Налаштування для Cloudflare (Проксі)
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// Налаштування CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => { policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
});

// --- 4. OPENAPI & SWAGGER ---

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Dark Souls Builds Assistant API",
        Version = "v1",
        Description = "API for managing Dark Souls character builds"
    });

    // Configure JWT authentication for Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token in the format: Bearer {your_token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        // Налаштування домену сервера (для Scalar через тунель)
        document.Servers = new List<OpenApiServer>
        {
            new() { Url = "https://app-library-system.com" },
            new() { Url = "http://localhost:7264" }
        };

        // Налаштування JWT авторизації в документації
        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Введіть токен"
        };

        document.Components ??= new OpenApiComponents();
        if (!document.Components.SecuritySchemes.ContainsKey("Bearer"))
            document.Components.SecuritySchemes["Bearer"] = securityScheme;

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

        document.SecurityRequirements.Add(requirement);
        return Task.CompletedTask;
    });
});

var app = builder.Build();

// --- 5. PIPELINE ---

app.UseForwardedHeaders(); // Першим!
app.UseCors("AllowAll");

// Ініціалізація майстер-даних
if (!string.IsNullOrEmpty(adminConnection)) await MasterDbInitializer.RunAsync(adminConnection);

app.UseAuthentication();
app.UseMiddleware<JWTBlacklistMiddleware>();
app.UseAuthorization();

// Документація (доступна завжди)
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Dark Souls Builds Assistant API v1");
    options.RoutePrefix = "swagger"; // Standard route for Swagger UI
});

app.MapOpenApi(); // Генерує /openapi/v1.json
app.MapScalarApiReference(options =>
{
    options
        .WithTitle("Library API")
        .WithTheme(ScalarTheme.Mars)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
        .WithOpenApiRoutePattern("/openapi/v1.json"); // Явно вказуємо шлях
});

app.UseHttpsRedirection();
app.MapControllers();

app.Run();