namespace YourDarkSoulsAssistant.CatalogService.Models.Equipments;

public class Influence
{
    public Guid Id { get; set; }

    public decimal Value { get; set; }

    public int InfluenceTypeId { get; set; }
    public InfluenceType InfluenceType { get; set; } = null!;

    public int OperationTypeId { get; set; }
    public Operation Operation { get; set; } = null!;

    public Guid? DescriptionId { get; set; }

    public int GameId { get; set; }
    public Game Game { get; set; } = null!;

    public string IconPath { get; set; } = string.Empty;

    public ICollection<EquipmentInfluenceMap> EquipmentInfluences { get; set; } = new List<EquipmentInfluenceMap>();
}