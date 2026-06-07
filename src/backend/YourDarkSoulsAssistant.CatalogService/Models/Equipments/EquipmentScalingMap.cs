namespace YourDarkSoulsAssistant.CatalogService.Models.Equipments;

public class EquipmentScalingMap
{
    public Guid EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = null!;
    public Guid ScalingId { get; set; }
    public EquipmentScaling Scaling { get; set; } = null!;
    public decimal Value { get; set; }
}
