using System.ComponentModel.DataAnnotations.Schema;
using DarkSoulsBuildsAssistant.Core.Entities.Base;

namespace DarkSoulsBuildsAssistant.Core.Entities.Equipment;

public abstract class EquipmentType : NamedEntity
{
    public int? SlotId { get; set; }

    [ForeignKey(nameof(SlotId))]
    public Slot? Slot { get; set; }
}
