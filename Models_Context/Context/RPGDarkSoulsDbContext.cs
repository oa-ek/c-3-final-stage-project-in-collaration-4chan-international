using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Models_Context.Models;

namespace Models_Context.Context;

public partial class RPGDarkSoulsDbContext : DbContext
{
    public RPGDarkSoulsDbContext()
    {
    }

    public RPGDarkSoulsDbContext(DbContextOptions<RPGDarkSoulsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Armor> Armors { get; set; }

    public virtual DbSet<ArmorInfluence> ArmorInfluences { get; set; }

    public virtual DbSet<ArmorType> ArmorTypes { get; set; }

    public virtual DbSet<Character> Characters { get; set; }


    public virtual DbSet<InfluenceType> InfluenceTypes { get; set; }


    public virtual DbSet<Set> Sets { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    public virtual DbSet<Weapon> Weapons { get; set; }

    public virtual DbSet<WeaponIfnluence> WeaponIfnluences { get; set; }

    public virtual DbSet<WeaponType> WeaponTypes { get; set; }
    public DbSet<CharacterBuild> CharacterBuilds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS01;Initial Catalog=RPGDarkSouls;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Command Timeout=0");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        base.OnModelCreating(modelBuilder);
    }
}
