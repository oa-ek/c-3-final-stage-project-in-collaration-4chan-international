using System.ComponentModel.DataAnnotations.Schema;
using DarkSoulsBuildsAssistant.Core.Entities.Base;

namespace DarkSoulsBuildsAssistant.Core.Entities.Equipment;

public abstract class BaseEquipment : NamedEntity
{
    public decimal? Weight { get; set; }
    
    public string? IconPath { get; set; }
    
    public string? Description { get; set; }

    public int? EquipmentTypeId { get; set; }
    
    [ForeignKey(nameof(EquipmentTypeId))]
    public EquipmentType? EquipmentType { get; set; }
}
