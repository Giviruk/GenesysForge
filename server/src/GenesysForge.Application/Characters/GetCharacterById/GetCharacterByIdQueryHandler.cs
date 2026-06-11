using GenesysForge.Contracts.Characters;
using MediatR;

namespace GenesysForge.Application.Characters.GetCharacterById;

public sealed class GetCharacterByIdQueryHandler(ICharactersRepository charactersRepository)
    : IRequestHandler<GetCharacterByIdQuery, CharacterDetailResponse>
{
    public async Task<CharacterDetailResponse> Handle(
        GetCharacterByIdQuery request,
        CancellationToken cancellationToken)
    {
        var character = await charactersRepository.GetByIdForOwnerAsync(
            request.CharacterId,
            request.OwnerUserId,
            cancellationToken);

        return character is null
            ? throw new CharacterNotFoundException(request.CharacterId)
            : CharacterResponseMapper.ToDetailResponse(character);
    }
}
