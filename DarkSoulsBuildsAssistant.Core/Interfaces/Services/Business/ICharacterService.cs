using DarkSoulsBuildsAssistant.Core.Entities.Character;

namespace DarkSoulsBuildsAssistant.Core.Interfaces.Services.Business
{
    public interface ICharacterService
    {
        IEnumerable<CharacterBuild> GetAllCharacters();
        void SaveCharacter(CharacterBuild character);
    }
}