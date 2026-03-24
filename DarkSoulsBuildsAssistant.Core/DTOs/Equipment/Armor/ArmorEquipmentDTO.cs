namespace DarkSoulsBuildsAssistant.Core.DTOs.Business.Armor;

public record ArmorEquipmentDTO
{
    public string? Name { get; init; }
    
    public string? Description { get; init; }

    public decimal? Weight { get; init; }

    public decimal? Poise { get; init; }

    public string? Skin { get; init; }
    
    public string? IconPath { get; init; }

    public ArmorTypeDTO? ArmorType { get; set; }

    public virtual ICollection<ArmorInfluenceDTO> ArmorInfluences { get; set; } = new List<ArmorInfluenceDTO>();

    public virtual ICollection<SetDTO> Sets { get; set; } = new List<SetDTO>();
}