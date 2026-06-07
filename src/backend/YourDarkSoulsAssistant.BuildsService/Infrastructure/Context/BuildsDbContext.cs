using Microsoft.EntityFrameworkCore;
using YourDarkSoulsAssistant.BuildsService.Models;

namespace YourDarkSoulsAssistant.BuildsService.Infrastructure.Context;

public class BuildsDbContext(DbContextOptions<BuildsDbContext> options) : DbContext(options)
{
    public DbSet<Character> Characters => Set<Character>();
    public DbSet<Build> Builds => Set<Build>();
    public DbSet<AttributeType> AttributeTypes => Set<AttributeType>();
    public DbSet<CharacterAttribute> CharacterAttributes => Set<CharacterAttribute>();
    public DbSet<EquipmentBuild> EquipmentBuilds => Set<EquipmentBuild>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Character>(entity =>
        {
            entity.ToTable("Characters");
            entity.HasKey(e => e.CharacterId);

            entity.Property(e => e.CharacterId).HasColumnName("character_id");
            entity.Property(e => e.CharacterName).HasColumnName("charact_name").HasMaxLength(255).IsRequired();
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.UserId).HasColumnName("user_id").HasMaxLength(128).IsRequired();

            entity.HasIndex(e => e.UserId);
        });

        modelBuilder.Entity<Build>(entity =>
        {
            entity.ToTable("Builds");
            entity.HasKey(e => e.SetId);

            entity.Property(e => e.SetId).HasColumnName("set_id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
            entity.Property(e => e.CharacterId).HasColumnName("character_id");

            entity.HasOne(d => d.Character)
                .WithMany(p => p.Builds)
                .HasForeignKey(d => d.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AttributeType>(entity =>
        {
            entity.ToTable("AttributeTypes");
            entity.HasKey(e => e.AttributeId);

            entity.Property(e => e.AttributeId).HasColumnName("attribute_id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
            entity.Property(e => e.GameId).HasColumnName("game_id");
        });

        modelBuilder.Entity<CharacterAttribute>(entity =>
        {
            entity.ToTable("Character_Attribute");

            entity.HasKey(e => new { e.CharacterId, e.AttributeId });

            entity.Property(e => e.CharacterId).HasColumnName("character_id");
            entity.Property(e => e.AttributeId).HasColumnName("attribute_id");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.Character)
                .WithMany(p => p.CharacterAttributes)
                .HasForeignKey(d => d.CharacterId);

            entity.HasOne(d => d.AttributeType)
                .WithMany(p => p.CharacterAttributes)
                .HasForeignKey(d => d.AttributeId);
        });

        modelBuilder.Entity<EquipmentBuild>(entity =>
        {
            entity.ToTable("Equipment_Build");

            entity.HasKey(e => new { e.EquipmentId, e.SetId });

            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity.Property(e => e.SetId).HasColumnName("set_id");

            entity.HasOne(d => d.Build)
                .WithMany(p => p.EquipmentBuilds)
                .HasForeignKey(d => d.SetId);
        });
    }
}
