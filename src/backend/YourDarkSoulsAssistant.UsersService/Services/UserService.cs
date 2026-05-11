using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YourDarkSoulsAssistant.Core.DTOs;
using YourDarkSoulsAssistant.UsersService.DTOs.Roles;
using YourDarkSoulsAssistant.UsersService.DTOs.Users;
using YourDarkSoulsAssistant.UsersService.Interfaces.Identity;
using YourDarkSoulsAssistant.UsersService.Models;

namespace YourDarkSoulsAssistant.UsersService.Services;

public class UserService(
    UserManager<User> userManager, 
    RoleManager<Role> roleManager, 
    IMapper mapper,
    ILogger<UserService> logger) : IUserService
{
    public async Task<UserResponseDTO?> GetUserProfileAsync(ClaimsPrincipal userPrincipal)
    {
        var userId = userManager.GetUserId(userPrincipal);
        
        if (string.IsNullOrEmpty(userId)) return null;
        
        var userDto = await userManager.Users
            .Where(u => u.Id.ToString() == userId)
            .ProjectTo<UserResponseDTO>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return userDto;
    }

    public async Task<IEnumerable<SmallUserResponseDTO>> GetAllUsersAsync()
    {
        var userDTOs = await userManager.Users
            .ProjectTo<SmallUserResponseDTO>(mapper.ConfigurationProvider)
            .ToListAsync();

        return userDTOs;
    }
    
    public async Task<IEnumerable<RoleResponseDTO>> GetAllRolesAsync()
    {
        var roles = await roleManager.Roles
            .ProjectTo<RoleResponseDTO>(mapper.ConfigurationProvider)
            .ToListAsync();

        return roles;
    }
    
    public async Task<HTTPResult<User>> CreateUserAsync(CreateUserRequestDTO model)
    {
        var user = new User
        {
            UserName = model.UserName,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        try
        {
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(" ", result.Errors.Select(e => e.Description));
                return HTTPResult<User>.Failure(errors);
            }

            await userManager.AddToRoleAsync(user, "User");
            
            return HTTPResult<User>.Success(user);
        }
        catch (Exception e)
        {
            logger.LogError(e, "--> [UserService]: Critical error during user registration.");
            return HTTPResult<User>.Failure("Внутрішня помилка сервера. Спробуйте пізніше.");
        }
    }

    public async Task<HTTPResult<UserResponseDTO>> UpdateUserProfileAsync(ClaimsPrincipal userPrincipal, UpdateUserRequestDTO model)
    {
        var userId = userManager.GetUserId(userPrincipal);
        var user = await userManager.GetUserAsync(userPrincipal);
    
        if (user == null) 
        {
            logger.LogWarning("--> [UserService]: Спроба оновити профіль неіснуючого користувача (ID: {UserId})", userId);
            return HTTPResult<UserResponseDTO>.Failure("Користувача не знайдено.");
        }
        
        mapper.Map(model, user);

        var result = await userManager.UpdateAsync(user);
    
        if (!result.Succeeded)
        {
            var errors = string.Join(" ", result.Errors.Select(e => e.Description));
            logger.LogWarning("--> [UserService]: Помилка оновлення профілю {UserId}: {Errors}", userId, errors);
            return HTTPResult<UserResponseDTO>.Failure(errors);
        }

        logger.LogInformation("--> [UserService]: ✅ Профіль користувача {UserId} успішно оновлено.", userId);
        
        var responseDto = mapper.Map<UserResponseDTO>(user);
        return HTTPResult<UserResponseDTO>.Success(responseDto);
    }

    public async Task<HTTPResult<bool>> UpdateUserRoleAsync(Guid id, IEnumerable<string> roles)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
    
        if (user == null)
        {
            logger.LogWarning("--> [UserService]: Спроба змінити ролі для неіснуючого користувача {UserId}", id);
            return HTTPResult<bool>.Failure("Користувача не знайдено.");
        }

        logger.LogInformation("--> [UserService]: Початок оновлення ролей для користувача {UserId}", id);
    
        try
        {
            var userRoles = await userManager.GetRolesAsync(user);
            var rolesToAdd = roles.Except(userRoles).ToList();
            var rolesToRemove = userRoles.Except(roles).ToList();
            
            if (rolesToRemove.Any())
            {
                var removeResult = await userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded) throw new Exception("Не вдалося видалити старі ролі.");
            }
            
            if (rolesToAdd.Any())
            {
                var addResult = await userManager.AddToRolesAsync(user, rolesToAdd);
                if (!addResult.Succeeded) throw new Exception("Не вдалося додати нові ролі.");
            }
        
            logger.LogInformation("--> [UserService]: ✅ Ролі користувача {UserId} успішно оновлено.", id);
            return HTTPResult<bool>.Success(true);
        }
        catch (Exception e)
        {
            logger.LogError(e, "--> [UserService]: ❌ Помилка під час оновлення ролей для {UserId}", id);
            return HTTPResult<bool>.Failure("Помилка при оновленні ролей.");
        }
    }
    
    public async Task DeleteUserAsync(ClaimsPrincipal userPrincipal)
    {
        var user = await userManager.GetUserAsync(userPrincipal);
        
        if (user != null) 
            await userManager.DeleteAsync(user);
    }
}
