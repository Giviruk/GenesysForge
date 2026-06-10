namespace GenesysForge.Contracts.Rules;

public sealed record RuleEntityDto(
    Guid Id,
    Guid RulesetId,
    string EntityType,
    string Key,
    string Name,
    string? Description);
