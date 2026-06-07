using YourDarkSoulsAssistant.CatalogService.Models.Equipments;

namespace YourDarkSoulsAssistant.CatalogService.Models;

public class Slot
{
    public int SlotId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int GameId { get; set; }
    public Game Game { get; set; } = null!;

    public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
}