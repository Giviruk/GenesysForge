namespace GenesysForge.Contracts.Rules;

public sealed record RuleSourceVersionDto(
    Guid Id,
    Guid SourceBookId,
    string Version,
    bool IsActive);
