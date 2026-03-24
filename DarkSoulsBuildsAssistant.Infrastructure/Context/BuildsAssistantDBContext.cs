using DarkSoulsBuildsAssistant.Core.Entities.Character;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment.Armor;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment.Weapon;
using DarkSoulsBuildsAssistant.Core.Entities.System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DarkSoulsBuildsAssistant.Infrastructure.Context;

public partial class BuildsAssistantDbContext : IdentityDbContext<User, Role, int>
{
    public BuildsAssistantDbContext() { }

    public BuildsAssistantDbContext(DbContextOptions<BuildsAssistantDbContext> options)
        : base(options) { }

    public virtual DbSet<ArmorInfluence> ArmorInfluences { get; set; }

    public virtual DbSet<CharacterBuild> CharacterBuilds { get; set; }
    
    public virtual DbSet<Game> Games { get; set; }
    
    public virtual DbSet<BaseEquipment> Equipments { get; set; }

    
    public virtual DbSet<EquipmentType> EquipmentTypes { get; set; }

    public virtual DbSet<InfluenceType> InfluenceTypes { get; set; }
    
    public virtual DbSet<Set> Sets { get; set; }
    
    public virtual DbSet<Slot> Slots { get; set; }
    
    public virtual DbSet<Log> Logs { get; set; }
    
    public virtual DbSet<LogLevel> LogLevels { get; set; }
    
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    
    public virtual DbSet<RevokedToken> RevokedTokens { get; set; }
    
    public virtual DbSet<WeaponInfluence> WeaponInfluences { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // 1. Налаштування ієрархії для EquipmentType
        builder.Entity<EquipmentType>()
            .HasDiscriminator<string>("TypeDiscriminator")
            .HasValue<WeaponType>("WeaponType")
            .HasValue<ArmorType>("ArmorType");

        // 2. Налаштування ієрархії для BaseEquipment
        builder.Entity<BaseEquipment>()
            .HasDiscriminator<string>("EquipmentDiscriminator")
            .HasValue<WeaponEquipment>("Weapon")
            .HasValue<ArmorEquipment>("Armor");

        // 3. Зв'язок між BaseEquipment та EquipmentType
        builder.Entity<BaseEquipment>()
            .HasOne(e => e.EquipmentType)
            .WithMany()
            .HasForeignKey(e => e.EquipmentTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // 4. Зв'язок між EquipmentType та Slot
        builder.Entity<EquipmentType>()
            .HasOne(et => et.Slot)
            .WithMany()
            .HasForeignKey(et => et.SlotId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
