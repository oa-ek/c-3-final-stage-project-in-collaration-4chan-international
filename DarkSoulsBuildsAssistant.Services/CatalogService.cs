using DarkSoulsBuildsAssistant.Core.DTOs.Equipment;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment.Weapon;
using DarkSoulsBuildsAssistant.Core.Interfaces.Repositories;
using DarkSoulsBuildsAssistant.Core.Interfaces.Services.Business;

namespace DarkSoulsBuildsAssistant.Services;

// Використовуємо Primary Constructor (як у вашому прикладі) для лаконічності
public class CatalogService(IUnitOfWork unitOfWork) : ICatalogService
{
    public async Task<IEnumerable<EquipmentDTO>> GetAllWeaponsAsync()
    {
        // Звертаємося до єдиного репозиторію Equipment і викликаємо його специфічний метод
        return await unitOfWork.Equipment.GetAllWeaponsAsync();
    }

    public async Task<IEnumerable<EquipmentDTO>> GetAllArmorAsync()
    {
        // Звертаємося до єдиного репозиторію Equipment
        return await unitOfWork.Equipment.GetAllArmorAsync();
    }

    public async Task<IEnumerable<EquipmentDTO>> GetEquipmentBySlotAsync(int slotId)
    {
        // Отримуємо екіпірування під конкретний слот (наприклад, тільки кільця або шоломи)
        return await unitOfWork.Equipment.GetEquipmentBySlotAsync(slotId);
    }

    public async Task AddEquipmentAsync(EquipmentDTO equipmentDto)
    {
        // Універсальний метод додавання. Entity Framework сам розбереться, 
        // чи це зброя, чи броня, на основі DTO.
        await unitOfWork.Equipment.AddAsync(equipmentDto);
        await unitOfWork.CompleteAsync(); // Зберігаємо зміни
    }

    public async Task AddWeaponAsync(WeaponDTO weaponDto)
    {
        await unitOfWork.Equipment.AddWeaponAsync(weaponDto);
        await unitOfWork.CompleteAsync(); // Не забудь зберегти!
    }

    public async Task AddArmorAsync(ArmorDTO armorDto)
    {
        await unitOfWork.Equipment.AddArmorAsync(armorDto);
        await unitOfWork.CompleteAsync();
    }

    public async Task UpdateArmorAsync(ArmorDTO armorDto)
    {
        await unitOfWork.Equipment.UpdateArmorAsync(armorDto);
        await unitOfWork.CompleteAsync();
    }

    public async Task UpdateWeaponAsync(WeaponDTO weaponDto)
    {
        await unitOfWork.Equipment.UpdateWeaponAsync(weaponDto);
        await unitOfWork.CompleteAsync();
    }


    // DarkSoulsBuildsAssistant.Services/CatalogService.cs
// Знайдіть метод DeleteEquipmentAsync і замініть його на цей:

    public async Task DeleteEquipmentAsync(int id)
    {
        // Більше не викликаємо GetByIdAsync та RemoveAsync(dto). 
        // Видаляємо напряму!
        await unitOfWork.Equipment.DeleteByIdAsync(id);
        await unitOfWork.CompleteAsync();
    }
}
