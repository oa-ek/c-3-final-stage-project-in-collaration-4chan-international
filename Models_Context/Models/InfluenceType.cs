using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models_Context.Models;

public partial class InfluenceType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TypeId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<ArmorInfluence> ArmorInfluences { get; set; } = new List<ArmorInfluence>();

    public virtual ICollection<WeaponIfnluence> WeaponIfnluences { get; set; } = new List<WeaponIfnluence>();
}
