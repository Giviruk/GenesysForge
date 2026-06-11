using System.Text.Json;

namespace GenesysForge.Application.Characters.RuleSnapshots;

internal static class RuleSnapshotService
{
    public static string CreateSnapshotContent(
        RuleSnapshotSource source,
        DraftProfileSnapshot draftProfile,
        DateTimeOffset createdAt)
    {
        var content = new CharacterSnapshotContent(
            SchemaVersion: 1,
            DraftProfile: draftProfile,
            RuleSnapshot: new RuleSnapshotContent(
                source.RulesetId,
                createdAt,
                source.SourceVersions
                    .OrderBy(sourceVersion => sourceVersion.Id)
                    .Select(sourceVersion => new RuleSourceVersionSnapshot(
                        sourceVersion.Id,
                        sourceVersion.SourceBookId,
                        sourceVersion.Version,
                        sourceVersion.IsActive))
                    .ToArray(),
                source.RuleEntities
                    .OrderBy(entity => entity.Id)
                    .Select(entity => new RuleEntitySnapshot(
                        entity.Id,
                        entity.RulesetId,
                        entity.EntityType,
                        entity.Key,
                        entity.Name,
                        entity.Description))
                    .ToArray(),
                source.RuleDefinitions
                    .OrderBy(definition => definition.Id)
                    .Select(definition => new RuleDefinitionSnapshot(
                        definition.Id,
                        definition.RuleEntityId,
                        definition.SourceVersionId,
                        definition.Key,
                        definition.ContentJson))
                    .ToArray()));

        return JsonSerializer.Serialize(content, JsonOptions);
    }

    internal static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
}
