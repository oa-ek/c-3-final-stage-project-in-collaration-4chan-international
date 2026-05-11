using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using YourDarkSoulsAssistant.Core.DTOs;
using YourDarkSoulsAssistant.UsersService.DTOs.Auth;
using YourDarkSoulsAssistant.UsersService.Infrastructure.Context;
using YourDarkSoulsAssistant.UsersService.Interfaces.Identity;
using YourDarkSoulsAssistant.UsersService.Models;

namespace YourDarkSoulsAssistant.UsersService.Services;

public class AuthService(UserManager<User> userManager, IConfiguration configuration, UserDBContext context)
    : IAuthService
{
public async Task<HTTPResult<AuthSuccessResponseDTO>> LoginAsync(LoginRequestDTO model)
    {
        var user = model.IsEmail 
            ? await userManager.FindByEmailAsync(model.Login)
            : await userManager.FindByNameAsync(model.Login);
        
        if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
        {
            return HTTPResult<AuthSuccessResponseDTO>.Failure("Невірний логін або пароль.");
        }
        
        return await IssueTokensAsync(user, model.RememberMe);
    }

    public async Task<HTTPResult<AuthSuccessResponseDTO>> RefreshTokenAsync(RefreshTokenRequestDTO model)
    {
        var storedRefreshToken = await context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == model.RefreshToken);

        if (storedRefreshToken == null)
            return HTTPResult<AuthSuccessResponseDTO>.Failure("Токен не знайдено.");

        if (storedRefreshToken.ExpiresAt < DateTime.UtcNow)
            return HTTPResult<AuthSuccessResponseDTO>.Failure("Термін дії токена минув. Увійдіть знову.");
        
        if (storedRefreshToken.IsRevoked)
            return HTTPResult<AuthSuccessResponseDTO>.Failure("Токен був відкликаний. Можлива компрометація.");
        
        storedRefreshToken.IsRevoked = true;
        context.RefreshTokens.Update(storedRefreshToken);
        await context.SaveChangesAsync();

        bool wasRememberMe = (storedRefreshToken.ExpiresAt - storedRefreshToken.CreatedAt).TotalDays > 7;
        
        var user = storedRefreshToken.User;
        
        return await IssueTokensAsync(user, wasRememberMe);
    }

    public async Task<HTTPResult<AuthSuccessResponseDTO>> IssueTokensAsync(User user, bool rememberMe)
    {
        var accessToken = await GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken(user.Id, rememberMe);
        
        await SaveRefreshTokenAsync(refreshToken);
        
        var roles = await userManager.GetRolesAsync(user);

        return HTTPResult<AuthSuccessResponseDTO>.Success(
            new AuthSuccessResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                UserName = user.UserName,
                Role = roles
            }
        );
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

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            configuration["Jwt:Issuer"],
            configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private RefreshToken GenerateRefreshToken(Guid userId, bool rememberMe)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        
        var lifespanDays = rememberMe ? 30 : 1; 
        
        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            ExpiresAt = DateTime.UtcNow.AddDays(lifespanDays),
            CreatedAt = DateTime.UtcNow,
            UserId = userId,
            IsRevoked = false
        };
    }

    private async Task SaveRefreshTokenAsync(RefreshToken token)
    {
        await context.RefreshTokens.AddAsync(token);
        await context.SaveChangesAsync();
    }
}
