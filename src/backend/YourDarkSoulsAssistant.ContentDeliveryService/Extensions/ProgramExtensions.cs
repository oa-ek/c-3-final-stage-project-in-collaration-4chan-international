using YourDarkSoulsAssistant.ContentDeliveryService.Interfaces;
using YourDarkSoulsAssistant.ContentDeliveryService.Services;

namespace YourDarkSoulsAssistant.ContentDeliveryService.Extensions;

public static class ProgramExtensions
{
    public static void CheckStoragePathVariable(this IServiceCollection services, IConfiguration config)
    {
        var storagePath = config["App:StoragePath"];
        
        if (string.IsNullOrWhiteSpace(storagePath))
        {
            throw new InvalidOperationException(
                "❌ Critical Error: Missing or invalid Path variable\n" +
                "Check:\n" +
                " - App:StoragePath");
        }
    }
    
    public static void AppServicesRegistration(this IServiceCollection services)
    {
        services
            .AddAutoMapper(config => { config.AddProfile<ContentItemProfile>(); })
            .AddHostedService<GarbageCollectorService>()
            .AddScoped<IRouteService, RouteService>()
            .AddScoped<IContentService, ContentService>();
    }
}
