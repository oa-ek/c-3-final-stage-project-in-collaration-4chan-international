using YourDarkSoulsAssistant.UsersService.Interfaces.Identity;

namespace YourDarkSoulsAssistant.UsersService.Services;

public static class Program
{
    public static void AppServicesRegistration(this IServiceCollection services)
    {
        services
            .AddAutoMapper(config => { config.AddProfile<UserMappingProfile>(); })
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<ITokenBlacklistService, TokenBlacklistService>();
    }
}