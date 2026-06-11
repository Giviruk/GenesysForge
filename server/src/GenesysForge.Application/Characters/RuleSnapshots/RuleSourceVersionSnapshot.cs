namespace GenesysForge.Application.Characters.RuleSnapshots;

internal sealed record RuleSourceVersionSnapshot(
    Guid Id,
    Guid SourceBookId,
    string Version,
    bool IsActive);
