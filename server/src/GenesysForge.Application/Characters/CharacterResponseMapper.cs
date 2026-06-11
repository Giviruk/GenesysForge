using System.Text.Json;
using GenesysForge.Application.Characters.RuleSnapshots;
using GenesysForge.Contracts.Characters;
using GenesysForge.Contracts.Rules;
using Mapster;
using DomainCharacter = GenesysForge.Domain.Characters.Character;

namespace GenesysForge.Application.Characters;

internal static class CharacterResponseMapper
{
    private static readonly TypeAdapterConfig Config = CreateConfig();

    public static CharacterSummaryResponse ToSummaryResponse(DomainCharacter character)
    {
        return character.Adapt<CharacterSummaryResponse>(Config);
    }

    public static CharacterDetailResponse ToDetailResponse(DomainCharacter character)
    {
        var summary = character.Adapt<CharacterSummaryResponse>(Config);
        var snapshotContent = GetLatestSnapshotContent(character);
        var draftProfileSnapshot = snapshotContent?.DraftProfile ?? GetLegacyDraftProfile(character);
        var ruleSnapshot = snapshotContent?.RuleSnapshot;
        var calculatedStats = draftProfileSnapshot is null
            ? null
            : new CalculatedCharacterStatsDto(
                character.AvailableXp,
                character.SpentXp,
                draftProfileSnapshot.Characteristics,
                draftProfileSnapshot.DerivedStats);

        return new CharacterDetailResponse(
            summary.Id,
            summary.Name,
            summary.Status,
            summary.RulesetId,
            summary.UpdatedAt,
            ruleSnapshot is null
                ? null
                : new RuleSnapshotDto(
                    ruleSnapshot.RulesetId,
                    ruleSnapshot.CreatedAt,
                    ruleSnapshot.SourceVersions.Select(sourceVersion => sourceVersion.Id).ToArray(),
                    ruleSnapshot.RuleEntities.Select(entity => entity.Id).ToArray(),
                    ruleSnapshot.RuleDefinitions.Select(definition => definition.Id).ToArray()),
            calculatedStats,
            draftProfileSnapshot is null
                ? null
                : new CharacterDraftProfileResponse(
                    draftProfileSnapshot.ArchetypeId,
                    draftProfileSnapshot.CareerId,
                    draftProfileSnapshot.CareerSkillRanksToAssign),
            character.Skills
                .OrderBy(skill => skill.RuleEntityId)
                .Select(skill => new CharacterSkillResponse(
                    skill.RuleEntityId,
                    skill.Rank,
                    skill.XpSpent,
                    skill.IsCareerSkill,
                    skill.UpdatedAt))
                .ToArray());
    }

    private static TypeAdapterConfig CreateConfig()
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<DomainCharacter, CharacterSummaryResponse>()
            .Map(destination => destination.UpdatedAt, source => source.UpdatedAt ?? source.CreatedAt);

        config.NewConfig<DomainCharacter, CharacterDetailResponse>()
            .Map(destination => destination.UpdatedAt, source => source.UpdatedAt ?? source.CreatedAt)
            .Map(destination => destination.RuleSnapshot, _ => (RuleSnapshotDto?)null)
            .Map(destination => destination.CalculatedStats, _ => (CalculatedCharacterStatsDto?)null)
            .Map(destination => destination.DraftProfile, _ => (CharacterDraftProfileResponse?)null)
            .Map(destination => destination.Skills, _ => Array.Empty<CharacterSkillResponse>());

        return config;
    }

    private static CharacterSnapshotContent? GetLatestSnapshotContent(DomainCharacter character)
    {
        var snapshot = GetLatestSnapshot(character);

        if (snapshot is null)
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<CharacterSnapshotContent>(
                snapshot.ContentJson,
                RuleSnapshotService.JsonOptions);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static DraftProfileSnapshot? GetLegacyDraftProfile(DomainCharacter character)
    {
        var snapshot = GetLatestSnapshot(character);

        if (snapshot is null)
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<DraftProfileSnapshot>(snapshot.ContentJson, JsonOptions);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static GenesysForge.Domain.Characters.CharacterSnapshot? GetLatestSnapshot(DomainCharacter character)
    {
        return character.Snapshots
            .OrderByDescending(snapshot => snapshot.CreatedAt)
            .FirstOrDefault();
    }

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
}
