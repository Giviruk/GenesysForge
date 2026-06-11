using GenesysForge.Application.Characters.Validation;
using GenesysForge.Contracts.Validation;
using MediatR;

namespace GenesysForge.Application.Characters.ValidateCharacter;

public sealed class ValidateCharacterQueryHandler(ICharactersRepository charactersRepository)
    : IRequestHandler<ValidateCharacterQuery, ValidationResultResponse>
{
    public async Task<ValidationResultResponse> Handle(
        ValidateCharacterQuery request,
        CancellationToken cancellationToken)
    {
        var character = await charactersRepository.GetByIdForOwnerAsync(
            request.CharacterId,
            request.OwnerUserId,
            cancellationToken);

        if (character is null)
        {
            throw new CharacterNotFoundException(request.CharacterId);
        }

        var response = CharacterResponseMapper.ToDetailResponse(character);

        return CharacterValidationEngine.Validate(response);
    }
}
