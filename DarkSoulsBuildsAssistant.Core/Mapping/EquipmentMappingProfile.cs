using AutoMapper;
using DarkSoulsBuildsAssistant.Core.DTOs.Character;
using DarkSoulsBuildsAssistant.Core.DTOs.Equipment;
using DarkSoulsBuildsAssistant.Core.Entities.Character;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment.Armor;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment.Weapon;

namespace DarkSoulsBuildsAssistant.Core.Mapping;

public class EquipmentMappingProfile : Profile
{
    public EquipmentMappingProfile()
    {
        CreateMap<BaseEquipment, EquipmentDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.IconPath, opt => opt.MapFrom(src => src.IconPath))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
            .ForMember(dest => dest.EquipmentTypeName, opt => opt.MapFrom(src => src.EquipmentType.Name))
            .ForMember(dest => dest.SlotName, opt => opt.MapFrom(src => src.EquipmentType.Slot.Name))
            .Include<WeaponEquipment, EquipmentDTO>()
            .Include<ArmorEquipment, EquipmentDTO>();
        
        CreateMap<WeaponEquipment, EquipmentDTO>()
            .ForMember(dest => dest.Damage, opt => opt.MapFrom(src => src.Damage))
            .ForMember(dest => dest.ReqStrength, opt => opt.MapFrom(src => src.ReqStrength))
            .ForMember(dest => dest.ReqDexterity, opt => opt.MapFrom(src => src.ReqDexterity))
            .ForMember(dest => dest.ReqIntelligence, opt => opt.MapFrom(src => src.ReqIntelligence))
            .ForMember(dest => dest.ReqFaith, opt => opt.MapFrom(src => src.ReqFaith))
            .ForMember(dest => dest.Poise, opt => opt.Ignore())
            .ForMember(dest => dest.Skin, opt => opt.Ignore());
        
        CreateMap<ArmorEquipment, EquipmentDTO>()
            .ForMember(dest => dest.Poise, opt => opt.MapFrom(src => src.Poise))
            .ForMember(dest => dest.Damage, opt => opt.Ignore());
        
        CreateMap<Slot, SlotViewDTO>()
            .ForMember(dest => dest.SlotName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.EquippedItem, opt => opt.Ignore()); // Item додаватимемо вручну в сервісі
        
        CreateMap<CharacterBuild, CharacterBuildDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
            .ForMember(dest => dest.Sets, opt => opt.MapFrom(src => src.Sets));
        
        CreateMap<Set, SetDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        
        CreateMap<ArmorDTO, ArmorEquipment>()
            // Ігноруємо колекцію під час стандартного мапінгу, бо заповнимо її вручну
            .ForMember(dest => dest.ArmorInfluences, opt => opt.Ignore())
            // Ігноруємо Id, оскільки це створення нового запису
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            // Задаємо власну логіку для створення Influences
            .AfterMap((src, dest) =>
            {
                dest.ArmorInfluences = new List<ArmorInfluence>();

                // Метод-помічник для додавання характеристик, якщо вони введені користувачем
                // Примітка: Тобі потрібно буде замінити цифри (1, 2, 3...) на реальні ID типів захисту (InfluenceTypeId) з твоєї бази даних
                void AddInfluenceIfPresent(double? value, int influenceTypeId)
                {
                    if (value is > 0)
                    {
                        dest.ArmorInfluences.Add(new ArmorInfluence 
                        { 
                            InfluenceTypeId = influenceTypeId, 
                            Value = value.Value 
                        });
                    }
                }

                AddInfluenceIfPresent(src.Physical, 1);   // ID для Physical
                AddInfluenceIfPresent(src.Strike, 2);     // ID для Strike
                AddInfluenceIfPresent(src.Slash, 3);      // ID для Slash
                AddInfluenceIfPresent(src.Pierce, 4);     // ID для Pierce
                AddInfluenceIfPresent(src.Magic, 5);      // ID для Magic
                AddInfluenceIfPresent(src.Fire, 6);       // ID для Fire
                AddInfluenceIfPresent(src.Lightning, 7);  // ID для Lightning
                AddInfluenceIfPresent(src.Holy, 8);       // ID для Holy
            });
        CreateMap<WeaponDTO, WeaponEquipment>()
            .ForMember(dest => dest.WeaponInfluences, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.WeaponInfluences = new List<WeaponInfluence>();

                void AddInfluenceIfPresent(double? value, int influenceTypeId)
                {
                    if (value is > 0)
                    {
                        dest.WeaponInfluences.Add(new WeaponInfluence 
                        { 
                            InfluenceTypeId = influenceTypeId, 
                            Value = value.Value 
                        });
                    }
                }

                AddInfluenceIfPresent(src.Physical, 1);   // ID для Physical
                AddInfluenceIfPresent(src.Strike, 2);     // ID для Strike
                AddInfluenceIfPresent(src.Slash, 3);      // ID для Slash
                AddInfluenceIfPresent(src.Pierce, 4);     // ID для Pierce
                AddInfluenceIfPresent(src.Magic, 5);      // ID для Magic
                AddInfluenceIfPresent(src.Fire, 6);       // ID для Fire
                AddInfluenceIfPresent(src.Lightning, 7);  // ID для Lightning
                AddInfluenceIfPresent(src.Holy, 8);       // ID для Holy
            });
    }
}