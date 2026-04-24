using Microsoft.EntityFrameworkCore;
using YourDarkSoulsAssistant.ContentDeliveryService.Models;

namespace YourDarkSoulsAssistant.ContentDeliveryService.Infrastructure.Context;

public class ContentDeliveryDBContext : DbContext
{
    public ContentDeliveryDBContext() { }

    public ContentDeliveryDBContext(DbContextOptions<ContentDeliveryDBContext> options)
        : base(options) { }
    
    public DbSet<ContentItem> ContentItems { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ContentItem>()
            .HasIndex(i => i.PublicRoute)
            .IsUnique();
    }
}
