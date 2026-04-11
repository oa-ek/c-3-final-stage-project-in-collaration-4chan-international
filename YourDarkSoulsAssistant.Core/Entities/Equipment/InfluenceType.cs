using DarkSoulsBuildsAssistant.Core.Entities.Base;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment.Armor;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment.Weapon;

namespace DarkSoulsBuildsAssistant.Core.Entities.Equipment;

public class InfluenceType : NamedEntity
{
    public virtual ICollection<ArmorInfluence> ArmorInfluences { get; set; } = new List<ArmorInfluence>();

    public virtual ICollection<WeaponInfluence> WeaponInfluences { get; set; } = new List<WeaponInfluence>();
}
