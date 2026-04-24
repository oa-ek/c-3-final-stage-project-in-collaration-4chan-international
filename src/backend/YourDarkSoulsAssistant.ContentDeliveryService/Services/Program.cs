using YourDarkSoulsAssistant.ContentDeliveryService.Interfaces;

namespace YourDarkSoulsAssistant.ContentDeliveryService.Services;

public static class Program
{
    public static void AppServicesRegistration(this IServiceCollection services)
    {
        services
            .AddAutoMapper(config => { config.AddProfile<ContentItemProfile>(); })
            .AddHostedService<GarbageCollectorService>()
            .AddScoped<IRouteService, RouteService>()
            .AddScoped<IContentService, ContentService>();
    }
}