using DarkSoulsBuildsAssistant.Core.Entities.Armor;

namespace DarkSoulsBuildsAssistant.Core.Interfaces.Repositories.Equipment;

public interface IArmorRepository : IGenericRepository<ArmorEquipment>
{
    // Отримати броню для конкретного слоту (наприклад, тільки шоломи)
    IEnumerable<ArmorEquipment> GetBySlotId(int slotId);

    // Отримати за назвою типу (наприклад, "Heavy Armor")
    IEnumerable<ArmorEquipment> GetByArmorTypeName(string typeName);
}
