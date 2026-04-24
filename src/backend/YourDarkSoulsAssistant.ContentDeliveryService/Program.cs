using YourDarkSoulsAssistant.ContentDeliveryService.Extensions;
using YourDarkSoulsAssistant.ContentDeliveryService.Services;
using YourDarkSoulsAssistant.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddDatabaseAndIdentity(builder.Configuration);
builder.Services.AppServicesRegistration();
builder.Services.AddWebConfiguration();
builder.Services.AddOpenApiDocumentation();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

await app.InitializeDatabaseAsync();

app.UseSecretHeaderCheck(builder.Configuration);

app.MapDefaultEndpoints();
app.MapOpenApi();

if (app.Environment.IsDevelopment())
    app.UseDarkSoulsScalar(builder.Configuration);

app.MapControllers();

app.Run();
