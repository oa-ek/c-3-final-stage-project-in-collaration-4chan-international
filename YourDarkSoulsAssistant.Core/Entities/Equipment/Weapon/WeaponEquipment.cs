using DarkSoulsBuildsAssistant.Core.Entities.Character;

namespace DarkSoulsBuildsAssistant.Core.Entities.Equipment.Weapon;

public class WeaponEquipment : BaseEquipment
{
    public decimal? Damage { get; set; }
    
    public int? ReqStrength { get; set; }
    
    public int? ReqDexterity { get; set; }
    
    public int? ReqIntelligence { get; set; }
    
    public int? ReqFaith { get; set; }
    
    public virtual ICollection<WeaponInfluence> WeaponInfluences { get; set; } = new List<WeaponInfluence>();
    
    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();
}
