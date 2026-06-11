using FluentValidation;
using GenesysForge.Domain.Characters;

namespace GenesysForge.Application.Characters.CreateCharacterDraft;

public sealed class CreateCharacterDraftCommandValidator : AbstractValidator<CreateCharacterDraftCommand>
{
    public CreateCharacterDraftCommandValidator()
    {
        RuleFor(command => command.OwnerUserId)
            .NotEmpty()
            .WithMessage("Пользователь не определен.");

        RuleFor(command => command.RulesetId)
            .NotEmpty()
            .WithMessage("Выберите набор правил.");

        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("Укажите имя персонажа.")
            .MaximumLength(Character.MaxNameLength)
            .WithMessage($"Имя персонажа должно содержать не больше {Character.MaxNameLength} символов.");
    }
}
