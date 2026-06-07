using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourDarkSoulsAssistant.CatalogService.Models.Equipments;

namespace YourDarkSoulsAssistant.CatalogService.Infrastructure.Configs.Equipments;

public class EquipmentScalingMapConfig : IEntityTypeConfiguration<EquipmentScalingMap>
{
    public void Configure(EntityTypeBuilder<EquipmentScalingMap> builder)
    {
        builder.HasKey(x => new { x.EquipmentId, x.ScalingId });

        builder.HasOne(x => x.Equipment)
            .WithMany(x => x.Scalings)
            .HasForeignKey(x => x.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Scaling)
            .WithMany(x => x.EquipmentScalings)
            .HasForeignKey(x => x.ScalingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
