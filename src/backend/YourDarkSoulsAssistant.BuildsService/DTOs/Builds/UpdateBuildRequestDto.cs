using System.ComponentModel.DataAnnotations;

namespace YourDarkSoulsAssistant.BuildsService.DTOs.Builds;

public class UpdateBuildRequestDto
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(128)]
    public string? UserId { get; set; }

    public CharacterStatsDto Stats { get; set; } = new();
    public EquipmentSlotsDto Equipment { get; set; } = new();
}