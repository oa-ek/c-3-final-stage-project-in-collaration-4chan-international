using DarkSoulsBuildsAssistant.Core.DTOs.Character;

namespace DarkSoulsBuildsAssistant.Core.Interfaces.Services.Business;

public interface ICharacterBuildService
{
    Task<IEnumerable<CharacterBuildDTO>> GetAllBuildsAsync();
    Task<CharacterBuildDTO?> GetBuildByIdAsync(int id);
    Task SaveBuildAsync(CharacterBuildDTO buildDto);
    Task DeleteBuildAsync(int id);
}