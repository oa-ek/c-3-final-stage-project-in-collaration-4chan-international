using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YourDarkSoulsAssistant.UserService.Models;

namespace YourDarkSoulsAssistant.UserService.Infrastructure.Context;

public class UserDBContext : IdentityDbContext<User, Role, Guid>
{
    public UserDBContext() { }

    public UserDBContext(DbContextOptions<UserDBContext> options)
        : base(options) { }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<RevokedToken> RevokedTokens { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<RevokedToken>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Token).IsRequired();
        });
        
        builder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<IdentityUserRole<Guid>>(
                role => role.HasOne<Role>().WithMany().HasForeignKey(ur => ur.RoleId).IsRequired(),
                user => user.HasOne<User>().WithMany().HasForeignKey(ur => ur.UserId).IsRequired(),
                join => join.ToTable("AspNetUserRoles")
            );
    }
}
