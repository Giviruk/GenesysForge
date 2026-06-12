namespace GenesysForge.Application.Characters.UpdateCharacterSkills;

public sealed class CharacterSkillUpdateNotAllowedException(Guid ruleEntityId, string message) : Exception(message)
{
    public Guid RuleEntityId { get; } = ruleEntityId;
}
