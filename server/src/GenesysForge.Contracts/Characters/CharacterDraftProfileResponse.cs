namespace GenesysForge.Contracts.Characters;

public sealed record CharacterDraftProfileResponse(
    Guid? ArchetypeId,
    Guid? CareerId,
    int CareerSkillRanksToAssign);
