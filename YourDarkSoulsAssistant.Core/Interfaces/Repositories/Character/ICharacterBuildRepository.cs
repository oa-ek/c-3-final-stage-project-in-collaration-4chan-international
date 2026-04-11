using DarkSoulsBuildsAssistant.Core.DTOs.Character;

namespace DarkSoulsBuildsAssistant.Core.Interfaces.Repositories.Character;

public interface ICharacterBuildRepository : IGenericRepository<CharacterBuildDTO>
{
    // Знаходимо всі білди конкретного користувача
    Task<IEnumerable<CharacterBuildDTO>> GetBuildsByUserIdAsync(int userId);
}
