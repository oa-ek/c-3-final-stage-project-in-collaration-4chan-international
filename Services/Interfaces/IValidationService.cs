using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IValidationService
    {
        // Повертає список помилок (наприклад: "Not enough Strength for Claymore")
        List<string> ValidateEquipmentRequirements(int strength, int dex, int intel, int faith, List<int> weaponIds);
    }
}
