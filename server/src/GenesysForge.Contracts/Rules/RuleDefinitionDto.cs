namespace GenesysForge.Contracts.Rules;

public sealed record RuleDefinitionDto(
    Guid Id,
    Guid RuleEntityId,
    Guid SourceVersionId,
    string Key,
    string ContentJson);
