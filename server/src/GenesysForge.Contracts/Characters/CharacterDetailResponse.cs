using GenesysForge.Contracts.Rules;

namespace GenesysForge.Contracts.Characters;

public sealed record CharacterDetailResponse(
    Guid Id,
    string Name,
    CharacterStatus Status,
    Guid RulesetId,
    RuleSnapshotDto? RuleSnapshot,
    CalculatedCharacterStatsDto? CalculatedStats);
