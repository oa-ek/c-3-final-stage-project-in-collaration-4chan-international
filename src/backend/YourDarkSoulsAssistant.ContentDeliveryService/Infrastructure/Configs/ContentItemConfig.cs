using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourDarkSoulsAssistant.ContentDeliveryService.Models;

namespace YourDarkSoulsAssistant.ContentDeliveryService.Infrastructure.Configs;

public class ContentItemConfig : IEntityTypeConfiguration<ContentItem>
{
    public void Configure(EntityTypeBuilder<ContentItem> builder)
    {
        builder.ToTable("content_items");
        
        builder.Property(i => i.Id)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");
        
        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(i => i.PublicRoute)
            .IsRequired()
            .HasMaxLength(1024);
            
        builder
            .HasIndex(i => i.PublicRoute)
            .IsUnique();
        
        builder.Property(i => i.PrivateRoute)
            .IsRequired()
            .HasMaxLength(1024);
    }
}
