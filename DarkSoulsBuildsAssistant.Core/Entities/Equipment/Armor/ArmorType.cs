using DarkSoulsBuildsAssistant.Core.Entities.Base;
using DarkSoulsBuildsAssistant.Core.Entities.Etc;

using System.ComponentModel.DataAnnotations.Schema;

namespace DarkSoulsBuildsAssistant.Core.Entities.Armor;

public class ArmorType : NamedEntity
{
    public int? SlotId { get; set; }

    [ForeignKey(nameof(SlotId))]
    public Slot? Slot { get; set; }
}
