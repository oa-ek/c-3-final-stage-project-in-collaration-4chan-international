using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models_Context.Models;

public partial class ArmorInfluence
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int InfluenceId { get; set; }

    [ForeignKey("InfluenceTypeId")]
    public InfluenceType? Influence { get; set; }
    public int? InfluenceTypeId { get; set; }

    public double? Value { get; set; }

    [ForeignKey("ArmorId")]
    public Armor? Armor { get; set; }
    public int? ArmorId { get; set; }

}
