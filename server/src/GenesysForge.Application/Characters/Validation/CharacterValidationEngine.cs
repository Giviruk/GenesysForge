using GenesysForge.Contracts.Characters;
using GenesysForge.Contracts.Validation;

namespace GenesysForge.Application.Characters.Validation;

internal static class CharacterValidationEngine
{
    private static readonly IReadOnlyCollection<CharacterValidationRule> Rules =
    [
        CharacterValidationRule.Error(
            "character.snapshot.required",
            "Персонаж должен иметь снимок правил.",
            "ruleSnapshot",
            context => context.Character.RuleSnapshot is null),
        CharacterValidationRule.Error(
            "character.snapshot.definitions.required",
            "Снимок правил должен содержать определения правил.",
            "ruleSnapshot.ruleDefinitionIds",
            context => context.Character.RuleSnapshot is not null &&
                context.Character.RuleSnapshot.RuleDefinitionIds.Count == 0),
        CharacterValidationRule.Error(
            "character.archetype.required",
            "Выберите архетип персонажа.",
            "draftProfile.archetypeId",
            context => context.Character.DraftProfile?.ArchetypeId is null),
        CharacterValidationRule.Error(
            "character.career.required",
            "Выберите карьеру персонажа.",
            "draftProfile.careerId",
            context => context.Character.DraftProfile?.CareerId is null),
        CharacterValidationRule.Error(
            "character.stats.required",
            "Расчетные характеристики персонажа должны быть доступны.",
            "calculatedStats",
            context => context.Character.CalculatedStats is null),
        CharacterValidationRule.Error(
            "character.xp.available.non_negative",
            "Доступный опыт не может быть отрицательным.",
            "calculatedStats.availableXp",
            context => context.Character.CalculatedStats?.AvailableXp < 0),
        CharacterValidationRule.Error(
            "character.skills.required",
            "Персонаж должен иметь список навыков из набора правил.",
            "skills",
            context => context.Character.Skills.Count == 0),
        CharacterValidationRule.Warning(
            "character.career.skills.required",
            "У выбранной карьеры должны быть отмечены карьерные навыки.",
            "skills",
            context => context.Character.DraftProfile?.CareerId is not null &&
                context.Character.Skills.Count > 0 &&
                !context.Character.Skills.Any(skill => skill.IsCareerSkill)),
        CharacterValidationRule.Warning(
            "character.career.starting_ranks.unassigned",
            context => $"Назначьте {context.RequiredCareerSkillRanks} стартовых ранга карьерных навыков.",
            "skills",
            context => context.RequiredCareerSkillRanks > 0 &&
                context.AssignedCareerSkillRanks < context.RequiredCareerSkillRanks)
    ];

    public static ValidationResultResponse Validate(CharacterDetailResponse character)
    {
        var context = new CharacterValidationContext(character);
        var messages = Rules
            .Where(rule => rule.IsBroken(context))
            .Select(rule => rule.ToMessage(context))
            .ToArray();

        return new ValidationResultResponse(
            messages.All(message => message.Severity != ValidationSeverity.Error),
            messages);
    }
}
