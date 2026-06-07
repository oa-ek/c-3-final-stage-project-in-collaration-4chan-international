namespace YourDarkSoulsAssistant.BuildsService.Models;

public class Build
{
    public Guid SetId { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid CharacterId { get; set; }

    public Character? Character { get; set; }
    public ICollection<EquipmentBuild> EquipmentBuilds { get; set; } = new List<EquipmentBuild>();
}
