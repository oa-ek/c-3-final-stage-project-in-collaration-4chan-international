using Data.Models;
using Microsoft.AspNetCore.Identity;
using Services.Interfaces;
using Shared.Dtos;
using System.Security.Claims;

namespace Services.Contracts;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserDto> GetUserProfileAsync(ClaimsPrincipal userPrincipal)
    {
        // Знаходимо юзера за ID з токена
        var user = await _userManager.GetUserAsync(userPrincipal);
        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);

        return new UserDto
        {
            Id = user.Id.ToString(),
            UserName = user.UserName,
            Email = user.Email,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumber = user.PhoneNumber,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            FullName = user.FullName,
            Address = user.Address,
            DateOfBirth = user.BirthDate,
            ReaderTicketNumber = user.ReaderTicketNumber,
            Role = roles.FirstOrDefault() ?? "Reader"
        };
    }

    public async Task<bool> UpdateUserProfileAsync(ClaimsPrincipal userPrincipal, UserDto model)
    {
        var user = await _userManager.GetUserAsync(userPrincipal);
        if (user == null) return false;

        // Оновлюємо поля
        user.FullName = model.FullName;
        user.Address = model.Address;
        user.PhoneNumber = model.PhoneNumber;
        user.BirthDate = model.DateOfBirth;

        // ReaderTicketNumber зазвичай змінює тільки адмін, але для простоти дозволимо і тут, 
        // або можна закоментувати, щоб юзер не міг сам собі придумати квиток.
        // user.ReaderTicketNumber = model.ReaderTicketNumber; 

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }
}