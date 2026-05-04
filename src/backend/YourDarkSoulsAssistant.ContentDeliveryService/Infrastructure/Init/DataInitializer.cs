using Microsoft.EntityFrameworkCore;
using YourDarkSoulsAssistant.ContentDeliveryService.Infrastructure.Context;
using YourDarkSoulsAssistant.Core.Interfaces.Infrastructure.Init;

namespace YourDarkSoulsAssistant.ContentDeliveryService.Infrastructure.Init;

/// <summary>
/// Відповідає за застосування міграцій та початкове наповнення бази даних контенту.
/// </summary>
public class DataInitializer(ContentDeliveryDBContext context, ILogger<DataInitializer> logger)
    : IDataInitializer
{
    // Оголошуємо приватні змінні для зберігання наших залежностей
    private readonly ContentDeliveryDBContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly ILogger<DataInitializer> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    // Конструктор: DI-контейнер автоматично передасть сюди потрібні об'єкти

    /// <summary>
    /// Запускає процес ініціалізації.
    /// </summary>
    public async Task InitializeAsync()
    {
        try
        {
            _logger.LogInformation("--> [Seeding]: Початок ініціалізації бази даних...");
            _logger.LogInformation("--> [Seeding]: Перевірка та застосування міграцій...");
            
            // Використовуємо контекст напряму. Ніяких зайвих scope створювати не треба!
            await _context.Database.MigrateAsync();
            
            _logger.LogInformation("--> [Seeding]: ✅ Міграції успішно застосовані.");
            
            // Тут у майбутньому можна додати виклик методу для наповнення даними (Seeding)
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "--> [Seeding]: ❌ КРИТИЧНА ПОМИЛКА під час ініціалізації!");
            
            // Прокидаємо помилку далі, щоб додаток зупинився, якщо БД не готова
            throw;
        }
    }
}