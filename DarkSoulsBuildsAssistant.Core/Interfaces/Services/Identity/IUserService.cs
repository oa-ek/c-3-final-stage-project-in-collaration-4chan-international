using DarkSoulsBuildsAssistant.Core.DTOs.System;
using System.Security.Claims;

namespace DarkSoulsBuildsAssistant.Core.Interfaces.Services.Identity;

public interface IUserService
{
    Task<UserDTO> GetUserProfileAsync(ClaimsPrincipal userPrincipal);

    // Оновити профіль
    Task<bool> UpdateUserProfileAsync(ClaimsPrincipal userPrincipal, UserDTO model);
}
