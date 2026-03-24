using DarkSoulsBuildsAssistant.Core.Entities.Base;

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
    
    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();
}
