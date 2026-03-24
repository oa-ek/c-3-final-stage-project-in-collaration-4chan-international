using DarkSoulsBuildsAssistant.Core.Interfaces.Repositories.Equipment;
using DarkSoulsBuildsAssistant.Core.Interfaces.Repositories.Character;

namespace DarkSoulsBuildsAssistant.Core.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IEquipmentRepository Equipment { get; }
    ICharacterBuildRepository CharacterBuilds { get; }
    
    Task<int> CompleteAsync();
}
