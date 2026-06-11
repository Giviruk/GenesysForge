namespace GenesysForge.Contracts.Characters;

public sealed record CharacterSkillResponse(
    Guid RuleEntityId,
    int Rank,
    int XpSpent,
    bool IsCareerSkill,
    DateTimeOffset UpdatedAt);
