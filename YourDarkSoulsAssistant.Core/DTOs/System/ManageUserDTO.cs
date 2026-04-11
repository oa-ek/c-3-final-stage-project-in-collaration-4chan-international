using System.ComponentModel.DataAnnotations;

namespace DarkSoulsBuildsAssistant.Core.DTOs.System;

public class ManageUserDTO
{
    public string? Id { get; set; } // null або пустий для нового користувача

    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? FirstName { get; set; }
        
    public string? LastName { get; set; }

    // Пароль необов'язковий при редагуванні (щоб не змінювати його, якщо поле пусте)
    public string? Password { get; set; }

    public bool IsAdmin { get; set; }
}
