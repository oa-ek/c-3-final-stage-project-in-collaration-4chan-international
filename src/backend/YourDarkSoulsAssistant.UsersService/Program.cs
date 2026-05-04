using YourDarkSoulsAssistant.ServiceDefaults;
using YourDarkSoulsAssistant.UsersService.Extensions;
using YourDarkSoulsAssistant.UsersService.Middleware;
using YourDarkSoulsAssistant.UsersService.Services;

using YourDarkSoulsAssistant.Core.Extensions;
using YourDarkSoulsAssistant.Core.Interfaces.Infrastructure.Init;
using YourDarkSoulsAssistant.UsersService.Infrastructure.Context;
using YourDarkSoulsAssistant.UsersService.Infrastructure.Init;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddScoped<IDataInitializer, DataInitializer>();
builder.Services.AddDatabase<UserDBContext>(builder.Configuration);
builder.Services.AddIdentity();

builder.Services.AppServicesRegistration();

builder.Services.AddBaseWebConfiguration();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddOpenApiDocumentationService(
    title: "Your Dark Souls Assistant - Users API",
    description: "API for managing users",
    url: "api/users", urlDescription: "Base URL for Users API",
    addSecurityScheme: true);
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseSecretHeaderCheck(builder.Configuration);

app.UseAuthentication();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseMiddleware<JWTBlacklistMiddleware>();
app.UseAuthorization();

app.MapDefaultEndpoints();

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync();
    
    app.UseDarkSoulsScalar(
        title: "Your Dark Souls Assistant - Users API (Development)",
        addSecurityScheme: true);
}


app.MapControllers();

app.Run();
