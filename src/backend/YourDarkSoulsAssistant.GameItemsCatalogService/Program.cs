using YourDarkSoulsAssistant.GameItemsCatalogService.Extensions;
using YourDarkSoulsAssistant.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddWebConfiguration();
builder.Services.AddHttpClients();
builder.Services.AddOpenApiDocumentation();

var app = builder.Build();

app.UseSecretHeaderCheck(builder.Configuration);

app.MapDefaultEndpoints();
app.MapOpenApi();

if (app.Environment.IsDevelopment())
    app.UseDarkSoulsScalar(builder.Configuration);

app.MapControllers();

app.Run();
