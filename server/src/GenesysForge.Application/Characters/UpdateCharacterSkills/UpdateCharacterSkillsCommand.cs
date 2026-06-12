using GenesysForge.Contracts.Characters;
using MediatR;

namespace GenesysForge.Application.Characters.UpdateCharacterSkills;

public sealed record UpdateCharacterSkillsCommand(
    Guid OwnerUserId,
    Guid CharacterId,
    IReadOnlyCollection<UpdateCharacterSkillCommandItem> Skills) : IRequest<CharacterDetailResponse>;
