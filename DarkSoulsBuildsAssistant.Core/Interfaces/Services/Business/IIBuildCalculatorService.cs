using DarkSoulsBuildsAssistant.Core.DTOs.Character;

namespace DarkSoulsBuildsAssistant.Core.Interfaces.Services.Business
{
    public interface IBuildCalculatorService
    {
        CharacterBuildDTO CalculateStats(int vigor, int endurance, int mind, List<int> weaponIds, List<int> armorIds);
    }
}
