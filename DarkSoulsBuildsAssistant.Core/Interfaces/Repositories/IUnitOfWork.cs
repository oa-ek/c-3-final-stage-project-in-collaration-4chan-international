namespace DarkSoulsBuildsAssistant.Core.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    // TODO: Add repositories
    
    Task CompleteAsync();
}
