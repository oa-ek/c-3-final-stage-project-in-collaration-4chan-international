using YourDarkSoulsAssistant.ContentDeliveryService.Infrastructure.Context;
using YourDarkSoulsAssistant.ContentDeliveryService.Infrastructure.Init;
using YourDarkSoulsAssistant.ContentDeliveryService.Services;
using YourDarkSoulsAssistant.Core.Extensions;
using YourDarkSoulsAssistant.Core.Interfaces.Infrastructure.Init;
using YourDarkSoulsAssistant.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddScoped<IDataInitializer, DataInitializer>();
builder.Services.AddDatabase<ContentDeliveryDBContext>(
    builder.Configuration,
    connectionStringName: "ContentDeliveryConnection");
builder.Services.AppServicesRegistration();
builder.Services.AddBaseWebConfiguration();
builder.Services.AddOpenApiDocumentationService(
    title: "Your Dark Souls Assistant - Content Delivery API",
    description: "API for delivering content to the Your Dark Souls Assistant frontend application.",
    url: "api/content",
    urlDescription: "Base URL for the Content Delivery API");

var app = builder.Build();

app.UseSecretHeaderCheck(builder.Configuration);

app.MapDefaultEndpoints();
app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync();
    
    app.UseDarkSoulsScalar("ContentDelivery");
}

app.MapControllers();

app.Run();
