using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourDarkSoulsAssistant.CatalogService.Models.Equipments;

namespace YourDarkSoulsAssistant.CatalogService.Infrastructure.Configs.Equipments;

public class ReqAttributeConfig : IEntityTypeConfiguration<ReqAttribute>
{
    public void Configure(EntityTypeBuilder<ReqAttribute> builder)
    {
        builder.HasKey(x => new { x.EquipmentId, x.AttributeId });

        builder.HasOne(x => x.Equipment)
            .WithMany(x => x.RequiredAttributes)
            .HasForeignKey(x => x.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.AttributeKey)
            .HasMaxLength(100)
            .IsRequired();
    }
}
