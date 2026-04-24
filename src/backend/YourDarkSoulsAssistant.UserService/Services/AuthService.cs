using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using YourDarkSoulsAssistant.UserService.DTOs.Auth;
using YourDarkSoulsAssistant.UserService.Infrastructure.Context;
using YourDarkSoulsAssistant.UserService.Interfaces.Identity;
using YourDarkSoulsAssistant.UserService.Models;

namespace YourDarkSoulsAssistant.UserService.Services;

public class AuthService(UserManager<User> userManager, IConfiguration configuration, UserDBContext context)
    : IAuthService
{
    public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        
        if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
            return new AuthResponseDTO { IsSuccess = false, ErrorMessage = "Невірний логін або пароль" };
        
        return await DispatchToken(user);
    }

    public async Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO model)
    {
        var existingUser = await userManager.FindByEmailAsync(model.Email);
        
        if (existingUser != null)
            return new AuthResponseDTO { IsSuccess = false, ErrorMessage = "Користувач з таким Email вже існує" };

        if (model.Password != model.ConfirmPassword)
            return new AuthResponseDTO { IsSuccess = false, ErrorMessage = "Passwords do not match" };
        
        var user = new User
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserName = model.UserName,
            Email = model.Email,
            IsAdmin = false,
            JoinDate = DateTime.UtcNow,
            Level = 1,
            Covenant = "Default",
            NormalizedUserName = model.UserName.ToUpper(),
            NormalizedEmail = model.Email.ToUpper(),
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthResponseDTO { IsSuccess = false, ErrorMessage = errors };
        }
        
        await userManager.AddToRoleAsync(user, "User");

        return await DispatchToken(user);
    }

    public async Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO model)
    {
        var storedRefreshToken = await context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == model.RefreshToken);
        
        if (storedRefreshToken == null)
            return new AuthResponseDTO { IsSuccess = false, ErrorMessage = "Token not found" };

        if (storedRefreshToken.ExpiresAt < DateTime.UtcNow)
            return new AuthResponseDTO { IsSuccess = false, ErrorMessage = "Token expired" };

        if (storedRefreshToken.IsRevoked)
            return new AuthResponseDTO { IsSuccess = false, ErrorMessage = "Token revoked" };
        
        storedRefreshToken.IsRevoked = true;
        context.RefreshTokens.Update(storedRefreshToken);
        await context.SaveChangesAsync();
        
        var user = storedRefreshToken.User;
        return await DispatchToken(user);
    }
    
    private async Task<AuthResponseDTO> DispatchToken(User user)
    {
        var accessToken = await GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken(user.Id);
        
        await SaveRefreshTokenAsync(refreshToken);
        
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
    
    private RefreshToken GenerateRefreshToken(Guid userId)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        
        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };
    }

    private async Task SaveRefreshTokenAsync(RefreshToken token)
    {
        await context.RefreshTokens.AddAsync(token);
        await context.SaveChangesAsync();
    }
}
