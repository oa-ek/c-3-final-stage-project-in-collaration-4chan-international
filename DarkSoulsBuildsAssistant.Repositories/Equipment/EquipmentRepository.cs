using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using DarkSoulsBuildsAssistant.Core.DTOs.Equipment;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment.Weapon;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment.Armor;
using DarkSoulsBuildsAssistant.Core.Interfaces.Repositories.Equipment;
using DarkSoulsBuildsAssistant.Infrastructure.Context;

namespace DarkSoulsBuildsAssistant.Repositories.Equipment;

public class EquipmentRepository(BuildsAssistantDbContext context, IMapper mapper)
    : GenericRepository<BaseEquipment, EquipmentDTO>(context, mapper), IEquipmentRepository
{
    public async Task<IEnumerable<EquipmentDTO>> GetAllWeaponsAsync()
    {
        // Витягуємо лише зброю
        return await DbContext.Set<BaseEquipment>()
            .OfType<WeaponEquipment>()
            .ProjectTo<EquipmentDTO>(Mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<IEnumerable<EquipmentDTO>> GetAllArmorAsync()
    {
        // Витягуємо лише броню
        return await DbContext.Set<BaseEquipment>()
            .OfType<ArmorEquipment>()
            .ProjectTo<EquipmentDTO>(Mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<IEnumerable<EquipmentDTO>> GetEquipmentBySlotAsync(int slotId)
    {
        // Фільтруємо екіпірування за конкретним слотом (наприклад, тільки шоломи)
        return await DbContext.Set<BaseEquipment>()
            .Where(e => e.EquipmentType.SlotId == slotId)
            .ProjectTo<EquipmentDTO>(Mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
