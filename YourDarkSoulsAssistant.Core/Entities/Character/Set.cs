using DarkSoulsBuildsAssistant.Core.Entities.Base;

using System.ComponentModel.DataAnnotations.Schema;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment;

namespace DarkSoulsBuildsAssistant.Core.Entities.Character;

public class Set : NamedEntity 
{
    public int? CharacterBuildId { get; set; }
    
    [ForeignKey(nameof(CharacterBuildId))]
    public CharacterBuild? CharacterBuild { get; set; }

    // Уніфікована колекція екіпірування
    public virtual ICollection<BaseEquipment> Equipments { get; set; } = new List<BaseEquipment>();
}
