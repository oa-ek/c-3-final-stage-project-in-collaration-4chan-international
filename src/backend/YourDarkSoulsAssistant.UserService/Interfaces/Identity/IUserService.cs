using System.Security.Claims;
using YourDarkSoulsAssistant.UserService.DTOs.Roles;
using YourDarkSoulsAssistant.UserService.DTOs.Users;

namespace YourDarkSoulsAssistant.UserService.Interfaces.Identity;

public interface IUserService
{
    Task<UserDTO?> GetUserProfileAsync(ClaimsPrincipal userPrincipal);
    Task<IEnumerable<SmallUserDTO>> GetAllUsersAsync();
    
    Task<IEnumerable<RoleDTO>> GetAllRolesAsync();
    
    Task<bool> UpdateUserProfileAsync(ClaimsPrincipal userPrincipal, UpdateUserDTO model);
    
    Task<bool> UpdateUserRoleAsync(Guid Id, IEnumerable<string> roles);
    
    Task DeleteUserAsync(ClaimsPrincipal userPrincipal);
}
