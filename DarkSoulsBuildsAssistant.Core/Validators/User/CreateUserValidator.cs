// Файл: Validators/User/CreateUserDtoValidator.cs
using FluentValidation;
using DarkSoulsBuildsAssistant.Core.DTOs;

namespace DarkSoulsBuildsAssistant.Core.Validators.User;

// Успадковуємося від AbstractValidator і вказуємо, який DTO ми перевіряємо
public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        // Правило для імені: не порожнє, мінімум 2 символи, максимум 50
        RuleFor(user => user.Name)
            .NotEmpty().WithMessage("Ім'я є обов'язковим.")
            .Length(2, 50).WithMessage("Ім'я має містити від 2 до 50 символів.");

        // Правило для Email: не порожнє і має бути дійсним форматом пошти
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Електронна пошта є обов'язковою.")
            .EmailAddress().WithMessage("Некоректний формат електронної пошти.");

        // Правило для пароля: не порожній, мінімум 8 символів
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Пароль є обов'язковим.")
            .MinimumLength(8).WithMessage("Пароль має містити щонайменше 8 символів.");
    }
}