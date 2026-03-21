using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models_Context.Models;

public partial class WeaponIfnluence
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int InfluenceId { get; set; }

    [ForeignKey("InfluenceTypeId")]
    public InfluenceType? InfluenceType { get; set; }
    public int? InfluenceTypeId { get; set; }

    public double? Value { get; set; }

    [ForeignKey("WeaponId")]
    public Weapon? Weapon { get; set; }
    public int? WeaponId { get; set; }
    
}
