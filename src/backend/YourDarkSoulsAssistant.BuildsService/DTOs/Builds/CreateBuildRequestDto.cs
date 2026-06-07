using System.ComponentModel.DataAnnotations;

namespace YourDarkSoulsAssistant.BuildsService.DTOs.Builds;

public class CreateBuildRequestDto
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(128)]
    public string UserId { get; set; } = string.Empty;

    [MaxLength(255)]
    public string CharacterName { get; set; } = "Tarnished";

    public CharacterStatsDto Stats { get; set; } = new();
    public EquipmentSlotsDto Equipment { get; set; } = new();
}