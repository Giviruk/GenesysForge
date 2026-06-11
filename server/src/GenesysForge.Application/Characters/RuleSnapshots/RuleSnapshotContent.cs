namespace GenesysForge.Application.Characters.RuleSnapshots;

internal sealed record RuleSnapshotContent(
    Guid RulesetId,
    DateTimeOffset CreatedAt,
    IReadOnlyCollection<RuleSourceVersionSnapshot> SourceVersions,
    IReadOnlyCollection<RuleEntitySnapshot> RuleEntities,
    IReadOnlyCollection<RuleDefinitionSnapshot> RuleDefinitions);
