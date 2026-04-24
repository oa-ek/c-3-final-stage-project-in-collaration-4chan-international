using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YourDarkSoulsAssistant.UserService.Infrastructure.Context;
using YourDarkSoulsAssistant.UserService.Models;

namespace YourDarkSoulsAssistant.UserService.Infrastructure.Init;

public static class DataInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        
        var logger = services.GetRequiredService<ILogger<UserDBContext>>();
        var context = services.GetRequiredService<UserDBContext>();
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<Role>>();

        try
        {
            logger.LogInformation("--> [Seeding]: Початок ініціалізації бази даних...");
            
            logger.LogInformation("--> [Seeding]: Перевірка та застосування міграцій...");
            await context.Database.MigrateAsync();
            logger.LogInformation("--> [Seeding]: ✅ Міграції успішно застосовані.");
            
            string[] roles = ["Admin", "User", "SystemApp"];
            foreach (var roleName in roles)
            {
                if (await roleManager.RoleExistsAsync(roleName)) continue;
                
                var result = await roleManager.CreateAsync(new Role { Name = roleName, Description = $"{roleName} role" });
                
                if (result.Succeeded)
                    logger.LogInformation($"--> [Seeding]: ✅ Роль '{roleName}' створена.");
                else
                    LogErrors(logger, $"створення ролі {roleName}", result.Errors);
            }
            
            var adminEmail = "admin@example.com";
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
                    IsAdmin = true,
                    EmailConfirmed = true,
                };
                
                var result = await userManager.CreateAsync(adminUser, "Admin_VeryStrong_123!"); 
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    logger.LogInformation("--> [Seeding]: ✅ Admin створений та призначений на роль.");
                }
                else
                {
                    LogErrors(logger, "створення Admin", result.Errors);
                }
            }
            
            var userEmail = "user@example.com";
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

                var result = await userManager.CreateAsync(regularUser, "User_VeryStrong_123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(regularUser, "User");
                    logger.LogInformation("--> [Seeding]: ✅ Тестовий користувач створений.");
                }
                else
                {
                    LogErrors(logger, "створення User", result.Errors);
                }
            }

            logger.LogInformation("--> [Seeding]: 🎉 Процес завершено.");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "--> [Seeding]: ❌ КРИТИЧНА ПОМИЛКА під час ініціалізації!");
            throw;
        }
    }
    
    private static void LogErrors(ILogger logger, string action, IEnumerable<IdentityError> errors)
    {
        logger.LogError($"--> [Seeding]: ❌ Помилка під час {action}:");
        foreach (var error in errors)
        {
            logger.LogError($"    - {error.Code}: {error.Description}");
        }
    }
}