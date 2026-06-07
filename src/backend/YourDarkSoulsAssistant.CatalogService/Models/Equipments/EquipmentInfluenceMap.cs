namespace YourDarkSoulsAssistant.CatalogService.Models.Equipments;

public class EquipmentInfluenceMap
{
    public Guid EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = null!;
    public Guid InfluenceId { get; set; }
    public Influence Influence { get; set; } = null!;
    public float Value { get; set; }
}
