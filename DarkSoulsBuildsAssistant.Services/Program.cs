using DarkSoulsBuildsAssistant.Core.Interfaces.Services.Business;
using Microsoft.Extensions.DependencyInjection;

namespace DarkSoulsBuildsAssistant.Services;

public static class Program
{
    public static void ServicesRegistration(this IServiceCollection services)
    {
        services
            .AddScoped<IBuildCalculatorService, BuildCalculatorService>()
            .AddScoped<ICharacterBuildService, CharacterBuildService>()
            .AddScoped<ICatalogService, CatalogService>();
    }
}