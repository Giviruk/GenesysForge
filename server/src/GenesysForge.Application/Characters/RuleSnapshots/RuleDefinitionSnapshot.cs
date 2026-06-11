namespace GenesysForge.Application.Characters.RuleSnapshots;

internal sealed record RuleDefinitionSnapshot(
    Guid Id,
    Guid RuleEntityId,
    Guid SourceVersionId,
    string Key,
    string ContentJson);
