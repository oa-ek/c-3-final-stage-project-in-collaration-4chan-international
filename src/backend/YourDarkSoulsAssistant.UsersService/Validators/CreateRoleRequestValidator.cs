using FluentValidation;
using YourDarkSoulsAssistant.UsersService.DTOs.Roles;

namespace YourDarkSoulsAssistant.UsersService.Validators;

public class CreateRoleRequestValidator: AbstractValidator<CreateRoleRequestDTO>
{
    public CreateRoleRequestValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Опис є обов'язковим.")
            .MaximumLength(256).WithMessage("Опис надто довгий.");
    }
}
