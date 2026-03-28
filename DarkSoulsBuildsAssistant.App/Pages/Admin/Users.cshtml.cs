using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DarkSoulsBuildsAssistant.Core.DTOs.System;
using DarkSoulsBuildsAssistant.Core.Interfaces.Services.Identity;

// Додай using для твого IUserService

namespace DarkSoulsBuildsAssistant.App.Pages.Admin;

public class UsersModel(IUserService userService) : PageModel
{
    // Припустимо, ти інжектиш свій сервіс тут

    public IEnumerable<UserDTO> UsersList { get; set; } = new List<UserDTO>();

    [BindProperty]
    public ManageUserDTO UserInput { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        // Завантажуємо список користувачів для відображення
        UsersList = await userService.GetAllUsersAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostSaveUserAsync()
    {
        if (!ModelState.IsValid)
        {
            UsersList = await userService.GetAllUsersAsync();
            return Page();
        }

        if (string.IsNullOrEmpty(UserInput.Id))
        {
            await userService.CreateUserAsync(UserInput);
        }
        else
        {
            await userService.UpdateUserAsync(UserInput);
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await userService.DeleteUserAsync(id.ToString());
        return RedirectToPage();
    }
}
