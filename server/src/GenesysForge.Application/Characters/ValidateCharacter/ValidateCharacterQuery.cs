using GenesysForge.Contracts.Validation;
using MediatR;

namespace GenesysForge.Application.Characters.ValidateCharacter;

public sealed record ValidateCharacterQuery(
    Guid OwnerUserId,
    Guid CharacterId) : IRequest<ValidationResultResponse>;
