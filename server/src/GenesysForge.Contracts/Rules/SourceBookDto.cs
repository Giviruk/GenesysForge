namespace GenesysForge.Contracts.Rules;

public sealed record SourceBookDto(
    Guid Id,
    Guid RulesetId,
    string Key,
    string Name);
