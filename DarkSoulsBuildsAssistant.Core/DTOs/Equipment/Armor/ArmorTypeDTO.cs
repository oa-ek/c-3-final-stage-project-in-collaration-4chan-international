namespace DarkSoulsBuildsAssistant.Core.DTOs.Business.Armor;

public record ArmorTypeDTO()
{
    public string? Name { get; init; }
    
    public SlotDTO? Slot { get; set; }
}