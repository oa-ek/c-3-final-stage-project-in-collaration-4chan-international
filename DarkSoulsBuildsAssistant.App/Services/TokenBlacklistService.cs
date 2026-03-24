using System.Security.Cryptography;
using System.Text;
using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.Contracts;

public class TokenBlacklistService(LibrarySystemDbContext context) : ITokenBlacklistService
{
    public async Task BlacklistTokenAsync(string token, DateTime expirationDate)
    {
        var tokenHash = ComputeSha256Hash(token);
        var revokedToken = new RevokedToken
        {
            Token = tokenHash,
            ExpirationDate = expirationDate
        };

        await context.RevokedTokens.AddAsync(revokedToken);
        await context.SaveChangesAsync();
    }

    public async Task<bool> IsTokenBlacklistedAsync(string token)
    {
        var tokenHash = ComputeSha256Hash(token);
        // Перевіряємо, чи є такий токен у базі
        return await context.RevokedTokens.AnyAsync(t => t.Token == tokenHash);
    }

    private static string ComputeSha256Hash(string rawData)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return Convert.ToHexString(bytes);
        }
    }
}