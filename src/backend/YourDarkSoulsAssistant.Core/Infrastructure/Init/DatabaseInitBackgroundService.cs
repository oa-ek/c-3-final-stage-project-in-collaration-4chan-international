using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YourDarkSoulsAssistant.Core.Interfaces.Infrastructure.Init;

namespace YourDarkSoulsAssistant.Core.Infrastructure.Init;

public class DatabaseInitBackgroundService(IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = scopeFactory.CreateScope();
        var initializer = scope.ServiceProvider.GetService<IDataInitializer>();
        
        if (initializer != null) await initializer.InitializeAsync(stoppingToken);
    }
}