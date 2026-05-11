namespace YourDarkSoulsAssistant.Core.Interfaces.Infrastructure.Init;

public interface IDataInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
}
