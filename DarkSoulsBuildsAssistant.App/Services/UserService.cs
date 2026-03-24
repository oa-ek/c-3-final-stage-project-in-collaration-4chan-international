using DarkSoulsBuildsAssistant.Core.DTOs.System;
using DarkSoulsBuildsAssistant.Core.Entities.System;
using DarkSoulsBuildsAssistant.Core.Interfaces.Services.Identity;

using System.Security.Claims;

using Microsoft.AspNetCore.Identity;

namespace DarkSoulsBuildsAssistant.App.Services;

public class UserService(UserManager<User> userManager) : IUserService
{
    public async Task<UserDTO> GetUserProfileAsync(ClaimsPrincipal userPrincipal)
    {
        // Знаходимо юзера за ID з токена
        var user = await userManager.GetUserAsync(userPrincipal);
        if (user == null) return null;

        var roles = await userManager.GetRolesAsync(user);

        return new UserDTO
        {
            Id = user.Id.ToString(),
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Roles = roles.ToList()
        };
    }

    public async Task<bool> UpdateUserProfileAsync(ClaimsPrincipal userPrincipal, UserDTO model)
    {
        var user = await userManager.GetUserAsync(userPrincipal);
        if (user == null) return false;

        // Оновлюємо поля
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        user.NormalizedEmail = model.Email.ToUpper();
        user.UserName = model.UserName;
        user.NormalizedUserName = model.UserName.ToUpper();

        var result = await userManager.UpdateAsync(user);
        return result.Succeeded;
    }
}
