using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using YourDarkSoulsAssistant.UsersService.Infrastructure.Context;
using YourDarkSoulsAssistant.UsersService.Interfaces.Identity;
using YourDarkSoulsAssistant.UsersService.Models;
using YourDarkSoulsAssistant.UsersService.Services;
using YourDarkSoulsAssistant.UsersService.Validators;

namespace YourDarkSoulsAssistant.UsersService.Extensions;

public static class ProgramExtensions
{
    public static void AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 12;
            
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._+";
            
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<UserDBContext>()
            .AddDefaultTokenProviders();
    }
    
    public static void AppServicesRegistration(this IServiceCollection services)
    {
        services
            .AddAutoMapper(config => { config.AddProfile<UserMappingProfile>(); })
            .AddHostedService<GarbageCollectorService>()
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<ITokenBlacklistService, TokenBlacklistService>();
    }
    
    public static void AddValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateRoleRequestValidator>();
    }
}
