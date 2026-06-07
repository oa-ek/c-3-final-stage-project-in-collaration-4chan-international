namespace YourDarkSoulsAssistant.CatalogService.Models.Equipments;

// Моделі зв'язку (для складених ключів Many-to-Many)
public class ReqAttribute
{
    public Guid EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = null!;
    public int AttributeId { get; set; }
    public string AttributeKey { get; set; } = string.Empty;
    public float Value { get; set; }
}
