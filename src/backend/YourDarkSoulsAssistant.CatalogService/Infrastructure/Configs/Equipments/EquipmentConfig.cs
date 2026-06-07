using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourDarkSoulsAssistant.CatalogService.Models.Equipments;

namespace YourDarkSoulsAssistant.CatalogService.Infrastructure.Configs.Equipments;

public class EquipmentConfig : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.IconPath)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasOne(x => x.EquipmentType)
            .WithMany(x => x.Equipments)
            .HasForeignKey(x => x.EquipmentTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Slot)
            .WithMany(x => x.Equipments)
            .HasForeignKey(x => x.SlotId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ReinforceSchema)
            .WithMany(x => x.Equipments)
            .HasForeignKey(x => x.ReinforceSchemaId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Game)
            .WithMany(x => x.Equipments)
            .HasForeignKey(x => x.GameId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
