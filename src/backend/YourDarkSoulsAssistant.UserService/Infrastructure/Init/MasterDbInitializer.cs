using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YourDarkSoulsAssistant.UserService.Infrastructure.Context;
using YourDarkSoulsAssistant.UserService.Models;

namespace YourDarkSoulsAssistant.UserService.Infrastructure.Init;

public static class MasterDbInitializer
{
    public static async Task RunAsync(string connectionString)
    {
        Console.WriteLine("--> [Master Mode]: Prepare services...");
        
        var services = new ServiceCollection();
        
        services.AddLogging(builder => builder.AddConsole());
        
        services.AddDbContext<UserDBContext>(options =>
            options.UseNpgsql(connectionString));
        
        services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<UserDBContext>()
            .AddDefaultTokenProviders();
        
        await using var serviceProvider = services.BuildServiceProvider();

        Console.WriteLine("--> [Master Mode]: Initializing database...");

        try
        {
            await DataInitializer.InitializeAsync(serviceProvider);
            Console.WriteLine("--> [Master Mode]: ✅ Database initialization completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> [Master Mode]: ❌ Error: {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"--> Details: {ex.InnerException.Message}");
        }
    }
}