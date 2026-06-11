namespace GenesysForge.Contracts.Rules;

public sealed record RuleCatalogResponse(
    IReadOnlyCollection<RulesetDto> Rulesets,
    IReadOnlyCollection<SourceBookDto> SourceBooks,
    IReadOnlyCollection<RuleSourceVersionDto> SourceVersions,
    IReadOnlyCollection<RuleEntityDto> Entities,
    IReadOnlyCollection<RuleDefinitionDto> Definitions);
