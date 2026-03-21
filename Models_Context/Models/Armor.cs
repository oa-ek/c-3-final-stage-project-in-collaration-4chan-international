using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models_Context.Models;

public partial class Armor
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ArmorId { get; set; }

    [ForeignKey("ArmorTypeId")]
    public ArmorType? ArmorType { get; set; }
    public int? ArmorTypeId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Weight { get; set; }

    public decimal? Poise { get; set; }

    public string? Skin { get; set; }
    public string? IconPath { get; set; }

    public virtual ICollection<ArmorInfluence> ArmorInfluences { get; set; } = new List<ArmorInfluence>();

    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();
}
