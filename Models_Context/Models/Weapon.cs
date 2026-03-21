using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models_Context.Models;

public partial class Weapon
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int WeaponId { get; set; }

    [ForeignKey("WeaponTypeId")]
    public WeaponType? WeaponType { get; set; }
    public int? WeaponTypeId { get; set; }

    public string? Name { get; set; }

    public decimal? Damage { get; set; }

    public decimal? Weight { get; set; }

    public int? ReqStrenght { get; set; }

    public int? ReqDexterity { get; set; }

    public int? ReqIntelligence { get; set; }

    public int? ReqFaith { get; set; }

    public string? IconPath { get; set; }
    public virtual ICollection<WeaponIfnluence> WeaponInfluences { get; set; } = new List<WeaponIfnluence>();

    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();
}
