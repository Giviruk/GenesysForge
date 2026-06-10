using FluentValidation;

namespace GenesysForge.Application.Auth.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
            .WithMessage("Укажите email.")
            .MaximumLength(320)
            .WithMessage("Email должен содержать не больше 320 символов.")
            .EmailAddress()
            .WithMessage("Введите корректный email.");

        RuleFor(command => command.Password)
            .NotEmpty()
            .WithMessage("Укажите пароль.")
            .MinimumLength(8)
            .WithMessage("Пароль должен содержать минимум 8 символов.")
            .MaximumLength(128)
            .WithMessage("Пароль должен содержать не больше 128 символов.");

        RuleFor(command => command.DisplayName)
            .NotEmpty()
            .WithMessage("Укажите отображаемое имя.")
            .MaximumLength(100)
            .WithMessage("Отображаемое имя должно содержать не больше 100 символов.");
    }
}
