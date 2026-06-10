namespace GenesysForge.Contracts.Characters;

public sealed record CharacterSummaryResponse(
    Guid Id,
    string Name,
    CharacterStatus Status,
    Guid RulesetId,
    DateTimeOffset UpdatedAt);
