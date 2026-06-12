using FluentValidation;
using GenesysForge.Domain.Characters;

namespace GenesysForge.Application.Characters.UpdateCharacterSkills;

public sealed class UpdateCharacterSkillsCommandValidator : AbstractValidator<UpdateCharacterSkillsCommand>
{
    public UpdateCharacterSkillsCommandValidator()
    {
        RuleFor(command => command.OwnerUserId)
            .NotEmpty();

        RuleFor(command => command.CharacterId)
            .NotEmpty();

        RuleFor(command => command.Skills)
            .NotNull()
            .Must(skills => skills.Select(skill => skill.RuleEntityId).Distinct().Count() == skills.Count)
            .WithMessage("Skill updates must not contain duplicates.");

        RuleForEach(command => command.Skills)
            .ChildRules(skill =>
            {
                skill.RuleFor(item => item.RuleEntityId)
                    .NotEmpty();

                skill.RuleFor(item => item.Rank)
                    .InclusiveBetween(0, CharacterSkill.MaxRank);
            });
    }
}
