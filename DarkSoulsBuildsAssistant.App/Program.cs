var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

// using System.Text;
// using Data.Context;
// using Data.Init;
// using Data.Models;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.AspNetCore.HttpOverrides;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;
// using Microsoft.OpenApi.Models;
// using Scalar.AspNetCore;
// using Server.Middleware;
// using Services.Contracts;
// using Services.Interfaces;
//
// var builder = WebApplication.CreateBuilder(args);
//
// // --- 1. КОНФІГУРАЦІЯ ТА БЕЗПЕКА ---
//
// // Отримуємо рядки підключення
// var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
// var adminConnection = builder.Configuration.GetConnectionString("AdminConnection");
//
// // Отримуємо секретний ключ для JWT
// // .NET сам знайде змінну оточення 'Jwt__Key' і підставить її сюди
// var jwtKey = builder.Configuration["Jwt:Key"];
// var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "LibraryServer";
// var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "LibraryClient";
//
// // "Fail Fast": Якщо ключів немає — не запускаємося
// if (string.IsNullOrEmpty(defaultConnection) || string.IsNullOrEmpty(adminConnection) || string.IsNullOrEmpty(jwtKey))
//     throw new InvalidOperationException(
//         "❌ КРИТИЧНА ПОМИЛКА: Відсутні необхідні змінні оточення!\n" +
//         "Перевірте наявність:\n" +
//         " - ConnectionStrings__DefaultConnection\n" +
//         " - ConnectionStrings__AdminConnection\n" +
//         " - Jwt__Key (мінімум 32 символи)");
//
// // --- 2. СЕРВІСИ ТА БД ---
//
// builder.Services.AddDbContext<LibrarySystemDbContext>(options =>
//     options.UseMySql(defaultConnection, ServerVersion.AutoDetect(defaultConnection)));
//
// builder.Services.AddIdentity<User, Role>(options =>
//     {
//         options.Password.RequireDigit = true;
//         options.Password.RequiredLength = 8;
//     })
//     .AddEntityFrameworkStores<LibrarySystemDbContext>()
//     .AddDefaultTokenProviders();
//
// // Реєстрація наших сервісів
// builder.Services.AddScoped<IAuthService, AuthService>();
// builder.Services.AddScoped<ITokenBlacklistService, TokenBlacklistService>();
// builder.Services.AddScoped<IUserService, UserService>();
// builder.Services.AddScoped<ILibraryService, LibraryService>();
//
// // --- 3. АВТЕНТИФІКАЦІЯ ---
//
// builder.Services.AddAuthentication(options =>
//     {
//         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//     })
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = jwtIssuer,
//             ValidAudience = jwtAudience,
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
//         };
//     });
//
// builder.Services.AddAuthorization();
// builder.Services.AddControllers();
//
// // Налаштування для Cloudflare (Проксі)
// builder.Services.Configure<ForwardedHeadersOptions>(options =>
// {
//     options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
//     options.KnownNetworks.Clear();
//     options.KnownProxies.Clear();
// });
//
// // Налаштування CORS
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll", policy => { policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
// });
//
// // --- 4. OPENAPI (SCALAR) ---
//
// builder.Services.AddOpenApi("v1", options =>
// {
//     options.AddDocumentTransformer((document, context, cancellationToken) =>
//     {
//         // Налаштування домену сервера (для Scalar через тунель)
//         document.Servers = new List<OpenApiServer>
//         {
//             new() { Url = "https://app-library-system.com" },
//             new() { Url = "http://localhost:5118" }
//         };
//
//         // Налаштування JWT авторизації в документації
//         var securityScheme = new OpenApiSecurityScheme
//         {
//             Name = "Authorization",
//             Type = SecuritySchemeType.Http,
//             Scheme = "bearer",
//             BearerFormat = "JWT",
//             In = ParameterLocation.Header,
//             Description = "Введіть токен"
//         };
//
//         document.Components ??= new OpenApiComponents();
//         if (!document.Components.SecuritySchemes.ContainsKey("Bearer"))
//             document.Components.SecuritySchemes["Bearer"] = securityScheme;
//
//         var requirement = new OpenApiSecurityRequirement
//         {
//             {
//                 new OpenApiSecurityScheme
//                 {
//                     Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
//                 },
//                 Array.Empty<string>()
//             }
//         };
//
//         document.SecurityRequirements.Add(requirement);
//         return Task.CompletedTask;
//     });
// });
//
// var app = builder.Build();
//
// // --- 5. PIPELINE ---
//
// app.UseForwardedHeaders(); // Першим!
// app.UseCors("AllowAll");
//
// // Ініціалізація майстер-даних
// if (!string.IsNullOrEmpty(adminConnection)) await MasterDbInitializer.RunAsync(adminConnection);
//
// app.UseAuthentication();
// app.UseMiddleware<JwtBlacklistMiddleware>();
// app.UseAuthorization();
//
// // Документація (доступна завжди)
// app.MapOpenApi(); // Генерує /openapi/v1.json
// app.MapScalarApiReference(options =>
// {
//     options
//         .WithTitle("Library API")
//         .WithTheme(ScalarTheme.Mars)
//         .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
//         .WithOpenApiRoutePattern("/openapi/v1.json"); // Явно вказуємо шлях
// });
//
// app.UseHttpsRedirection();
// app.MapControllers();
//
// app.Run();