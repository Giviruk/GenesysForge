namespace GenesysForge.Contracts.Characters;

public sealed record CalculatedCharacterStatsDto(
    int AvailableXp,
    int SpentXp,
    IReadOnlyDictionary<string, int> Characteristics,
    IReadOnlyDictionary<string, int> DerivedStats);
