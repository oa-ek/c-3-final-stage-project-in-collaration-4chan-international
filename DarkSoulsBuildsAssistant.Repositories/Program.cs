using DarkSoulsBuildsAssistant.Core.Interfaces.Repositories;
using DarkSoulsBuildsAssistant.Core.Interfaces.Repositories.Character;
using DarkSoulsBuildsAssistant.Core.Interfaces.Repositories.Equipment;
using DarkSoulsBuildsAssistant.Repositories.Character;
using DarkSoulsBuildsAssistant.Repositories.Equipment;
using Microsoft.Extensions.DependencyInjection;

namespace DarkSoulsBuildsAssistant.Repositories;

public static class Program
{
        public static void RepositoriesRegistration(this IServiceCollection services)
        {
            services
                .AddScoped<ICharacterBuildRepository, CharacterBuildRepository>()
                .AddScoped<IEquipmentRepository, EquipmentRepository>()
                .AddScoped<IUnitOfWork, UnitOfWork>();
        }
}
