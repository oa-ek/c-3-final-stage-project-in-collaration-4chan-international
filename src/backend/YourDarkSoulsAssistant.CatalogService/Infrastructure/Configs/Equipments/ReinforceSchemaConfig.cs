using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourDarkSoulsAssistant.CatalogService.Models.Equipments;

namespace YourDarkSoulsAssistant.CatalogService.Infrastructure.Configs.Equipments;

public class ReinforceSchemaConfig : IEntityTypeConfiguration<ReinforceSchema>
{
    public void Configure(EntityTypeBuilder<ReinforceSchema> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasOne(x => x.Game)
            .WithMany()
            .HasForeignKey(x => x.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Steps)
            .WithOne()
            .HasForeignKey(x => x.SchemaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
