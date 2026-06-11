namespace GenesysForge.Application.Characters.RuleSnapshots;

internal sealed record CharacterSnapshotContent(
    int SchemaVersion,
    DraftProfileSnapshot DraftProfile,
    RuleSnapshotContent RuleSnapshot);
