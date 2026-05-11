using System.Security.Claims;
using YourDarkSoulsAssistant.Core.DTOs;
using YourDarkSoulsAssistant.UsersService.DTOs.Auth;
using YourDarkSoulsAssistant.UsersService.DTOs.Roles;
using YourDarkSoulsAssistant.UsersService.DTOs.Users;
using YourDarkSoulsAssistant.UsersService.Models;

namespace YourDarkSoulsAssistant.UsersService.Interfaces.Identity;

public interface IUserService
{
    Task<UserResponseDTO?> GetUserProfileAsync(ClaimsPrincipal userPrincipal);
    Task<IEnumerable<SmallUserResponseDTO>> GetAllUsersAsync();
    
    Task<IEnumerable<RoleResponseDTO>> GetAllRolesAsync();

    Task<HTTPResult<User>> CreateUserAsync(CreateUserRequestDTO model);

    Task<HTTPResult<UserResponseDTO>> UpdateUserProfileAsync(ClaimsPrincipal userPrincipal, UpdateUserRequestDTO model);

    Task<HTTPResult<bool>> UpdateUserRoleAsync(Guid id, IEnumerable<string> roles);
    
    Task DeleteUserAsync(ClaimsPrincipal userPrincipal);
}
