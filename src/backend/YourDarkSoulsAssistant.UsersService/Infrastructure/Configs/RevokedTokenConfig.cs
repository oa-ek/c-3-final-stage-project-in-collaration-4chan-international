using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourDarkSoulsAssistant.UsersService.Models;

namespace YourDarkSoulsAssistant.UsersService.Infrastructure.Configs;

public class RevokedTokenConfig: IEntityTypeConfiguration<RevokedToken>
{
    public void Configure(EntityTypeBuilder<RevokedToken> builder)
    {
        builder.ToTable("revoked_tokens");

        builder.Property(token => token.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();
        
        builder.HasKey(token => token.Id);
        
        builder.Property(token => token.TokenHash)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(token => token.ExpirationDate)
            .IsRequired();
    }
}
