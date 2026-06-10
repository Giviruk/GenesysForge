using GenesysForge.Contracts.Rules;

namespace GenesysForge.Contracts.Characters;

public enum CharacterStatus
{
    Draft = 0,
    Active = 1,
    Archived = 2,
}

public sealed record CreateCharacterDraftRequest(
    string Name,
    Guid RulesetId);

public sealed record UpdateCharacterBasicInfoRequest(
    string Name);

public sealed record CharacterSummaryResponse(
    Guid Id,
    string Name,
    CharacterStatus Status,
    Guid RulesetId,
    DateTimeOffset UpdatedAt);

public sealed record CharacterDetailResponse(
    Guid Id,
    string Name,
    CharacterStatus Status,
    Guid RulesetId,
    RuleSnapshotDto? RuleSnapshot,
    CalculatedCharacterStatsDto? CalculatedStats);

public sealed record CalculatedCharacterStatsDto(
    int AvailableXp,
    int SpentXp,
    IReadOnlyDictionary<string, int> Characteristics,
    IReadOnlyDictionary<string, int> DerivedStats);
