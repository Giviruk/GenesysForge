using GenesysForge.Contracts.Characters;
using MediatR;

namespace GenesysForge.Application.Characters.GetCharacterById;

public sealed record GetCharacterByIdQuery(
    Guid OwnerUserId,
    Guid CharacterId) : IRequest<CharacterDetailResponse>;
