namespace YourDarkSoulsAssistant.CatalogService.Models.Equipments;

// Моделі зв'язку (для складених ключів Many-to-Many)
public class ReqAttribute
{
    public Guid EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = null!;
    public Guid AttributeId { get; set; }
    public CharacterAttribute Attribute { get; set; } = null!;
    public float Value { get; set; }
}
