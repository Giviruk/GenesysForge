namespace GenesysForge.Contracts.Characters;

public sealed record UpdateCharacterSkillsRequest(
    IReadOnlyCollection<UpdateCharacterSkillRequest> Skills);
