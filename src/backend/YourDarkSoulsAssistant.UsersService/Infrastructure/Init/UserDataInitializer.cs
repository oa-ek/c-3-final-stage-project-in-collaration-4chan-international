using Microsoft.AspNetCore.Identity;
using YourDarkSoulsAssistant.Core.Infrastructure.Init;
using YourDarkSoulsAssistant.UsersService.Infrastructure.Context;
using YourDarkSoulsAssistant.UsersService.Models;

namespace YourDarkSoulsAssistant.UsersService.Infrastructure.Init;

public class UserDataInitializer(
    UserDBContext context,
    ILogger<UserDataInitializer> logger,
    IWebHostEnvironment env,
    UserManager<User> userManager,
    RoleManager<Role> roleManager)
    : BaseDataInitializer<UserDBContext>(context, logger, env)
{
    private static readonly string[] Roles = ["Admin", "User", "SystemApp"];
    
    protected override async Task SeedDataAsync()
    {
        Logger.LogInformation("--> [Seeding]: Перевірка наявності тестових ролей...");

        foreach (var roleName in Roles)
        {
            if (await roleManager.RoleExistsAsync(roleName)) continue;
                
            var result = await roleManager.CreateAsync(new Role { Name = roleName, Description = $"{roleName} role" });
                
            if (result.Succeeded)
            {
                Logger.LogInformation($"--> [Seeding]: ✅ Роль '{roleName}' створена.");
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"❌ Критична помилка створення ролі {roleName}: {errors}");
            }
        }
        
        Logger.LogInformation("--> [Seeding]: Перевірка наявності тестових користувачів...");

        var adminEmail = "admin@example.com";
        var userEmail = "test@dsassistant.com";
        
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            logger.LogInformation("--> [Seeding]: Створення користувача Admin...");
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

            await CreateAndConfigureUser(adminUser, "Admin_VeryStrong_123!", "Admin");
        }
        if (await userManager.FindByEmailAsync(userEmail) == null)
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
            
            await CreateAndConfigureUser(regularUser, "Test_VeryStrong_123!", "User");
        }
        else
        {
            Logger.LogInformation("--> [Seeding]: Дані вже існують, пропускаємо.");
        }
    }
    
    private async Task CreateAndConfigureUser(User user, string password, string role)
    {
        var result = await userManager.CreateAsync(user, password); 
            
        if (!result.Succeeded) 
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"❌ Помилка створення користувача {user.UserName}: {errors}");
        }
        
        Logger.LogInformation($"--> [Seeding]: ✅ {user.UserName} створений");
            
        var roleResult = await userManager.AddToRoleAsync(user, role);
            
        if (!roleResult.Succeeded) 
        {
            var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"❌ Помилка додавання {user.UserName} до ролі '{role}': {errors}");
        }
        
        Logger.LogInformation($"--> [Seeding]: ✅ {user.UserName} доданий до ролі '{role}'");
    }
}
