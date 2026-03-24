using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using Data.Models;
using Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using Shared.DTO.Auth;

namespace Services.Contracts;

public class AuthService(UserManager<User> userManager, IConfiguration configuration, LibrarySystemDbContext context)
    : IAuthService
{
    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto model)
    {
        // 1. Шукаємо користувача
        var user = await userManager.FindByEmailAsync(model.Email);

        // 2. Перевіряємо пароль
        if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
            return new AuthResponseDto { IsSuccess = false, ErrorMessage = "Невірний логін або пароль" };

        // 3. Генеруємо токен
        var accessToken = await GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();
        
        await SaveRefreshTokenAsync(user.Id, refreshToken);

        // 4. ОТРИМУЄМО РОЛІ (Виправлення)
        // User не містить ролей, їх треба запросити у менеджера
        var roles = await userManager.GetRolesAsync(user);
        
        return new AuthResponseDto 
        { 
            IsSuccess = true, 
            AccessToken = accessToken, 
            RefreshToken = refreshToken.Token,
            UserName = user.UserName,
            Role = roles.FirstOrDefault() 
        };
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto model)
    {
        // Перевірка, чи існує вже такий користувач
        var existingUser = await userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
            return new AuthResponseDto { IsSuccess = false, ErrorMessage = "Користувач з таким Email вже існує" };

        var user = new User
        {
            Email = model.Email,
            // UserName обов'язковий в Identity. Часто використовують email як логін.
            UserName = model.Email,
            FullName = model.FullName,
            // 1. ✅ ПЕРЕДАЄМО АДРЕСУ
            // Тепер база буде задоволена, бо ми передаємо реальний рядок
            Address = model.Address,

            // 2. ✅ ЛОГІКА КВИТКА
            // Якщо в DTO прийшов номер (наприклад, реєструє бібліотекар) -> беремо його.
            // Якщо null (юзер сам реєструється) -> генеруємо унікальний номер.
            ReaderTicketNumber = model.ReaderTicketNumber
                                 ?? GenerateReaderTicket()
        };

        var result = await userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthResponseDto { IsSuccess = false, ErrorMessage = errors };
        }

        // Додаємо роль за замовчуванням
        // Важливо: Роль "Reader" повинна існувати в таблиці Roles (DbInitializer це робить)
        await userManager.AddToRoleAsync(user, "Reader");

        return new AuthResponseDto { IsSuccess = true };
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto model)
    {
        // 1. Шукаємо Refresh Token у БД
        var storedRefreshToken = await context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == model.RefreshToken);

        // 2. Валідація
        if (storedRefreshToken == null)
            return new AuthResponseDto { IsSuccess = false, ErrorMessage = "Token not found" };

        if (storedRefreshToken.Expires < DateTime.UtcNow)
            return new AuthResponseDto { IsSuccess = false, ErrorMessage = "Token expired" };

        if (storedRefreshToken.IsRevoked)
            return new AuthResponseDto { IsSuccess = false, ErrorMessage = "Token revoked" };

        // 3. (Опціонально) Можна перевірити валідність старого AccessToken, 
        // але зазвичай він вже протух, тому ми просто віримо RefreshToken.

        // 4. "Спалюємо" старий токен (щоб його не використали двічі)
        storedRefreshToken.IsRevoked = true;
        context.RefreshTokens.Update(storedRefreshToken);

        // 5. Генеруємо нову пару
        var user = storedRefreshToken.User;
        var newAccessToken = await GenerateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken();

        // 6. Зберігаємо новий
        await SaveRefreshTokenAsync(user.Id, newRefreshToken);
        await context.SaveChangesAsync(); // Зберігаємо зміни (і ревок, і новий)

        return new AuthResponseDto 
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
            // Identity використовує ClaimTypes.NameIdentifier для збереження ID
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Додаємо ролі в токен
        foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

        // Отримуємо ключ. 
        // Зверни увагу: ми використовуємо Environment Variables, які ASP.NET мапить у конфіг.
        // Змінна оточення: Jwt__Key
        // У коді: config["Jwt:Key"]
        var keyString = configuration["Jwt:Key"];
        // Подвійна перевірка (хоча Program.cs вже перевірив)
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
            Created = DateTime.UtcNow
        };
    }

    // Допоміжний метод генерації (можна зробити красивішим)
    private string GenerateReaderTicket()
    {
        // Наприклад: R-20231212-ABCD
        return $"R-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
    }

    private async Task SaveRefreshTokenAsync(int userId, RefreshToken token)
    {
        token.UserId = userId;
        await context.RefreshTokens.AddAsync(token);
        await context.SaveChangesAsync();
    }
}