using DarkSoulsBuildsAssistant.Core.DTOs.Base;

namespace DarkSoulsBuildsAssistant.Core.DTOs.Equipment;

public record EquipmentDTO: NamedDTO
{
    public string? IconPath { get; init; }
    
    public decimal? Weight { get; init; }
    
    public decimal? Damage { get; init; }
    
    public int? ReqStrength { get; init; }
    
    public int? ReqDexterity { get; init; }
    
    public int? ReqIntelligence { get; init; }
    
    public int? ReqFaith { get; init; }
    
    public decimal? Poise { get; init; }
    
    public string? Skin { get; init; }
    
    public string? EquipmentTypeName { get; init; }
    
    public string? SlotName { get; init; }
}
