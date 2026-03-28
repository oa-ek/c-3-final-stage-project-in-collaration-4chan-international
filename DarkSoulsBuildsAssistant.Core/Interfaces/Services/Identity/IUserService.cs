using System.Security.Claims;
using DarkSoulsBuildsAssistant.Core.DTOs.System;

namespace DarkSoulsBuildsAssistant.Core.Interfaces.Services.Identity;

public interface IUserService
{
    Task<UserDTO> GetUserProfileAsync(ClaimsPrincipal userPrincipal);
    Task<bool> UpdateUserProfileAsync(ClaimsPrincipal userPrincipal, UserDTO model);
    
    // ДОДАЄМО НОВИЙ МЕТОД:
    Task<IEnumerable<UserDTO>> GetAllUsersAsync();
    Task DeleteUserAsync(string id);
    Task CreateUserAsync(ManageUserDTO userDto);
    Task UpdateUserAsync(ManageUserDTO userDto);
}