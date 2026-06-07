using YourDarkSoulsAssistant.CatalogService.Interfaces.GameItems;
using YourDarkSoulsAssistant.CatalogService.Mappings;
using YourDarkSoulsAssistant.CatalogService.Services;

namespace YourDarkSoulsAssistant.CatalogService.Extensions;

public static class ProgramExtensions
{
    public static void AppServicesRegistration(this IServiceCollection services)
    {
        services
            .AddAutoMapper(config => { config.AddProfile<EquipmentProfile>(); })
            .AddScoped<IEquipmentsService, EquipmentsService>();
    }
}