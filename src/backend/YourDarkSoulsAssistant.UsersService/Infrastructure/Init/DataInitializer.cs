using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YourDarkSoulsAssistant.Core.Interfaces.Infrastructure.Init;
using YourDarkSoulsAssistant.UsersService.Infrastructure.Context;
using YourDarkSoulsAssistant.UsersService.Models;

namespace YourDarkSoulsAssistant.UsersService.Infrastructure.Init;

public class DataInitializer(
    UserDBContext context,
    ILogger<DataInitializer> logger,
    RoleManager<Role> roleManager,
    UserManager<User> userManager) : IDataInitializer
{
    // Оголошуємо приватні змінні для зберігання наших залежностей
    private readonly UserDBContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly ILogger<DataInitializer> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly RoleManager<Role> _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
    private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    
    public async Task InitializeAsync()
    {
        try
        {
            _logger.LogInformation("--> [Seeding]: Початок ініціалізації бази даних...");
            
            _logger.LogInformation("--> [Seeding]: Перевірка та застосування міграцій...");
            await _context.Database.MigrateAsync();
            _logger.LogInformation("--> [Seeding]: ✅ Міграції успішно застосовані.");
            
            string[] roles = ["Admin", "User", "SystemApp"];
            foreach (var roleName in roles)
            {
                if (await _roleManager.RoleExistsAsync(roleName)) continue;
                
                var result = await _roleManager.CreateAsync(new Role { Name = roleName, Description = $"{roleName} role" });
                
                if (result.Succeeded)
                    _logger.LogInformation($"--> [Seeding]: ✅ Роль '{roleName}' створена.");
                else
                    LogErrors($"створення ролі {roleName}", result.Errors);
            }
            
            var adminEmail = "admin@example.com";
            if (await _userManager.FindByEmailAsync(adminEmail) == null)
            {
                _logger.LogInformation("--> [Seeding]: Створення користувача Admin...");
                var adminUser = new User
                {
                    FirstName = "Admin",
                    LastName = "System",
                    UserName = "admin",
                    Email = adminEmail,
                    JoinDate = DateTime.UtcNow,
                    Level = 1,
                    Covenant = "Default",
                    EmailConfirmed = true,
                };
                
                var result = await _userManager.CreateAsync(adminUser, "Admin_VeryStrong_123!"); 
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                   _logger.LogInformation("--> [Seeding]: ✅ Admin створений та призначений на роль.");
                }
                else
                {
                    LogErrors("створення Admin", result.Errors);
                }
            }
            
            var userEmail = "user@example.com";
            if (await _userManager.FindByEmailAsync(userEmail) == null)
            {
                var regularUser = new User 
                { 
                    FirstName = "Test", 
                    LastName = "User",
                    UserName = "tester", 
                    Email = userEmail, 
                    JoinDate = DateTime.UtcNow, 
                    Level = 1, 
                    Covenant = "Default",
                    EmailConfirmed = true,
                };

                var result = await _userManager.CreateAsync(regularUser, "User_VeryStrong_123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(regularUser, "User");
                   _logger.LogInformation("--> [Seeding]: ✅ Тестовий користувач створений.");
                }
                else
                {
                    LogErrors("створення User", result.Errors);
                }
            }

            _logger.LogInformation("--> [Seeding]: 🎉 Процес завершено.");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "--> [Seeding]: ❌ КРИТИЧНА ПОМИЛКА під час ініціалізації!");
            throw;
        }
    }
    
    private void LogErrors(string action, IEnumerable<IdentityError> errors)
    {
        _logger.LogError($"--> [Seeding]: ❌ Помилка під час {action}:");
        foreach (var error in errors)
        {
           _logger.LogError($"    - {error.Code}: {error.Description}");
        }
    }
}