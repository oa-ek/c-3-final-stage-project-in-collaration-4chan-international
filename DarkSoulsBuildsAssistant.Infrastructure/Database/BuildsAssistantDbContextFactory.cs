using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DarkSoulsBuildsAssistant.Infrastructure.Context;

namespace DarkSoulsBuildsAssistant.Infrastructure.Database;

// Цей клас використовується ТІЛЬКИ під час виконання команд dotnet ef для міграцій
public class BuildsAssistantDbContextFactory : IDesignTimeDbContextFactory<BuildsAssistantDbContext>
{
    public BuildsAssistantDbContext CreateDbContext(string[] args)
    {
        // 1. Беремо рядок підключення адміністратора зі змінних оточення Windows
        var adminConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__AdminConnection");

        if (string.IsNullOrEmpty(adminConnectionString))
        {
            throw new Exception("Не знайдено змінну оточення ConnectionStrings__AdminConnection. Переконайся, що термінал чи IDE було перезапущено після її створення.");
        }

        // 2. Налаштовуємо параметри для офіційного провайдера Oracle
        var optionsBuilder = new DbContextOptionsBuilder<BuildsAssistantDbContext>();
        
        // Зверни увагу: в пакеті Oracle метод називається UseMySQL (з великими SQL)
        optionsBuilder.UseMySQL(adminConnectionString);

        // 3. Повертаємо контекст із правами адміністратора
        return new BuildsAssistantDbContext(optionsBuilder.Options);
    }
}
