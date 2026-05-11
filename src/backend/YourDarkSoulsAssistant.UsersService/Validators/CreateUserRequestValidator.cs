using FluentValidation;
using YourDarkSoulsAssistant.UsersService.DTOs.Users;

namespace YourDarkSoulsAssistant.UsersService.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequestDTO>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ім'я є обов'язковим.")
            .MaximumLength(100).WithMessage("Ім'я надто довге.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Прізвище є обов'язковим.")
            .MaximumLength(100).WithMessage("Прізвище надто довге.");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Юзернейм є обов'язковим.")
            .MinimumLength(3).WithMessage("Юзернейм має містити мінімум 3 символи.")
            .Matches("^[a-zA-Z0-9\\-_\\.]+$").WithMessage("Дозволені лише латинські літери, цифри та знаки -, _, .");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email є обов'язковим.")
            .EmailAddress().WithMessage("Некоректний формат Email.");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль є обов'язковим.")
            .MinimumLength(12).WithMessage("Пароль має містити щонайменше 12 символів.")
            .Matches("[A-Z]").WithMessage("Пароль має містити хоча б одну велику літеру.")
            .Matches("[a-z]").WithMessage("Пароль має містити хоча б одну малу літеру.")
            .Matches("[0-9]").WithMessage("Пароль має містити хоча б одну цифру.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Пароль має містити хоча б один спецсимвол.");
        
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Паролі не збігаються.");
    }
}
