using DarkSoulsBuildsAssistant.Core.Entities.Armor;
using DarkSoulsBuildsAssistant.Core.Entities.Base;
using DarkSoulsBuildsAssistant.Core.Entities.Etc;
using DarkSoulsBuildsAssistant.Core.Entities.Weapon;

using System.ComponentModel.DataAnnotations.Schema;

namespace DarkSoulsBuildsAssistant.Core.Entities.Character;

public class CharacterBuild : NamedEntity
{
    public int? Level { get; set; }
    
    public int? Vigor { get; set; }
    
    public int? Endurance { get; set; }
    
    public int? Strength { get; set; }
    
    public int? Dexterity { get; set; }
    
    public int? Intelligence { get; set; }
    
    public int? Faith { get; set; }
    
    public int? RightHandId { get; set; }
    
    public int? LeftHandId { get; set; }
    
    public int? HeadId { get; set; }
    
    public int? TorsoId { get; set; }
    
    public int? LegsId { get; set; }
    
    [ForeignKey(nameof(RightHandId))]
    public virtual WeaponEquipment? RightHand { get; set; }
    
    [ForeignKey(nameof(LeftHandId))]
    public virtual WeaponEquipment? LeftHand { get; set; }
    
    [ForeignKey(nameof(HeadId))]
    public virtual ArmorEquipment? Head { get; set; }
    
    [ForeignKey(nameof(TorsoId))]
    public virtual ArmorEquipment? Torso { get; set; }
    
    [ForeignKey(nameof(LegsId))]
    public virtual ArmorEquipment? Legs { get; set; }
    
    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();
}
