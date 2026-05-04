using System.Security.Claims;
using YourDarkSoulsAssistant.UsersService.DTOs.Roles;
using YourDarkSoulsAssistant.UsersService.DTOs.Users;

namespace YourDarkSoulsAssistant.UsersService.Interfaces.Identity;

public interface IUserService
{
    Task<UserDTO?> GetUserProfileAsync(ClaimsPrincipal userPrincipal);
    Task<IEnumerable<SmallUserDTO>> GetAllUsersAsync();
    
    Task<IEnumerable<RoleDTO>> GetAllRolesAsync();
    
    Task<bool> UpdateUserProfileAsync(ClaimsPrincipal userPrincipal, UpdateUserDTO model);
    
    Task<bool> UpdateUserRoleAsync(Guid Id, IEnumerable<string> roles);
    
    Task DeleteUserAsync(ClaimsPrincipal userPrincipal);
}
