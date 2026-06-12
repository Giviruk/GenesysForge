using GenesysForge.Contracts.Characters;
using GenesysForge.Application.Rules;
using MediatR;

namespace GenesysForge.Application.Characters.UpdateCharacterSkills;

public sealed class UpdateCharacterSkillsCommandHandler(ICharactersRepository charactersRepository)
    : IRequestHandler<UpdateCharacterSkillsCommand, CharacterDetailResponse>
{
    public async Task<CharacterDetailResponse> Handle(
        UpdateCharacterSkillsCommand request,
        CancellationToken cancellationToken)
    {
        var character = await charactersRepository.GetByIdForOwnerForUpdateAsync(
            request.CharacterId,
            request.OwnerUserId,
            cancellationToken);

        if (character is null)
        {
            throw new CharacterNotFoundException(request.CharacterId);
        }

        var skillsByRuleEntityId = character.Skills.ToDictionary(skill => skill.RuleEntityId);
        var updatedAt = DateTimeOffset.UtcNow;

        foreach (var requestedSkill in request.Skills)
        {
            if (!skillsByRuleEntityId.TryGetValue(requestedSkill.RuleEntityId, out var existingSkill))
            {
                throw new RuleEntityNotFoundException(requestedSkill.RuleEntityId, "skill");
            }

            if (!existingSkill.IsCareerSkill && requestedSkill.Rank > 0)
            {
                throw new CharacterSkillUpdateNotAllowedException(
                    requestedSkill.RuleEntityId,
                    "Only career skills can receive starting ranks in the creation wizard.");
            }

            character.SetSkillRank(
                requestedSkill.RuleEntityId,
                requestedSkill.Rank,
                existingSkill.XpSpent,
                existingSkill.IsCareerSkill,
                updatedAt);
        }

        await charactersRepository.SaveChangesAsync(cancellationToken);

        return CharacterResponseMapper.ToDetailResponse(character);
    }
}
