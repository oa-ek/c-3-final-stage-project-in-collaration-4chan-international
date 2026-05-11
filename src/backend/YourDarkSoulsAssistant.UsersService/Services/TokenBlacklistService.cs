using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using YourDarkSoulsAssistant.UsersService.Infrastructure.Context;
using YourDarkSoulsAssistant.UsersService.Interfaces.Identity;
using YourDarkSoulsAssistant.UsersService.Models;

namespace YourDarkSoulsAssistant.UsersService.Services;

public class TokenBlacklistService(UserDBContext context, IDistributedCache cache) : ITokenBlacklistService
{
    public async Task BlacklistTokenAsync(string token, DateTime expirationDate)
    {
        var tokenHash = ComputeSha256Hash(token);
        
        var revokedToken = new RevokedToken
        {
            TokenHash = tokenHash,
            ExpirationDate = expirationDate
        };

        await context.RevokedTokens.AddAsync(revokedToken);
        await context.SaveChangesAsync();
        
        var cacheOptions = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(expirationDate);
        
        await cache.SetStringAsync($"BL_{tokenHash}", "1", cacheOptions);
    }

    public async Task<bool> IsTokenBlacklistedAsync(string token)
    {
        var tokenHash = ComputeSha256Hash(token);
        var cacheKey = $"BL_{tokenHash}";
        
        var cachedValue = await cache.GetStringAsync(cacheKey);
        
        if (cachedValue != null)
        {
            return true;
        }
        
        var existsInDb = await context.RevokedTokens.AnyAsync(t => t.TokenHash == tokenHash);

        if (existsInDb)
        {
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));
                
            await cache.SetStringAsync(cacheKey, "1", options);
        }

        return existsInDb;
    }

    private static string ComputeSha256Hash(string rawData)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        return Convert.ToHexString(bytes);
    }
}
