using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourDarkSoulsAssistant.CatalogService.Models.Equipments;

namespace YourDarkSoulsAssistant.CatalogService.Infrastructure.Configs.Equipments;

public class ReinforceStepConfig : IEntityTypeConfiguration<ReinforceStep>
{
    public void Configure(EntityTypeBuilder<ReinforceStep> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Modifiers)
            .HasColumnType("jsonb")
            .IsRequired();
    }
}
