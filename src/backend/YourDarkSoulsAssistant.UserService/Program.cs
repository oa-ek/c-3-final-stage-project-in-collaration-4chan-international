using YourDarkSoulsAssistant.ServiceDefaults;
using YourDarkSoulsAssistant.UserService.Extensions;
using YourDarkSoulsAssistant.UserService.Middleware;
using YourDarkSoulsAssistant.UserService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDatabaseAndIdentity(builder.Configuration);
builder.Services.AppServicesRegistration();
builder.Services.AddWebConfiguration();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddOpenApiDocumentation();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

await app.InitializeDatabaseAsync();
app.UseSecretHeaderCheck(builder.Configuration);

app.UseAuthentication();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseMiddleware<JWTBlacklistMiddleware>();
app.UseAuthorization();

app.MapDefaultEndpoints();

app.MapOpenApi();

if (app.Environment.IsDevelopment())
    app.UseDarkSoulsScalar(builder.Configuration);

app.MapControllers();

app.Run();
