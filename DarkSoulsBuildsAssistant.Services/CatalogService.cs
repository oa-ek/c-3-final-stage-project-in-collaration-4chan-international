using DarkSoulsBuildsAssistant.Core.DTOs.Equipment;
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

    public async Task DeleteEquipmentAsync(int id)
    {
        // Знаходимо DTO екіпірування за ID
        var equipmentDto = await unitOfWork.Equipment.GetByIdAsync(id);
        
        if (equipmentDto != null)
        {
            // Передаємо знайдений DTO для видалення
            await unitOfWork.Equipment.RemoveAsync(equipmentDto);
            await unitOfWork.CompleteAsync(); // Фіксуємо транзакцію
        }
    }
}
