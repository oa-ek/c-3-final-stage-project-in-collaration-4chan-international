using YourDarkSoulsAssistant.CatalogService.Models.Equipments;

namespace YourDarkSoulsAssistant.CatalogService.Models;

public class Game
{
    public int GameId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
    public ICollection<Slot> Slots { get; set; } = new List<Slot>();
}
