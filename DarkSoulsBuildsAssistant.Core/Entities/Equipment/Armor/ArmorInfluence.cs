using DarkSoulsBuildsAssistant.Core.Entities.Base;
using DarkSoulsBuildsAssistant.Core.Entities.Etc;
 
using System.ComponentModel.DataAnnotations.Schema;

namespace DarkSoulsBuildsAssistant.Core.Entities.Armor;

public class ArmorInfluence : BaseEntity
{
    public double? Value { get; set; }
    
    public int? ArmorId { get; set; }
    
    public int? InfluenceTypeId { get; set; }
    
    [ForeignKey(nameof(ArmorId))]
    public ArmorEquipment? Armor { get; set; }
    
    [ForeignKey(nameof(InfluenceTypeId))]
    public InfluenceType? Influence { get; set; }
}
