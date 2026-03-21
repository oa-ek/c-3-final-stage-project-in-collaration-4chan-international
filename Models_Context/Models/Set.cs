using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models_Context.Models;

public partial class Set
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SetId { get; set; }

    public string? Name { get; set; }

    [ForeignKey("CharacterId")]
    public Character? Character { get; set; }
    public int? CharacterId { get; set; }

    public virtual ICollection<Armor> Armors { get; set; } = new List<Armor>();

    public virtual ICollection<Weapon> Weapons { get; set; } = new List<Weapon>();
}
