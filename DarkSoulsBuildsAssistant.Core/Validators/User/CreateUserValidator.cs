using FluentValidation;
using DarkSoulsBuildsAssistant.Core.DTOs.Auth;

namespace DarkSoulsBuildsAssistant.Core.Validators.User;

public class RegisterRequestDTOValidator : AbstractValidator<RegisterRequestDTO>
{
    public RegisterRequestDTOValidator()
    {
        RuleFor(user => user.UserName)
            .NotEmpty().WithMessage("Ім'я користувача є обов'язковим.")
            .Length(2, 50).WithMessage("Ім'я має містити від 2 до 50 символів.");

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Електронна пошта є обов'язковою.")
            .EmailAddress().WithMessage("Некоректний формат електронної пошти.");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Пароль є обов'язковим.")
            .MinimumLength(8).WithMessage("Пароль має містити щонайменше 8 символів.");
    }
}