using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourDarkSoulsAssistant.CatalogService.Models;

namespace YourDarkSoulsAssistant.CatalogService.Infrastructure.Configs;

public class OperationConfig : IEntityTypeConfiguration<Operation>
{
    public void Configure(EntityTypeBuilder<Operation> builder)
    {
        builder.HasKey(x => x.OperationId);

        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();
    }
}
