using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using DarkSoulsBuildsAssistant.Core.DTOs.Equipment;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment.Weapon;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment.Armor;
using DarkSoulsBuildsAssistant.Core.Interfaces.Repositories.Equipment;
using YourDarkSoulsAssistant.Infrastructure.Context;

namespace YourDarkSoulsAssistant.Repositories.Equipment;

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
    
    public async Task DeleteByIdAsync(int id)
    {
        // Знаходимо сутність безпосередньо в базі
        var entity = await DbContext.Set<BaseEquipment>().FindAsync(id);
    
        if (entity != null)
        {
            DbContext.Set<BaseEquipment>().Remove(entity);
        }
    }
    
    public async Task AddWeaponAsync(WeaponDTO weaponDto)
    {
        // 1. Мапимо DTO у сутність WeaponEquipment
        var weaponEntity = Mapper.Map<WeaponEquipment>(weaponDto);

        // 2. Додаємо безпосередньо в DbContext
        await DbContext.Set<BaseEquipment>().AddAsync(weaponEntity);
        
        // Зверни увагу: SaveChangesAsync зазвичай викликається в UnitOfWork,
        // тому тут ми його не пишемо, якщо у тебе так налаштовано.
    }

    public async Task AddArmorAsync(ArmorDTO armorDto)
    {
        // 1. Мапимо DTO у сутність ArmorEquipment
        var armorEntity = Mapper.Map<ArmorEquipment>(armorDto);

        // 2. Додаємо в базу
        await DbContext.Set<BaseEquipment>().AddAsync(armorEntity);
    }
    
    public async Task UpdateArmorAsync(ArmorDTO armorDto)
    {
        // Знаходимо броню разом із її поточними характеристиками захисту
        var entity = await DbContext.Set<BaseEquipment>()
            .OfType<ArmorEquipment>()
            .Include(a => a.ArmorInfluences)
            .FirstOrDefaultAsync(a => a.Id == armorDto.Id);

        if (entity != null)
        {
            // 1. Оновлюємо базові поля
            entity.Name = armorDto.Name;
            entity.EquipmentTypeId = armorDto.EquipmentTypeId;
            entity.IconPath = armorDto.IconPath;
            entity.Weight = armorDto.Weight;
            entity.Poise = armorDto.Poise;

            // 2. Очищаємо старі значення захисту
            entity.ArmorInfluences.Clear();

            // 3. Додаємо нові (якщо вони були введені)
            void AddInfluence(double? val, int typeId)
            {
                if (val.HasValue) 
                    entity.ArmorInfluences.Add(new ArmorInfluence { InfluenceTypeId = typeId, Value = val.Value });
            }

            AddInfluence(armorDto.Physical, 1);
            AddInfluence(armorDto.Strike, 2);
            AddInfluence(armorDto.Slash, 3);
            AddInfluence(armorDto.Pierce, 4);
            AddInfluence(armorDto.Magic, 5);
            AddInfluence(armorDto.Fire, 6);
            AddInfluence(armorDto.Lightning, 7);
            AddInfluence(armorDto.Holy, 8);
            // ... додай інші типи шкоди як ти робив для AddArmorAsync
        }
    }

    // Аналогічно для зброї
    public async Task UpdateWeaponAsync(WeaponDTO weaponDto)
    {
        var entity = await DbContext.Set<BaseEquipment>()
            .OfType<WeaponEquipment>()
            .Include(w => w.WeaponInfluences)
            .FirstOrDefaultAsync(w => w.Id == weaponDto.Id);

        if (entity != null)
        {
            entity.Name = weaponDto.Name;
            entity.EquipmentTypeId = weaponDto.EquipmentTypeId;
            entity.IconPath = weaponDto.IconPath;
            entity.Weight = weaponDto.Weight;
            entity.Damage = weaponDto.Damage;
            entity.ReqStrength = weaponDto.ReqStrength;
            // ... онови інші базові поля (Dex, Int, Faith) ...

            entity.WeaponInfluences.Clear();

            void AddInfluence(double? val, int typeId)
            {
                if (val.HasValue) 
                    entity.WeaponInfluences.Add(new WeaponInfluence { InfluenceTypeId = typeId, Value = val.Value });
            }

            AddInfluence(weaponDto.Physical, 1);
            AddInfluence(weaponDto.Strike, 2);     // ID для Strike
            AddInfluence(weaponDto.Slash, 3);      // ID для Slash
            AddInfluence(weaponDto.Pierce, 4);     // ID для Pierce
            AddInfluence(weaponDto.Magic, 5);      // ID для Magic
            AddInfluence(weaponDto.Fire, 6);       // ID для Fire
            AddInfluence(weaponDto.Lightning, 7);  // ID для Lightning
            AddInfluence(weaponDto.Holy, 8);       // ID для Holy
            // ... додай інші типи шкоди ...
        }
    }
}
