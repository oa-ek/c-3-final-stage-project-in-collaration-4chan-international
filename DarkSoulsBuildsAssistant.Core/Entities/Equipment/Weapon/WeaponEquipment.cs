using DarkSoulsBuildsAssistant.Core.Entities.Base;
using DarkSoulsBuildsAssistant.Core.Entities.Etc;

using System.ComponentModel.DataAnnotations.Schema;

namespace DarkSoulsBuildsAssistant.Core.Entities.Weapon;

public class WeaponEquipment : NamedEntity
{
    public decimal? Damage { get; set; }

    public decimal? Weight { get; set; }

    public int? ReqStrength { get; set; }

    public int? ReqDexterity { get; set; }

    public int? ReqIntelligence { get; set; }

    public int? ReqFaith { get; set; }

    public string? IconPath { get; set; }
    
    public int? WeaponTypeId { get; set; }
    
    [ForeignKey(nameof(WeaponTypeId))]
    public WeaponType? WeaponType { get; set; }
    
    public virtual ICollection<WeaponInfluence> WeaponInfluences { get; set; } = new List<WeaponInfluence>();

    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();
}
