namespace GenesysForge.Contracts.Characters;

public sealed record UpdateCharacterSkillRequest(
    Guid RuleEntityId,
    int Rank);
