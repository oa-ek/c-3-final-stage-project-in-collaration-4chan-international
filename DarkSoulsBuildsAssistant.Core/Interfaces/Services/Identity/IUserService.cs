using Shared.Dtos;
using System.Security.Claims;

namespace Services.Interfaces;

public interface IUserService
{
    // Отримати профіль поточного юзера
    Task<UserDto> GetUserProfileAsync(ClaimsPrincipal userPrincipal);

    // Оновити профіль
    Task<bool> UpdateUserProfileAsync(ClaimsPrincipal userPrincipal, UserDto model);
}