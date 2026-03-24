using DarkSoulsBuildsAssistant.Core.Entities.System;
using DarkSoulsBuildsAssistant.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DarkSoulsBuildsAssistant.Infrastructure.Init;

public static class MasterDbInitializer
{
    public static async Task RunAsync(string adminConnectionString)
    {
        Console.WriteLine("--> [Master Mode]: Підготовка окремого контейнера...");

        // 1. Створюємо НОВУ, ЧИСТУ колекцію сервісів.
        // Це не впливає на основний додаток.
        var adminServices = new ServiceCollection();

        // 2. Налаштування логування (щоб бачити помилки Identity)
        adminServices.AddLogging(builder => builder.AddConsole());

        // 3. Підключаємо БД з АДМІНСЬКИМ рядком
        adminServices.AddDbContext<BuildsAssistantDbContext>(options =>
            options.UseMySQL(adminConnectionString));

        // 4. Підключаємо Identity (це підтягне UserManager, RoleManager, Hashing і т.д.)
        adminServices.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                // Інші налаштування паролів, такі ж як в Program.cs
            })
            .AddEntityFrameworkStores<BuildsAssistantDbContext>()
            .AddDefaultTokenProviders();

        // 5. Будуємо ТИМЧАСОВИЙ провайдер
        // Оскільки це локальна змінна, попередження аналізатора тут не повинно спрацьовувати,
        // або його можна безпечно ігнорувати.
        await using var adminServiceProvider = adminServices.BuildServiceProvider();

        Console.WriteLine("--> [Master Mode]: Контейнер готовий. Запуск ініціалізації...");

        try
        {
            // Викликаємо ваш стандартний ініціалізатор, передаючи йому цей спец-провайдер
            await DataInitializer.InitializeAsync(adminServiceProvider);
            Console.WriteLine("--> [Master Mode]: ✅ Успішно завершено.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> [Master Mode]: ❌ ПОМИЛКА: {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"--> Details: {ex.InnerException.Message}");
        }
    }
}