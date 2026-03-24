using DarkSoulsBuildsAssistant.Core.Entities.Armor;
using DarkSoulsBuildsAssistant.Core.Entities.Base;
using DarkSoulsBuildsAssistant.Core.Entities.Weapon;

namespace DarkSoulsBuildsAssistant.Core.Entities.Etc;

public class Slot : NamedEntity
{
    public ArmorType? ArmorType { get; set; }
    
    public WeaponType? WeaponType { get; set; }
}
