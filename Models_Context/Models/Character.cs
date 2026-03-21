using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models_Context.Models;

public partial class Character
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CharacterId { get; set; }

    public string? CharactName { get; set; }

    public int? Level { get; set; }

    public int? Vigor { get; set; }

    public int? Endurance { get; set; }

    public int? Strenght { get; set; }

    public int? Dexterity { get; set; }

    public int? Intelligence { get; set; }

    public int? Faith { get; set; }

    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();

    // Add these navigation properties if they exist in your model
    public virtual Weapon? RightHand { get; set; }

    public virtual Weapon? LeftHand { get; set; }

    public virtual Armor? Head { get; set; }

    public virtual Armor? Torso { get; set; }

    public virtual Armor? Hands { get; set; }

    public virtual Armor? Legs { get; set; }
}
