using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models_Context.Models;

public partial class Slot
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SlotId { get; set; }

    public string? Name { get; set; }

    public ArmorType? ArmorType { get; set; }
    public WeaponType? WeaponType { get; set; }
}
