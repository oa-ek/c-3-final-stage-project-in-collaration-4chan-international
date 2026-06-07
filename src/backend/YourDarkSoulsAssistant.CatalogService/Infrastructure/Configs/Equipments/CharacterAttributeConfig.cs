using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourDarkSoulsAssistant.CatalogService.Models.Equipments;

namespace YourDarkSoulsAssistant.CatalogService.Infrastructure.Configs.Equipments;

public class CharacterAttributeConfig : IEntityTypeConfiguration<CharacterAttribute>
{
    public void Configure(EntityTypeBuilder<CharacterAttribute> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.IconPath)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(x => new { x.GameId, x.Name })
            .IsUnique();
    }
}