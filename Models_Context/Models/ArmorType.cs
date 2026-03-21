using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models_Context.Models;

public partial class ArmorType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TypeId { get; set; }

    public string? Type { get; set; }

    [ForeignKey("SlotId")]
    public Slot? Slot { get; set; }
    public int? SlotId { get; set; }
}
