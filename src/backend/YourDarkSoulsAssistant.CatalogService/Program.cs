using YourDarkSoulsAssistant.Core.Extensions;
using YourDarkSoulsAssistant.CatalogService.Extensions;
using YourDarkSoulsAssistant.CatalogService.Infrastructure.Context;
using YourDarkSoulsAssistant.CatalogService.Infrastructure.Init;
using YourDarkSoulsAssistant.Core.Interfaces.Infrastructure.Init;
using YourDarkSoulsAssistant.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDatabase<CatalogDBContext>(
    config: builder.Configuration,
    connectionStringName: "CatalogDBConnection"
);
builder.Services.AddScoped<IDataInitializer, CatalogDataInitializer>();
builder.Services.AppServicesRegistration();
builder.Services.AddBaseWebConfiguration();
builder.Services.AddOpenApiDocumentationService(
    title: "Your Dark Souls Assistant - Catalog API",
    description: "API for managing catalog items",
    url: "api/catalog",
    urlDescription: "Base URL for the Catalog API");

var app = builder.Build();

app.UseExceptionHandler(opt => { });

app.UseSecretHeaderCheck(builder.Configuration);

app.MapDefaultEndpoints();
app.MapOpenApi();

if (app.Environment.IsDevelopment())
    app.UseDarkSoulsScalar(title: "Your Dark Souls Assistant - Catalog API");

app.MapControllers();

app.Run();
