using DarkSoulsBuildsAssistant.Core.Interfaces.Services.Identity;

namespace YourDarkSoulsAssistant.App.Services;

public static class Program
{
    public static void AppServicesRegistration(this IServiceCollection services)
    {
        services
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<ITokenBlacklistService, TokenBlacklistService>();
    }
}