using System.ComponentModel.DataAnnotations.Schema;
using DarkSoulsBuildsAssistant.Core.Entities.Base;
using DarkSoulsBuildsAssistant.Core.Entities.Etc;

namespace DarkSoulsBuildsAssistant.Core.Entities.Weapon;

public class WeaponType : NamedEntity
{
    public int? SlotId { get; set; }

    [ForeignKey(nameof(SlotId))]
    public Slot? Slot { get; set; }
}
