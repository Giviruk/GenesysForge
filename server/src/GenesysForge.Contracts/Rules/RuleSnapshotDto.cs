namespace GenesysForge.Contracts.Rules;

public sealed record RuleSnapshotDto(
    Guid RulesetId,
    DateTimeOffset CreatedAt,
    IReadOnlyCollection<Guid> SourceVersionIds,
    IReadOnlyCollection<Guid> RuleEntityIds);
