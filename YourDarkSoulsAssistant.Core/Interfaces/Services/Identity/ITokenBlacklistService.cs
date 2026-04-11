namespace DarkSoulsBuildsAssistant.Core.Interfaces.Services.Identity;

public interface ITokenBlacklistService
{
    Task BlacklistTokenAsync(string token, DateTime expirationDate);
    Task<bool> IsTokenBlacklistedAsync(string token);
}