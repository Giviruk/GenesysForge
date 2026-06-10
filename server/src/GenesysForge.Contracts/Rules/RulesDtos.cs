namespace GenesysForge.Contracts.Rules;

public sealed record RulesetDto(
    Guid Id,
    string Name,
    string Version,
    string? Description);

public sealed record SourceBookDto(
    Guid Id,
    Guid RulesetId,
    string Key,
    string Name);

public sealed record RuleSourceVersionDto(
    Guid Id,
    Guid SourceBookId,
    string Version,
    bool IsActive);

public sealed record RuleEntityDto(
    Guid Id,
    Guid RulesetId,
    string EntityType,
    string Key,
    string Name,
    string? Description);

public sealed record RuleCatalogResponse(
    IReadOnlyCollection<RulesetDto> Rulesets,
    IReadOnlyCollection<SourceBookDto> SourceBooks,
    IReadOnlyCollection<RuleSourceVersionDto> SourceVersions,
    IReadOnlyCollection<RuleEntityDto> Entities);

public sealed record RuleSnapshotDto(
    Guid RulesetId,
    DateTimeOffset CreatedAt,
    IReadOnlyCollection<Guid> SourceVersionIds,
    IReadOnlyCollection<Guid> RuleEntityIds);
