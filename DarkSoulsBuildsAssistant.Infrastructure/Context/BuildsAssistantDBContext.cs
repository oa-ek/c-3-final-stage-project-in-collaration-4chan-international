using DarkSoulsBuildsAssistant.Core.Entities.Armor;
using DarkSoulsBuildsAssistant.Core.Entities.Character;
using DarkSoulsBuildsAssistant.Core.Entities.Etc;
using DarkSoulsBuildsAssistant.Core.Entities.System;
using DarkSoulsBuildsAssistant.Core.Entities.Weapon;

using Microsoft.EntityFrameworkCore;

namespace DarkSoulsBuildsAssistant.Infrastructure.Context;

public partial class BuildsAssistantDbContext : DbContext
{
    public BuildsAssistantDbContext()
    {
    }

    public BuildsAssistantDbContext(DbContextOptions<BuildsAssistantDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ArmorEquipment> Armors { get; set; }

    public virtual DbSet<ArmorInfluence> ArmorInfluences { get; set; }

    public virtual DbSet<ArmorType> ArmorTypes { get; set; }

    public virtual DbSet<CharacterBuild> CharacterBuilds { get; set; }
    
    public virtual DbSet<Game> Games { get; set; }
    
    public virtual DbSet<InfluenceType> InfluenceTypes { get; set; }
    
    public virtual DbSet<Set> Sets { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }
    
    public virtual DbSet<Log> Logs { get; set; }
    
    public virtual DbSet<LogLevel> LogLevels { get; set; }
    
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    
    public virtual DbSet<RevokedToken> RevokedTokens { get; set; }
    
    public virtual DbSet<Role> Roles { get; set; }
    
    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WeaponEquipment> Weapons { get; set; }

    public virtual DbSet<WeaponInfluence> WeaponInfluences { get; set; }

    public virtual DbSet<WeaponType> WeaponTypes { get; set; }
}
