using DarkSoulsBuildsAssistant.Core.DTOs.Character;

namespace DarkSoulsBuildsAssistant.Core.Interfaces.Services.Business;

public interface IBuildCalculatorService
{
    // Рахує загальну вагу всіх одягнених предметів
    double CalculateTotalWeight(CharacterBuildDTO build);
    
    // Повертає тип перекату в залежності від ваги
    string GetRollType(double currentWeight, double maxEquipLoad);
}
