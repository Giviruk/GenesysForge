using FluentValidation;

namespace GenesysForge.Application.Auth.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
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
            .WithMessage("Укажите пароль.");
    }
}
