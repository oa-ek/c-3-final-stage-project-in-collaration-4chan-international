using YourDarkSoulsAssistant.Core.Extensions;
using YourDarkSoulsAssistant.Core.Interfaces.Infrastructure.Init;
using YourDarkSoulsAssistant.BuildsService.Extensions;
using YourDarkSoulsAssistant.BuildsService.Infrastructure.Context;
using YourDarkSoulsAssistant.BuildsService.Infrastructure.Init;
using YourDarkSoulsAssistant.ServiceDefaults;


var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDatabase<BuildsDbContext>(
    config: builder.Configuration,
    connectionStringName: "BuildsDBConnection"
);
builder.Services.AddScoped<IDataInitializer, BuildsDataInitializer>();
builder.Services.AppServicesRegistration();
builder.Services.AddBaseWebConfiguration();
builder.Services.AddOpenApiDocumentationService(
    title: "Your Dark Souls Assistant - User's Builds API",
    description: "API for managing user builds",
    url: "api/builds",
    urlDescription: "Base URL for the Builds API");

var app = builder.Build();

app.UseExceptionHandler(opt => { });

app.UseSecretHeaderCheck(builder.Configuration);

app.MapDefaultEndpoints();
app.MapOpenApi();

if (app.Environment.IsDevelopment())
    app.UseDarkSoulsScalar(title: "Your Dark Souls Assistant - User's Builds API");

app.MapControllers();

app.Run();
