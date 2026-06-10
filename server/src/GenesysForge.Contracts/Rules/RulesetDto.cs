namespace GenesysForge.Contracts.Rules;

public sealed record RulesetDto(
    Guid Id,
    string Name,
    string Version,
    string? Description);
