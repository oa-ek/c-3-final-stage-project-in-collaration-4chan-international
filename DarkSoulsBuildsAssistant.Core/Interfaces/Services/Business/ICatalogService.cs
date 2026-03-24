using DarkSoulsBuildsAssistant.Core.DTOs.Equipment;

namespace DarkSoulsBuildsAssistant.Core.Interfaces.Services.Business;

public interface ICatalogService
{
    Task<IEnumerable<EquipmentDTO>> GetAllWeaponsAsync();
    Task<IEnumerable<EquipmentDTO>> GetAllArmorAsync();
    Task<IEnumerable<EquipmentDTO>> GetEquipmentBySlotAsync(int slotId); // Дуже корисно для випадаючих списків при збірці білду
    
    Task AddEquipmentAsync(EquipmentDTO equipmentDto);
    Task DeleteEquipmentAsync(int id);
}
