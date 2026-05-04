namespace YourDarkSoulsAssistant.UsersService.Interfaces.Identity;

public interface ITokenBlacklistService
{
    Task BlacklistTokenAsync(string token, DateTime expirationDate);
    Task<bool> IsTokenBlacklistedAsync(string token);
}