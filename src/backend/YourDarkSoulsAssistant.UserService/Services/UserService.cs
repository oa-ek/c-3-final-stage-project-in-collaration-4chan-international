using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YourDarkSoulsAssistant.UserService.DTOs.Roles;
using YourDarkSoulsAssistant.UserService.DTOs.Users;
using YourDarkSoulsAssistant.UserService.Interfaces.Identity;
using YourDarkSoulsAssistant.UserService.Models;

namespace YourDarkSoulsAssistant.UserService.Services;

public class UserService(
    UserManager<User> userManager, 
    RoleManager<Role> roleManager, 
    IMapper mapper,
    IServiceProvider serviceProvider) : IUserService
{
    private readonly ILogger<UserService> _logger = serviceProvider.GetRequiredService<ILogger<UserService>>();
    
    public async Task<UserDTO?> GetUserProfileAsync(ClaimsPrincipal userPrincipal)
    {
        var userId = userManager.GetUserId(userPrincipal);
        
        if (string.IsNullOrEmpty(userId)) return null;
        
        var userDto = await userManager.Users
            .Where(u => u.Id.ToString() == userId)
            .ProjectTo<UserDTO>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return userDto;
    }

    public async Task<IEnumerable<SmallUserDTO>> GetAllUsersAsync()
    {
        var userDTOs = await userManager.Users
            .ProjectTo<SmallUserDTO>(mapper.ConfigurationProvider)
            .ToListAsync();

        return userDTOs;
    }
    
    public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
    {
        var roles = await roleManager.Roles
            .ProjectTo<RoleDTO>(mapper.ConfigurationProvider)
            .ToListAsync();

        return roles;
    }

    public async Task<bool> UpdateUserProfileAsync(ClaimsPrincipal userPrincipal, UpdateUserDTO model)
    {
        var user = await userManager.GetUserAsync(userPrincipal);
        if (user == null) return false;
        
        mapper.Map(model, user);

        var result = await userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> UpdateUserRoleAsync(Guid id, IEnumerable<string> roles)
    {
        var user = userManager.Users.FirstOrDefault(u => u.Id == id);
        
        if (user != null)
        {
            _logger.LogInformation("Updating user roles for user with ID {UserId}", id);
            try
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var rolesToAdd = roles.Except(userRoles);
                var rolesToRemove = userRoles.Except(roles);
                
                var result1 = await userManager.RemoveFromRolesAsync(user, rolesToRemove);
                var result2 = await userManager.AddToRolesAsync(user, rolesToAdd);
                
                return result1.Succeeded && result2.Succeeded;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating user roles for user with ID {UserId}", id);
                return false;
            }
        }
        return false;
    }

    public async Task DeleteUserAsync(ClaimsPrincipal userPrincipal)
    {
        var user = await userManager.GetUserAsync(userPrincipal);
        
        if (user != null) 
            await userManager.DeleteAsync(user);
    }
}
