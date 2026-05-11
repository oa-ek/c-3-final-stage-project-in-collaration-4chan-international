using YourDarkSoulsAssistant.Core.Extensions;
using YourDarkSoulsAssistant.GameItemsCatalogService.Extensions;
using YourDarkSoulsAssistant.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddBaseWebConfiguration();
builder.Services.AddHttpClients();
builder.Services.AddOpenApiDocumentationService(
    title: "Your Dark Souls Assistant - Game Items Catalog API",
    description: "API for managing game items",
    url: "api/game-items",
    urlDescription: "Base URL for the Game Items Catalog API");

var app = builder.Build();

app.UseExceptionHandler(opt => { });

app.UseSecretHeaderCheck(builder.Configuration);

app.MapDefaultEndpoints();
app.MapOpenApi();

if (app.Environment.IsDevelopment())
    app.UseDarkSoulsScalar(title: "Your Dark Souls Assistant - Game Items Catalog API");

app.MapControllers();

app.Run();
