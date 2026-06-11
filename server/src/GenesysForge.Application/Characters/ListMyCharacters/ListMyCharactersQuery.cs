using GenesysForge.Contracts.Characters;
using MediatR;

namespace GenesysForge.Application.Characters.ListMyCharacters;

public sealed record ListMyCharactersQuery(Guid OwnerUserId) : IRequest<IReadOnlyCollection<CharacterSummaryResponse>>;
