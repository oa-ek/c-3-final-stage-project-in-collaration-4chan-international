using DarkSoulsBuildsAssistant.Core.Interfaces.Repositories;
using DarkSoulsBuildsAssistant.Core.Interfaces.Repositories.Character;
using DarkSoulsBuildsAssistant.Core.Interfaces.Repositories.Equipment;
using DarkSoulsBuildsAssistant.Infrastructure.Context;

namespace DarkSoulsBuildsAssistant.Repositories;

public class UnitOfWork(
    BuildsAssistantDbContext context,
    IEquipmentRepository equipment,
    ICharacterBuildRepository characterBuilds)
    : IUnitOfWork
{
    public IEquipmentRepository Equipment { get; } = equipment;
    public ICharacterBuildRepository CharacterBuilds { get; } = characterBuilds;

    public async Task<int> CompleteAsync()
    {
        return await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}