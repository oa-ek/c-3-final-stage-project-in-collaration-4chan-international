using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using YourDarkSoulsAssistant.Core.Interfaces.Infrastructure.Init;

namespace YourDarkSoulsAssistant.Core.Infrastructure.Init;

public abstract class BaseDataInitializer<TContext>(
    TContext context, 
    ILogger logger, 
    IWebHostEnvironment environment) : IDataInitializer
    where TContext : DbContext
{
    protected readonly TContext Context = context;
    protected readonly ILogger Logger = logger;
    protected readonly IWebHostEnvironment Environment = environment;

    // Глобальний прапорець для нашого Health Check
    public static bool IsDatabaseReady { get; private set; } = false;

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                Logger.LogInformation($"--> [Init]: Початок ініціалізації БД для {typeof(TContext).Name}...");

                var strategy = Context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    Logger.LogInformation($"--> [Init]: Застосування міграцій...");
                    await Context.Database.MigrateAsync(cancellationToken);
                });

                if (Environment.IsDevelopment())
                {
                    try 
                    {
                        Logger.LogInformation($"--> [Init]: Запуск наповнення тестовими даними...");
                        await SeedDataAsync();
                    }
                    catch(Exception seedEx)
                    {
                        Logger.LogWarning(seedEx, $"--> [Init]: ⚠️ Помилка тестових даних, але міграції успішні.");
                    }
                }

                // ВАЖЛИВО: Сигналізуємо системі, що база повністю готова до прийому трафіку!
                IsDatabaseReady = true;
                Logger.LogInformation($"--> [Init]: 🎉 БД {typeof(TContext).Name} успішно ініціалізована.");
                
                return; // Виходимо з нескінченного циклу
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"--> [Init]: ⚠️ БД недоступна або помилка міграції. Повтор через 10 секунд...");
                // Чекаємо 10 секунд і пробуємо знову, не вбиваючи сам мікросервіс
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
            }
        }
    }
    
    protected abstract Task SeedDataAsync();
}