using DarkSoulsBuildsAssistant.Core.Entities.Character;

namespace DarkSoulsBuildsAssistant.Core.Interfaces.Services.Business
{
    public interface ICharacterBuildService
    {
        IEnumerable<CharacterBuild> GetAllBuilds();
        void SaveBuild(CharacterBuild build);
        void DeleteBuild(int id);
    }
}