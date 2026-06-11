using GenesysForge.Domain.Rules;

namespace GenesysForge.Application.Characters.RuleSnapshots;

public sealed record RuleSnapshotSource(
    Guid RulesetId,
    IReadOnlyCollection<RuleSourceVersion> SourceVersions,
    IReadOnlyCollection<RuleEntity> RuleEntities,
    IReadOnlyCollection<RuleDefinition> RuleDefinitions);
