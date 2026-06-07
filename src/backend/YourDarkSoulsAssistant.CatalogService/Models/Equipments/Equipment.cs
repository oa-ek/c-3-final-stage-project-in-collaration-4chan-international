namespace YourDarkSoulsAssistant.CatalogService.Models.Equipments;

public class Equipment
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? DescriptionId { get; set; }
    public float Weight { get; set; }
    public int Durability { get; set; }
    public string IconPath { get; set; } = string.Empty;

    public Guid EquipmentTypeId { get; set; }
    public EquipmentType EquipmentType { get; set; } = null!;

    public int SlotId { get; set; }
    public Slot Slot { get; set; } = null!;

    public Guid? ReinforceSchemaId { get; set; }
    public ReinforceSchema? ReinforceSchema { get; set; }

    public int GameId { get; set; }
    public Game Game { get; set; } = null!;

    public ICollection<ReqAttribute> RequiredAttributes { get; set; } = new List<ReqAttribute>();
    public ICollection<EquipmentScalingMap> Scalings { get; set; } = new List<EquipmentScalingMap>();
    public ICollection<EquipmentInfluenceMap> Influences { get; set; } = new List<EquipmentInfluenceMap>();
}
