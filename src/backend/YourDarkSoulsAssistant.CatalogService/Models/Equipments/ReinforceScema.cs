namespace YourDarkSoulsAssistant.CatalogService.Models.Equipments;

public class ReinforceSchema
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int GameId { get; set; }
    public Game Game { get; set; } = null!;

    public ICollection<ReinforceStep> Steps { get; set; } = new List<ReinforceStep>();
    public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
}
