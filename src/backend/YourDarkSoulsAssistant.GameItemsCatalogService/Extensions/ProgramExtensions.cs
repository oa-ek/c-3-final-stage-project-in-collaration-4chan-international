using YourDarkSoulsAssistant.GameItemsCatalogService.Services;

namespace YourDarkSoulsAssistant.GameItemsCatalogService.Extensions;

public static class ProgramExtensions
{
    public static void AddHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient<OutsideGameItemsService>()
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
            {
                ConnectTimeout = TimeSpan.FromMinutes(10),
            })
            .SetHandlerLifetime(Timeout.InfiniteTimeSpan);
    }
    
}
