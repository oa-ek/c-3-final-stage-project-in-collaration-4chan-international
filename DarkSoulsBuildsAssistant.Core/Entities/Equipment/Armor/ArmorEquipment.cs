using DarkSoulsBuildsAssistant.Core.Entities.Base;
using DarkSoulsBuildsAssistant.Core.Entities.Etc;

using System.ComponentModel.DataAnnotations.Schema;

namespace DarkSoulsBuildsAssistant.Core.Entities.Armor;

public class ArmorEquipment : NamedEntity
{
    public string? Description { get; set; }

    public decimal? Weight { get; set; }

    public decimal? Poise { get; set; }

    public string? Skin { get; set; }
    
    public string? IconPath { get; set; }
    
    public int? ArmorTypeId { get; set; }
    
    [ForeignKey(nameof(ArmorTypeId))]
    public ArmorType? ArmorType { get; set; }

    public virtual ICollection<ArmorInfluence> ArmorInfluences { get; set; } = new List<ArmorInfluence>();

    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();
}
