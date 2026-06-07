namespace YourDarkSoulsAssistant.CatalogService.Models.Equipments;

public class CharacterAttribute
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string IconPath { get; set; } = string.Empty;
    public int GameId { get; set; }
}