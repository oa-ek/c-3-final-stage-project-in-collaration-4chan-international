namespace YourDarkSoulsAssistant.CatalogService.Models.Equipments;

public class EquipmentType
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? DescriptionId { get; set; }
    public Guid? BaseTypeId { get; set; }
    public Guid? AdditionalInfluenceId { get; set; }

    public int GameId { get; set; }
    public Game Game { get; set; } = null!;

    public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
}
