using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourDarkSoulsAssistant.CatalogService.Models.Equipments;

namespace YourDarkSoulsAssistant.CatalogService.Infrastructure.Configs.Equipments;

public class InfluenceConfig : IEntityTypeConfiguration<Influence>
{
    public void Configure(EntityTypeBuilder<Influence> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Value)
            .HasColumnType("decimal(10,2)");

        builder.Property(x => x.IconPath)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasOne(x => x.InfluenceType)
            .WithMany()
            .HasForeignKey(x => x.InfluenceTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Operation)
            .WithMany()
            .HasForeignKey(x => x.OperationTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Game)
            .WithMany()
            .HasForeignKey(x => x.GameId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
