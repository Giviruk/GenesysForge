using GenesysForge.Contracts.Characters;

namespace GenesysForge.Application.Characters.Validation;

internal sealed class CharacterValidationContext(CharacterDetailResponse character)
{
    public CharacterDetailResponse Character { get; } = character;

    public int RequiredCareerSkillRanks => Character.DraftProfile?.CareerSkillRanksToAssign ?? 0;

    public int AssignedCareerSkillRanks => Character.Skills
        .Where(skill => skill.IsCareerSkill)
        .Sum(skill => skill.Rank);
}
