using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourDarkSoulsAssistant.CatalogService.Models.Equipments;

namespace YourDarkSoulsAssistant.CatalogService.Infrastructure.Configs.Equipments;

public class EquipmentInfluenceMapConfig : IEntityTypeConfiguration<EquipmentInfluenceMap>
{
    public void Configure(EntityTypeBuilder<EquipmentInfluenceMap> builder)
    {
        builder.HasKey(x => new { x.EquipmentId, x.InfluenceId });

        builder.HasOne(x => x.Equipment)
            .WithMany(x => x.Influences)
            .HasForeignKey(x => x.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Influence)
            .WithMany(x => x.EquipmentInfluences)
            .HasForeignKey(x => x.InfluenceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
