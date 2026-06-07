using Microsoft.EntityFrameworkCore;
using YourDarkSoulsAssistant.CatalogService.Models;
using YourDarkSoulsAssistant.CatalogService.Models.Equipments;

namespace YourDarkSoulsAssistant.CatalogService.Infrastructure.Context;

public class CatalogDBContext : DbContext
{
    public CatalogDBContext() { }

    public CatalogDBContext(DbContextOptions<CatalogDBContext> options) : base(options) { }

    public DbSet<Game> Games { get; set; } = null!;
    public DbSet<Slot> Slots { get; set; } = null!;
    public DbSet<Operation> Operations { get; set; } = null!;
    public DbSet<Equipment> Equipments { get; set; } = null!;
    public DbSet<EquipmentType> EquipmentTypes { get; set; } = null!;
    public DbSet<Influence> Influences { get; set; } = null!;
    public DbSet<InfluenceType> InfluenceTypes { get; set; } = null!;
    public DbSet<ReinforceSchema> ReinforceSchemas { get; set; } = null!;
    public DbSet<ReinforceStep> ReinforceSteps { get; set; } = null!;
    public DbSet<ReqAttribute> ReqAttributes { get; set; } = null!;
    public DbSet<EquipmentScaling> EquipmentScalings { get; set; } = null!;
    public DbSet<EquipmentScalingMap> EquipmentScalingsMap { get; set; } = null!;
    public DbSet<EquipmentInfluenceMap> EquipmentInfluencesMap { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDBContext).Assembly);
    }
}