using AutoMapper;
using YourDarkSoulsAssistant.BuildsService.DTOs.Builds;
using YourDarkSoulsAssistant.BuildsService.Models;

namespace YourDarkSoulsAssistant.BuildsService.Mappings;

public class BuildProfile : Profile
{
    private static readonly Dictionary<string, Action<CharacterStatsDto, int>> StatSetters = new(StringComparer.OrdinalIgnoreCase)
    {
        ["vigor"] = (stats, value) => stats.Vigor = value,
        ["mind"] = (stats, value) => stats.Mind = value,
        ["endurance"] = (stats, value) => stats.Endurance = value,
        ["strength"] = (stats, value) => stats.Strength = value,
        ["dexterity"] = (stats, value) => stats.Dexterity = value,
        ["intelligence"] = (stats, value) => stats.Intelligence = value,
        ["faith"] = (stats, value) => stats.Faith = value,
        ["arcane"] = (stats, value) => stats.Arcane = value
    };

    public BuildProfile()
    {
        CreateMap<Build, BuildResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SetId))
            .ForMember(dest => dest.Stats, opt => opt.MapFrom(src => CreateStats(src)))
            .ForMember(dest => dest.Equipment, opt => opt.MapFrom(src => CreateEquipment(src)));
    }

    private static CharacterStatsDto CreateStats(Build build)
    {
        var stats = new CharacterStatsDto
        {
            Level = build.Character?.Level ?? 1
        };

        var attributes = build.Character?.CharacterAttributes;
        if (attributes is null)
        {
            return stats;
        }

        foreach (var attribute in attributes)
        {
            var key = NormalizeKey(attribute.AttributeType?.Name);
            if (key is null)
            {
                continue;
            }

            if (StatSetters.TryGetValue(key, out var setter))
            {
                setter(stats, attribute.Value);
            }
        }

        return stats;
    }

    private static EquipmentSlotsDto CreateEquipment(Build build)
    {
        var equipmentIds = build.EquipmentBuilds
            .OrderBy(x => x.EquipmentId)
            .Select(x => (Guid?)x.EquipmentId)
            .ToArray();

        var slots = new EquipmentSlotsDto();
        CopyInto(slots.Weapons, equipmentIds, 0);
        CopyInto(slots.Shields, equipmentIds, 3);
        CopyInto(slots.Arrows, equipmentIds, 6);
        slots.Armor.Head = equipmentIds.ElementAtOrDefault(10);
        slots.Armor.Chest = equipmentIds.ElementAtOrDefault(11);
        slots.Armor.Hands = equipmentIds.ElementAtOrDefault(12);
        slots.Armor.Legs = equipmentIds.ElementAtOrDefault(13);
        CopyInto(slots.Talismans, equipmentIds, 14);
        CopyInto(slots.Consumables, equipmentIds, 18);

        return slots;
    }

    private static void CopyInto(Guid?[] target, Guid?[] source, int offset)
    {
        for (var i = 0; i < target.Length; i++)
        {
            target[i] = source.ElementAtOrDefault(offset + i);
        }
    }

    private static string? NormalizeKey(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim().ToLowerInvariant();
    }
}