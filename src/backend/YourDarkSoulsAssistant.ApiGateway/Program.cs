using Scalar.AspNetCore;
using YourDarkSoulsAssistant.ApiGateway.Extensions;
using YourDarkSoulsAssistant.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddWebConfiguration(builder.Configuration);
builder.Services.AddOpenApiDocumentation();
builder.Services.AddSecretHeaders(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Dark Souls Assistant - Master API")
            .WithTheme(ScalarTheme.Mars)
            .WithOpenApiRoutePattern("/api/{documentName}/openapi/v1.json")
            .AddDocument("user", "User Service")
            .AddDocument("content", "Content Delivery Service");
    });

    app.MapGet("/", () => Results.Redirect("/scalar"));
    
    app.UseCors("AllowAll");
}
else
{
    app.UseCors("StrictFrontend");
}

app.MapReverseProxy();

app.Run();
