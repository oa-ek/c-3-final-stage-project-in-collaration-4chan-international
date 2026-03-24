using System.ComponentModel.DataAnnotations.Schema;
using DarkSoulsBuildsAssistant.Core.Entities.Base;
using DarkSoulsBuildsAssistant.Core.Entities.Etc;

namespace DarkSoulsBuildsAssistant.Core.Entities.Weapon;

public class WeaponInfluence : BaseEntity
{
    public double? Value { get; set; }
    
    public int? InfluenceTypeId { get; set; }
    
    public int? WeaponId { get; set; }
    
    [ForeignKey(nameof(InfluenceTypeId))]
    public InfluenceType? InfluenceType { get; set; }

    [ForeignKey(nameof(WeaponId))]
    public WeaponEquipment? Weapon { get; set; }
}
