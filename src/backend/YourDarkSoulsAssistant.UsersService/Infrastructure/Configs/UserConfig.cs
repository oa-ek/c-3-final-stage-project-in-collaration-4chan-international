using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourDarkSoulsAssistant.UsersService.Models;

namespace YourDarkSoulsAssistant.UsersService.Infrastructure.Configs;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.Property(u => u.AvatarPath)
            .IsRequired()
            .HasMaxLength(512)
            .HasDefaultValue("");

        builder.Property(u => u.Covenant)
            .IsRequired()
            .HasMaxLength(100)
            .HasDefaultValue("");
        
        builder.Property(u => u.Level)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(u => u.JoinDate)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<IdentityUserRole<Guid>>(
                role => role.HasOne<Role>().WithMany().HasForeignKey(ur => ur.RoleId), 
                user => user.HasOne<User>().WithMany().HasForeignKey(ur => ur.UserId), 
                join => 
                { 
                    join.HasKey(ur => new { ur.UserId, ur.RoleId }); 
                    join.ToTable("users_roles"); 
                }
                );
    }
}