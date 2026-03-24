using DarkSoulsBuildsAssistant.Core.DTOs.Auth;
using DarkSoulsBuildsAssistant.Core.Entities.System;
using DarkSoulsBuildsAssistant.Core.Interfaces.Services.Identity;
using DarkSoulsBuildsAssistant.Infrastructure.Context;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DarkSoulsBuildsAssistant.App.Services;

public class AuthService(UserManager<User> userManager, IConfiguration configuration, BuildsAssistantDbContext context)
    : IAuthService
{
    public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        
        if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
            return new AuthResponseDTO { IsSuccess = false, ErrorMessage = "Невірний логін або пароль" };
        
        var accessToken = await GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();
        
        await SaveRefreshTokenAsync(user.Id, refreshToken);
        
        var roles = await userManager.GetRolesAsync(user);
        
        return new AuthResponseDTO 
        { 
            IsSuccess = true, 
            AccessToken = accessToken, 
            RefreshToken = refreshToken.Token,
            UserName = user.UserName,
            Role = roles.FirstOrDefault() 
        };
    }

    public async Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO model)
    {
        var existingUser = await userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
            return new AuthResponseDTO { IsSuccess = false, ErrorMessage = "Користувач з таким Email вже існує" };

        var user = new User
        {
            Email = model.Email,
            NormalizedEmail = model.Email.ToUpper(),
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserName = model.UserName,
        };

        var result = await userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthResponseDTO { IsSuccess = false, ErrorMessage = errors };
        }
        
        await userManager.AddToRoleAsync(user, "User");

        return new AuthResponseDTO { IsSuccess = true };
    }

    public async Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO model)
    {
        var storedRefreshToken = await context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == model.RefreshToken);
        
        if (storedRefreshToken == null)
            return new AuthResponseDTO { IsSuccess = false, ErrorMessage = "Token not found" };

        if (storedRefreshToken.Expires < DateTime.UtcNow)
            return new AuthResponseDTO { IsSuccess = false, ErrorMessage = "Token expired" };

        if (storedRefreshToken.IsRevoked)
            return new AuthResponseDTO { IsSuccess = false, ErrorMessage = "Token revoked" };
        
        storedRefreshToken.IsRevoked = true;
        context.RefreshTokens.Update(storedRefreshToken);
        
        var user = storedRefreshToken.User;
        var newAccessToken = await GenerateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken();

        await SaveRefreshTokenAsync(user.Id, newRefreshToken);
        await context.SaveChangesAsync();

        return new AuthResponseDTO 
        { 
            IsSuccess = true, 
            AccessToken = newAccessToken, 
            RefreshToken = newRefreshToken.Token 
        };
    }

    private async Task<string> GenerateJwtToken(User user)
    {
        var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));
        
        var keyString = configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(keyString))
            throw new InvalidOperationException("JWT Key is missing in configuration.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            configuration["Jwt:Issuer"] ?? "LibraryServer",
            configuration["Jwt:Audience"] ?? "LibraryClient",
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        
        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            Expires = DateTime.UtcNow.AddDays(30),
            CreatedAt = DateTime.UtcNow
        };
    }

    private async Task SaveRefreshTokenAsync(int userId, RefreshToken token)
    {
        token.UserId = userId;
        await context.RefreshTokens.AddAsync(token);
        await context.SaveChangesAsync();
    }
}
