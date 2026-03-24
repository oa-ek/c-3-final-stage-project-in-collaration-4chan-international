using DarkSoulsBuildsAssistant.Core.Entities.Equipment.Armor;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment.Weapon;

namespace DarkSoulsBuildsAssistant.Core.Interfaces.Services.Business
{
    public interface ICatalogService
    {
        IEnumerable<WeaponEquipment> GetAllWeapons();
        IEnumerable<WeaponEquipment> FilterWeaponsByType(int typeId);

        IEnumerable<ArmorEquipment> GetAllArmor();
        IEnumerable<ArmorEquipment> GetArmorBySlot(int slotId);
    }
}
