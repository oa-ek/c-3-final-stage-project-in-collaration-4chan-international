using AutoMapper;
using System.Globalization;
using YourDarkSoulsAssistant.CatalogService.DTOs.Equipment;
using YourDarkSoulsAssistant.CatalogService.DTOs.GameItems;
using YourDarkSoulsAssistant.CatalogService.Models;
using YourDarkSoulsAssistant.CatalogService.Models.Equipments;

namespace YourDarkSoulsAssistant.CatalogService.Mappings;

public class EquipmentProfile : Profile
{
    public EquipmentProfile()
    {
        CreateMap<Equipment, EquipmentResponseDTO>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.EquipmentType.Name))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.IconPath))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight.ToString("F1", CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => MapCategory(src.EquipmentType.Name)))
            .ForMember(dest => dest.Required, opt => opt.MapFrom(src => MapRequired(src.RequiredAttributes)))
            .ForMember(dest => dest.Scaling, opt => opt.MapFrom(src => MapScaling(src.Scalings)))
            .ForMember(dest => dest.Attack, opt => opt.MapFrom(src => MapAttack(src.Influences)))
            .ForMember(dest => dest.Guard, opt => opt.MapFrom(src => MapGuard(src.Influences)));
        
        CreateMap<Game, GameResponseDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GameId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.IconPath, opt => opt.MapFrom(src => src.Icon));
    }

    private RequiredStats MapRequired(ICollection<ReqAttribute> attributes)
    {
        var stats = new RequiredStats();

        foreach (var attribute in attributes)
        {
            var key = NormalizeKey(attribute.Attribute?.Name) ?? NormalizeKey(attribute.Attribute?.IconPath);
            var value = FormatNumber(attribute.Value);
            SetRequiredStat(stats, key, value);
        }

        return stats;
    }

    private ScalingStats MapScaling(ICollection<EquipmentScalingMap> scalings)
    {
        var stats = new ScalingStats();

        foreach (var scaling in scalings)
        {
            var key = NormalizeKey(scaling.Scaling?.IconPath) ?? NormalizeKey(scaling.Scaling?.Name);
            var value = string.IsNullOrWhiteSpace(scaling.Scaling?.Name)
                ? FormatNumber(scaling.Value)
                : scaling.Scaling.Name;

            SetScalingStat(stats, key, value);
        }

        return stats;
    }

    private AttackStats MapAttack(ICollection<EquipmentInfluenceMap> influences)
    {
        var stats = new AttackStats();

        foreach (var influence in influences)
        {
            var key = NormalizeKey(influence.Influence?.InfluenceType?.Name);
            var value = FormatNumber(influence.Value);

            switch (key)
            {
                case "physical":
                    stats.Physical = value;
                    break;
                case "magic":
                    stats.Magic = value;
                    break;
                case "fire":
                    stats.Fire = value;
                    break;
                case "lightning":
                    stats.Lightning = value;
                    break;
                case "holy":
                    stats.Holy = value;
                    break;
                case "critical":
                    stats.Critical = value;
                    break;
            }
        }

        return stats;
    }

    private GuardStats MapGuard(ICollection<EquipmentInfluenceMap> influences)
    {
        var stats = new GuardStats();

        foreach (var influence in influences)
        {
            var key = NormalizeKey(influence.Influence?.InfluenceType?.Name);
            var value = FormatNumber(influence.Value);

            switch (key)
            {
                case "physicalguard":
                case "guardphysical":
                case "physicalnegation":
                case "physical":
                    stats.Physical = value;
                    break;
                case "magicguard":
                case "guardmagic":
                case "magicnegation":
                case "magic":
                    stats.Magic = value;
                    break;
                case "fireguard":
                case "guardfire":
                case "firenegation":
                case "fire":
                    stats.Fire = value;
                    break;
                case "lightningguard":
                case "guardlightning":
                case "lightningnegation":
                case "lightning":
                    stats.Lightning = value;
                    break;
                case "holyguard":
                case "guardholy":
                case "holynegation":
                case "holy":
                    stats.Holy = value;
                    break;
                case "boost":
                case "guardboost":
                    stats.Boost = value;
                    break;
            }
        }

        return stats;
    }

    private static void SetRequiredStat(RequiredStats stats, string? key, string value)
    {
        switch (key)
        {
            case "strength":
            case "str":
                stats.Str = value;
                break;
            case "dexterity":
            case "dex":
                stats.Dex = value;
                break;
            case "intelligence":
            case "int":
                stats.Int = value;
                break;
            case "faith":
            case "fai":
                stats.Fai = value;
                break;
            case "arcane":
            case "arc":
                stats.Arc = value;
                break;
        }
    }

    private static void SetScalingStat(ScalingStats stats, string? key, string value)
    {
        switch (key)
        {
            case "strength":
            case "str":
                stats.Str = value;
                break;
            case "dexterity":
            case "dex":
                stats.Dex = value;
                break;
            case "intelligence":
            case "int":
                stats.Int = value;
                break;
            case "faith":
            case "fai":
                stats.Fai = value;
                break;
            case "arcane":
            case "arc":
                stats.Arc = value;
                break;
        }
    }

    private static string MapCategory(string? equipmentTypeName)
    {
        var key = NormalizeKey(equipmentTypeName) ?? string.Empty;

        if (key.Contains("armor") || key.Contains("helm") || key.Contains("gauntlet") || key.Contains("greave"))
        {
            return "armor";
        }

        if (key.Contains("talisman") || key.Contains("ring"))
        {
            return "talisman";
        }

        return "weapon";
    }

    private static string FormatNumber(float value)
        => value.ToString("0.##", CultureInfo.InvariantCulture);

    private static string FormatNumber(decimal value)
        => value.ToString("0.##", CultureInfo.InvariantCulture);

    private static string? NormalizeKey(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var chars = value.Where(char.IsLetterOrDigit).ToArray();
        return new string(chars).ToLowerInvariant();
    }
}