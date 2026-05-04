using YourDarkSoulsAssistant.Core.Extensions;
using YourDarkSoulsAssistant.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddGatewayConfiguration(builder.Configuration);
builder.Services.AddOpenApiDocumentationRoot();
builder.Services.AddSecretHeaders(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    app.UseScalarApiReferences();

    app.MapGet("/", () => Results.Redirect("/scalar"));
    
    app.UseCors("AllowAll");
}
else
{
    app.UseCors("StrictFrontend");
}

app.MapReverseProxy();

app.Run();
