using GenesysForge.Contracts.Characters;
using MediatR;

namespace GenesysForge.Application.Characters.ListMyCharacters;

public sealed class ListMyCharactersQueryHandler(ICharactersRepository charactersRepository)
    : IRequestHandler<ListMyCharactersQuery, IReadOnlyCollection<CharacterSummaryResponse>>
{
    public Task<IReadOnlyCollection<CharacterSummaryResponse>> Handle(
        ListMyCharactersQuery request,
        CancellationToken cancellationToken)
    {
        return ListForOwnerAsync(request.OwnerUserId, cancellationToken);
    }

    private async Task<IReadOnlyCollection<CharacterSummaryResponse>> ListForOwnerAsync(
        Guid ownerUserId,
        CancellationToken cancellationToken)
    {
        var characters = await charactersRepository.ListForOwnerAsync(ownerUserId, cancellationToken);

        return characters
            .Select(CharacterResponseMapper.ToSummaryResponse)
            .ToList();
    }
}
