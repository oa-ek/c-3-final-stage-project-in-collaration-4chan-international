using Microsoft.AspNetCore.Mvc.RazorPages;
using DarkSoulsBuildsAssistant.Core.DTOs.System;
using DarkSoulsBuildsAssistant.Core.Interfaces.Services.Identity;

namespace DarkSoulsBuildsAssistant.App.Pages.Admin;

public class UsersModel(IUserService userService) : PageModel
{
    private readonly IUserService _userService = userService;
    
    // Припустимо, сервіс повертає список UserDTO
    public IEnumerable<UserDTO> Users { get; set; } = new List<UserDTO>();

    public async Task OnGetAsync()
    {
        // Отримуємо список
        // (Якщо такого методу немає, створіть фейковий список для тестування дизайну)
        Users = await _userService.GetAllUsersAsync(); 
    }
}