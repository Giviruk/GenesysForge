using System.Text.Json;
using GenesysForge.Application.Characters.RuleSnapshots;
using GenesysForge.Contracts.Characters;
using DomainCharacter = GenesysForge.Domain.Characters.Character;

namespace GenesysForge.Application.Characters.Calculations;

internal static class CharacterCalculationService
{
    private const string StartingProfileDefinitionKey = "starting-profile";

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public static CalculatedCharacterStatsDto? Calculate(
        DomainCharacter character,
        CharacterSnapshotContent? snapshotContent,
        DraftProfileSnapshot? legacyDraftProfile)
    {
        var profile = GetStartingProfile(snapshotContent) ?? GetLegacyStartingProfile(legacyDraftProfile);
        if (profile is null)
        {
            return null;
        }

        return new CalculatedCharacterStatsDto(
            character.AvailableXp,
            character.SpentXp,
            profile.Characteristics,
            profile.DerivedStats);
    }

    private static StartingProfile? GetStartingProfile(CharacterSnapshotContent? snapshotContent)
    {
        if (snapshotContent?.DraftProfile.ArchetypeId is not { } archetypeId)
        {
            return null;
        }

        var definition = snapshotContent.RuleSnapshot.RuleDefinitions
            .FirstOrDefault(definition =>
                definition.RuleEntityId == archetypeId &&
                definition.Key == StartingProfileDefinitionKey);

        if (definition is null)
        {
            return null;
        }

        try
        {
            var profile = JsonSerializer.Deserialize<StartingProfile>(definition.ContentJson, JsonOptions);

            return profile is { Characteristics: not null, DerivedStats: not null }
                ? profile
                : null;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static StartingProfile? GetLegacyStartingProfile(DraftProfileSnapshot? draftProfile)
    {
        return draftProfile is null
            ? null
            : new StartingProfile(draftProfile.Characteristics, draftProfile.DerivedStats);
    }

    private sealed record StartingProfile(
        IReadOnlyDictionary<string, int> Characteristics,
        IReadOnlyDictionary<string, int> DerivedStats);
}
