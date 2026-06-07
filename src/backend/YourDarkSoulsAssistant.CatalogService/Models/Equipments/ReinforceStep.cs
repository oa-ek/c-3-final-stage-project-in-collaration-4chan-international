using System.Text.Json;

namespace YourDarkSoulsAssistant.CatalogService.Models.Equipments;

public class ReinforceStep
{
    public Guid Id { get; set; }

    public Guid SchemaId { get; set; }
    
    public int Level { get; set; }

    // Використовуємо jsonb для гнучкого збереження модифікаторів
    public JsonDocument Modifiers { get; set; } = null!;
}
