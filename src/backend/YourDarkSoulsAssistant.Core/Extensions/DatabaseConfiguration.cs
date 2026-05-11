using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using YourDarkSoulsAssistant.Core.Infrastructure.Init;

namespace YourDarkSoulsAssistant.Core.Extensions;

public static class DatabaseConfiguration
{
private static string GetAndValidateConnectionString(IConfiguration config, string connectionStringName)
    {
        var connectionString = config.GetConnectionString(connectionStringName);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"❌ Critical Error: Missing Connection String: {connectionStringName}");
        }

        return connectionString;
    }

    public static IServiceCollection AddDatabase<TContext>(
        this IServiceCollection services, 
        IConfiguration config, 
        string connectionStringName) where TContext : DbContext
    {
        var connectionString = GetAndValidateConnectionString(config, connectionStringName);
        
        services.AddDbContext<TContext>(options =>
            options.UseNpgsql(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            }));
            
        services.AddHealthChecks()
            .AddNpgSql(connectionString, 
                name: $"{typeof(TContext).Name}-db-check", 
                failureStatus: HealthStatus.Unhealthy,
                tags: ["db", "postgresql"])
            .AddCheck<DatabaseInitHealthCheck<TContext>>(
                name: $"{typeof(TContext).Name}-init-check",
                failureStatus: HealthStatus.Unhealthy,
                tags: ["db", "init"]);
        
        services.AddHostedService<DatabaseInitBackgroundService>();

        return services;
    }
}
