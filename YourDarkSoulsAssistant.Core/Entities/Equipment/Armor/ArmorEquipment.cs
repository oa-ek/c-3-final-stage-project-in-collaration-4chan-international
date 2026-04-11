using DarkSoulsBuildsAssistant.Core.Entities.Character;

namespace DarkSoulsBuildsAssistant.Core.Entities.Equipment.Armor;

public class ArmorEquipment : BaseEquipment
{
    public decimal? Poise { get; set; }
    
    public string? Skin { get; set; }
    
    public virtual ICollection<ArmorInfluence> ArmorInfluences { get; set; } = new List<ArmorInfluence>();
    
    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();
}
