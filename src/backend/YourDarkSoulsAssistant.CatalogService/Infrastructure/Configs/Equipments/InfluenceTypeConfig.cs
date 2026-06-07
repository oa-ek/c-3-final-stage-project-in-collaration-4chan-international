using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourDarkSoulsAssistant.CatalogService.Models.Equipments;

namespace YourDarkSoulsAssistant.CatalogService.Infrastructure.Configs.Equipments;

public class InfluenceTypeConfig : IEntityTypeConfiguration<InfluenceType>
{
    public void Configure(EntityTypeBuilder<InfluenceType> builder)
    {
        builder.HasKey(x => x.TypeId);

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();
    }
}
