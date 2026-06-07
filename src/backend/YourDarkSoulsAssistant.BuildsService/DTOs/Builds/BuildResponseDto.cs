namespace YourDarkSoulsAssistant.BuildsService.DTOs.Builds;

public class BuildResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public CharacterStatsDto Stats { get; set; } = new();
    public EquipmentSlotsDto Equipment { get; set; } = new();
}
