using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace YourDarkSoulsAssistant.Core.Infrastructure.Init;

public class DatabaseInitHealthCheck<TContext> : IHealthCheck where TContext : DbContext
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        if (BaseDataInitializer<TContext>.IsDatabaseReady)
        {
            return Task.FromResult(HealthCheckResult.Healthy($"БД {typeof(TContext).Name} ініціалізована та готова."));
        }

        return Task.FromResult(HealthCheckResult.Unhealthy($"Очікування застосування міграцій для {typeof(TContext).Name}..."));
    }
}