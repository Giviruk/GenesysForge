namespace GenesysForge.Application.Characters.RuleSnapshots;

internal sealed record DraftProfileSnapshot(
    Guid? ArchetypeId,
    Guid? CareerId,
    int CareerSkillRanksToAssign,
    IReadOnlyDictionary<string, int> Characteristics,
    IReadOnlyDictionary<string, int> DerivedStats);
