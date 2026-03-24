using DarkSoulsBuildsAssistant.Core.Entities.Equipment.Weapon;

namespace DarkSoulsBuildsAssistant.Core.Interfaces.Repositories.Equipment;

public interface IWeaponRepository : IGenericRepository<WeaponEquipment>
{
    IEnumerable<WeaponEquipment> GetByWeaponTypeId(int typeId);

    // Знайти зброю за назвою типу (наприклад, "Dagger")
    IEnumerable<WeaponEquipment> GetByWeaponTypeName(string typeName);
}
