using System.ComponentModel.DataAnnotations.Schema;
using DarkSoulsBuildsAssistant.Core.Entities.Base;
using DarkSoulsBuildsAssistant.Core.Entities.System;

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
    
    public int? UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
    
    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();
}
