using DarkSoulsBuildsAssistant.Core.DTOs.Equipment;

namespace DarkSoulsBuildsAssistant.Core.Interfaces.Repositories.Equipment;

public interface IEquipmentRepository : IGenericRepository<EquipmentDTO>
{
    // Специфічні методи для нашого "єдиного" репозиторію
    Task<IEnumerable<EquipmentDTO>> GetAllWeaponsAsync();
    Task<IEnumerable<EquipmentDTO>> GetAllArmorAsync();
    Task<IEnumerable<EquipmentDTO>> GetEquipmentBySlotAsync(int slotId);
    
    Task DeleteByIdAsync(int id);
    Task AddWeaponAsync(WeaponDTO weaponDto);
    Task AddArmorAsync(ArmorDTO armorDto);
    Task UpdateArmorAsync(ArmorDTO armorDto);
    Task UpdateWeaponAsync(WeaponDTO weaponDto);
}
