using AutoMapper;
using DarkSoulsBuildsAssistant.Core.DTOs.Equipment;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment.Armor;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment.Weapon;

namespace DarkSoulsBuildsAssistant.Core.Mapping;

public class EquipmentMappingProfile : Profile
{
    public EquipmentMappingProfile()
    {
        // 1. Базовий мапінг загальних полів
        CreateMap<BaseEquipment, EquipmentDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.IconPath, opt => opt.MapFrom(src => src.IconPath))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
            .ForMember(dest => dest.EquipmentTypeName, opt => opt.MapFrom(src => src.EquipmentType.Name))
            .ForMember(dest => dest.SlotName, opt => opt.MapFrom(src => src.EquipmentType.Slot.Name))
            // Додаємо Include для спадкоємців, щоб AutoMapper знав, куди дивитися
            .Include<WeaponEquipment, EquipmentDTO>()
            .Include<ArmorEquipment, EquipmentDTO>();

        // 2. Специфічний мапінг для Зброї
        CreateMap<WeaponEquipment, EquipmentDTO>()
            .ForMember(dest => dest.Damage, opt => opt.MapFrom(src => src.Damage))
            .ForMember(dest => dest.ReqStrength, opt => opt.MapFrom(src => src.ReqStrength))
            .ForMember(dest => dest.ReqDexterity, opt => opt.MapFrom(src => src.ReqDexterity))
            .ForMember(dest => dest.ReqIntelligence, opt => opt.MapFrom(src => src.ReqIntelligence))
            .ForMember(dest => dest.ReqFaith, opt => opt.MapFrom(src => src.ReqFaith))
            // Для зброї Poise буде null, і це нормально
            .ForMember(dest => dest.Poise, opt => opt.Ignore())
            .ForMember(dest => dest.Skin, opt => opt.Ignore());

        // 3. Специфічний мапінг для Броні
        CreateMap<ArmorEquipment, EquipmentDTO>()
            .ForMember(dest => dest.Poise, opt => opt.MapFrom(src => src.Poise))
            // Для броні Damage буде null
            .ForMember(dest => dest.Damage, opt => opt.Ignore());
            
        // 4. Мапінг слота (для списку у CharacterBuildDTO)
        CreateMap<Slot, SlotViewDTO>()
            .ForMember(dest => dest.SlotName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.EquippedItem, opt => opt.Ignore()); // Item додаватимемо вручну в сервісі
    }
}