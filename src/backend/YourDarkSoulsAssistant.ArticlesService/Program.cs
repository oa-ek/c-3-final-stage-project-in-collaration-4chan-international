using YourDarkSoulsAssistant.ArticlesService.Infrastructure.Context;
using YourDarkSoulsAssistant.ArticlesService.Infrastructure.Init;
using YourDarkSoulsAssistant.ArticlesService.Interfaces.Services;
using YourDarkSoulsAssistant.ArticlesService.Interfaces.Services.External;
using YourDarkSoulsAssistant.ArticlesService.Services;
using YourDarkSoulsAssistant.ArticlesService.Services.External;
using YourDarkSoulsAssistant.Core.Extensions;
using YourDarkSoulsAssistant.Core.Interfaces.Infrastructure.Init;
using YourDarkSoulsAssistant.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRedisDistributedCache("redis");

builder.Services.AddScoped<IDataInitializer, ArticleDbInitializer>();
builder.Services.AddScoped<IYouTubeGuidesService, YouTubeGuidesService>();
builder.Services.AddScoped<IWikiEnrichmentService, WikiEnrichmentService>();
builder.Services.AddBaseWebConfiguration();
builder.Services.AddDatabase<ArticleDBContext>(
    config: builder.Configuration,
    connectionStringName: "ArticleDBConnection");

builder.Services.AddHttpClient<MediaWikiClient>()
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(5),
        PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
        EnableMultipleHttp2Connections = true
    })
    .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

builder.Services.AddHttpClient<EldenRingApiClient>()
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(5),
        PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
        EnableMultipleHttp2Connections = true
    })
    .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

builder.Services.AddOpenApiDocumentationService(
    title: "Your Dark Souls Assistant - Articles API",
    description: "API for managing articles",
    url: "api/articles", 
    urlDescription: "Base URL for Articles API"
);

var app = builder.Build();

app.UseExceptionHandler(opt => { });

app.UseSecretHeaderCheck(builder.Configuration);

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    app.UseDarkSoulsScalar(
        title: "Your Dark Souls Assistant - Articles API (Development)"
        );
}

app.MapControllers();

app.Run();
