using DarkSoulsBuildsAssistant.Core.Entities.Armor;
using DarkSoulsBuildsAssistant.Core.Entities.Base;
using DarkSoulsBuildsAssistant.Core.Entities.Character;
using DarkSoulsBuildsAssistant.Core.Entities.Weapon;

using System.ComponentModel.DataAnnotations.Schema;

namespace DarkSoulsBuildsAssistant.Core.Entities.Etc;

public class Set : NamedEntity 
{
    public int? CharacterBuildId { get; set; }
    
    [ForeignKey(nameof(CharacterBuildId))]
    public CharacterBuild? CharacterBuild { get; set; }

    public virtual ICollection<ArmorEquipment> Armors { get; set; } = new List<ArmorEquipment>();

    public virtual ICollection<WeaponEquipment> Weapons { get; set; } = new List<WeaponEquipment>();
}
