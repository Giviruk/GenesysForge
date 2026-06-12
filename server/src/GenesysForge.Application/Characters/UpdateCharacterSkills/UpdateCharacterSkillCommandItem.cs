namespace GenesysForge.Application.Characters.UpdateCharacterSkills;

public sealed record UpdateCharacterSkillCommandItem(
    Guid RuleEntityId,
    int Rank);
