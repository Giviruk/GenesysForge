using GenesysForge.Application.Rules;
using GenesysForge.Contracts.Characters;
using GenesysForge.Domain.Characters;
using MediatR;

namespace GenesysForge.Application.Characters.CreateCharacterDraft;

public sealed class CreateCharacterDraftCommandHandler(ICharactersRepository charactersRepository)
    : IRequestHandler<CreateCharacterDraftCommand, CharacterDetailResponse>
{
    public async Task<CharacterDetailResponse> Handle(
        CreateCharacterDraftCommand request,
        CancellationToken cancellationToken)
    {
        var rulesetExists = await charactersRepository.RulesetExistsAsync(request.RulesetId, cancellationToken);
        if (!rulesetExists)
        {
            throw new RulesetNotFoundException(request.RulesetId);
        }

        var character = Character.CreateDraft(request.OwnerUserId, request.RulesetId, request.Name);

        await charactersRepository.AddAsync(character, cancellationToken);
        await charactersRepository.SaveChangesAsync(cancellationToken);

        return CharacterResponseMapper.ToDetailResponse(character);
    }
}
