using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YourDarkSoulsAssistant.Core.Interfaces.Infrastructure.Init;

namespace YourDarkSoulsAssistant.Core.Extensions;

public static class DatabaseConfiguration
{
    /// <summary>
    /// Отримує та валідує рядок підключення з конфігурації.
    /// Повертає рядок замість збереження у статичний стан.
    /// </summary>
    private static string GetAndValidateConnectionString(IConfiguration config, string connectionStringName)
    {
        // Виправлено баг: прибрано лапки навколо connectionStringName
        var connectionString = config.GetConnectionString(connectionStringName);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "❌ Critical Error: There are missing variables\nCheck:\n" +
                $" - ConnectionStrings:{connectionStringName}\n");
        }

        return connectionString;
    }
    
    /// <summary>
    /// Універсальний метод для реєстрації будь-якого DbContext у DI-контейнері.
    /// </summary>
    /// <typeparam name="TContext">Тип вашого контексту бази даних (має успадковувати DbContext)</typeparam>
    public static IServiceCollection AddDatabase<TContext>(
        this IServiceCollection services, 
        IConfiguration config, 
        string connectionStringName = "DefaultConnection") where TContext : DbContext
    {
        // Отримуємо безпечний та перевірений рядок підключення
        string connectionString = GetAndValidateConnectionString(config, connectionStringName);
        
        // Реєструємо переданий тип контексту з PostgreSQL
        services.AddDbContext<TContext>(options =>
            options.UseNpgsql(connectionString));

        // Повертаємо services для підтримки ланцюжкових викликів
        return services;
    }
    
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        // 1. Створюємо ізольовану область видимості (Scope). 
        // Це критично важливо, адже DbContext не можна викликати глобально (Singleton).
        using var scope = app.Services.CreateAsyncScope();
        
        // 2. Просимо DI-контейнер дати нам ініціалізатор, специфічний для поточного сервісу
        var dataInitializer = scope.ServiceProvider.GetService<IDataInitializer>();

        // 3. Якщо ініціалізатор зареєстровано, запускаємо його
        if (dataInitializer != null)
        {
            await dataInitializer.InitializeAsync();
        }
        else
        {
            // Якщо для якогось мікросервісу ініціалізація не потрібна, 
            // він просто не зареєструє IDataInitializer і цей код нічого не зламає.
            // Можна додати логування (ILogger), що ініціалізатор пропущено.
        }
    }
}