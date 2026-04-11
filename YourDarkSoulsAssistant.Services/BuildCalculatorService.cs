using DarkSoulsBuildsAssistant.Core.DTOs.Character;
using DarkSoulsBuildsAssistant.Core.Interfaces.Services.Business;

namespace YourDarkSoulsAssistant.Services;

public class BuildCalculatorService : IBuildCalculatorService
{
    public double CalculateTotalWeight(CharacterBuildDTO build)
    {
        double totalWeight = 0;

        // Тут логіка буде залежати від вашої структури слотів у DTO.
        // Зазвичай це виглядає так: ми проходимося по активному "Набору" (Set)
        // і додаємо вагу (Weight) кожного екіпірованого предмета (EquipmentDTO).
        
        /* Приклад (якщо у Set є колекція Slots):
        if (build.Sets != null && build.Sets.Any())
        {
            var activeSet = build.Sets.First(); // Беремо перший набір для прикладу
            foreach (var slot in activeSet.Slots)
            {
                if (slot.EquippedItem != null)
                {
                    totalWeight += slot.EquippedItem.Weight;
                }
            }
        }
        */

        return totalWeight;
    }

    public string GetRollType(double currentWeight, double maxEquipLoad)
    {
        if (maxEquipLoad <= 0) return "Overencumbered (Перевантаження)";

        double ratio = currentWeight / maxEquipLoad;

        // Логіка Dark Souls 3 / Elden Ring
        if (ratio <= 0.30) return "Fast Roll (Швидкий)";
        if (ratio <= 0.70) return "Mid Roll (Середній)";
        if (ratio <= 1.00) return "Fat Roll (Важкий)";
        
        return "Overencumbered (Перевантаження)";
    }
}
