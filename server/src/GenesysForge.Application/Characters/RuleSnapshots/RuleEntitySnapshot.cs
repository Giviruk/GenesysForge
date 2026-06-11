namespace GenesysForge.Application.Characters.RuleSnapshots;

internal sealed record RuleEntitySnapshot(
    Guid Id,
    Guid RulesetId,
    string EntityType,
    string Key,
    string Name,
    string? Description);
