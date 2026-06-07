using AutoMapper;
using YourDarkSoulsAssistant.BuildsService.Interfaces;
using YourDarkSoulsAssistant.BuildsService.Mappings;
using YourDarkSoulsAssistant.BuildsService.Services;

namespace YourDarkSoulsAssistant.BuildsService.Extensions;

public static class ProgramExtensions
{
    public static void AppServicesRegistration(this IServiceCollection services)
    {
        services.AddAutoMapper(config => { config.AddProfile<BuildProfile>(); });
        services.AddScoped<IBuildsService, Services.BuildsService>();
    }
}
