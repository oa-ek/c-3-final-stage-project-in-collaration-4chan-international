namespace YourDarkSoulsAssistant.CatalogService.Models.Equipments;

public class EquipmentScaling
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty; // Наприклад: "S", "A", "B"

    public Guid? DescriptionId { get; set; }

    public string IconPath { get; set; } = string.Empty;

    public int GameId { get; set; }
    public Game Game { get; set; } = null!;

    public ICollection<EquipmentScalingMap> EquipmentScalings { get; set; } = new List<EquipmentScalingMap>();
}
