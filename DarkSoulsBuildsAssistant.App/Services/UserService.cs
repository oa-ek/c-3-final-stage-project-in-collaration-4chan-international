using DarkSoulsBuildsAssistant.Core.DTOs.System;
using DarkSoulsBuildsAssistant.Core.Entities.System;
using DarkSoulsBuildsAssistant.Core.Interfaces.Services.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DarkSoulsBuildsAssistant.App.Services;

public class UserService(UserManager<User> userManager) : IUserService
{
    public async Task<UserDTO> GetUserProfileAsync(ClaimsPrincipal userPrincipal)
    {
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

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        user.NormalizedEmail = model.Email.ToUpper();
        user.UserName = model.UserName;
        user.NormalizedUserName = model.UserName.ToUpper();

        var result = await userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    // НОВИЙ МЕТОД ДЛЯ АДМІНКИ
    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
    {
        // Отримуємо всіх користувачів з БД
        var users = await userManager.Users.ToListAsync();
        var userDtos = new List<UserDTO>();

        foreach (var user in users)
        {
            // Для кожного користувача дістаємо його ролі
            var roles = await userManager.GetRolesAsync(user);
            
            userDtos.Add(new UserDTO
            {
                Id = user.Id.ToString(),
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = roles.ToList()
            });
        }

        return userDtos;
    }

    public async Task CreateUserAsync(ManageUserDTO userDto)
    {
        var user = new User
        {
            UserName = userDto.UserName,
            Email = userDto.Email,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName
        };
        
        // Створюємо користувача
        var result = await userManager.CreateAsync(user, userDto.Password ?? "DefaultPassword123!");
        
        if (result.Succeeded)
        {
            // Додаємо роль після успішного створення
            if (userDto.IsAdmin)
                await userManager.AddToRoleAsync(user, "Admin");
            else
                await userManager.AddToRoleAsync(user, "User");
        }
    }

    public async Task UpdateUserAsync(ManageUserDTO userDto)
    {
        if (string.IsNullOrEmpty(userDto.Id)) return;

        // 1. Знаходимо користувача в базі
        var user = await userManager.FindByIdAsync(userDto.Id);
        
        if (user == null) return; // Якщо не знайшли - виходимо

        // 2. Оновлюємо поля (тільки ті, що нам треба)
        user.UserName = userDto.UserName;
        user.Email = userDto.Email;
        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;

        // 3. Зберігаємо зміни
        await userManager.UpdateAsync(user);

        // 4. Оновлюємо пароль (якщо користувач щось ввів у форму)
        if (!string.IsNullOrEmpty(userDto.Password))
        {
            // Скидаємо старий пароль і ставимо новий
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            await userManager.ResetPasswordAsync(user, token, userDto.Password);
        }

        // 5. Оновлюємо ролі
        // Найпростіше: видалити старі ролі і додати нову
        var currentRoles = await userManager.GetRolesAsync(user);
        await userManager.RemoveFromRolesAsync(user, currentRoles);

        if (userDto.IsAdmin)
            await userManager.AddToRoleAsync(user, "Admin");
        else
            await userManager.AddToRoleAsync(user, "User");
    }

    // У DeleteUserAsync в тебе був int userId, але Identity використовує string для Id
    public async Task DeleteUserAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user != null)
        {
            await userManager.DeleteAsync(user);
        }
    }
}