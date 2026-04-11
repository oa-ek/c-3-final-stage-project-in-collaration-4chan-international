using DarkSoulsBuildsAssistant.Core.DTOs.Base;
using DarkSoulsBuildsAssistant.Core.DTOs.Equipment;

namespace DarkSoulsBuildsAssistant.Core.DTOs.Character;

public record CharacterBuildDTO : NamedDTO
{
    public int? Level { get; init; }
    
    public int? Vigor { get; init; }
    
    public int? Endurance { get; init; }
    
    public int? Strength { get; init; }
    
    public int? Dexterity { get; init; }
    
    public int? Intelligence { get; init; }

    public int? Faith { get; init; }

    public virtual ICollection<SetDTO> Sets { get; init; } = new List<SetDTO>();
}
