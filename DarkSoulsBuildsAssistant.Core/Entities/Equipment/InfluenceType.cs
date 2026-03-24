using DarkSoulsBuildsAssistant.Core.Entities.Armor;
using DarkSoulsBuildsAssistant.Core.Entities.Base;
using DarkSoulsBuildsAssistant.Core.Entities.Weapon;

namespace DarkSoulsBuildsAssistant.Core.Entities.Etc;

public class InfluenceType : NamedEntity
{
    public virtual ICollection<ArmorInfluence> ArmorInfluences { get; set; } = new List<ArmorInfluence>();

    public virtual ICollection<WeaponInfluence> WeaponInfluences { get; set; } = new List<WeaponInfluence>();
}
