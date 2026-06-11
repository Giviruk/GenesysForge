using GenesysForge.Contracts.Characters;
using MediatR;

namespace GenesysForge.Application.Characters.CreateCharacterDraft;

public sealed record CreateCharacterDraftCommand(
    Guid OwnerUserId,
    Guid RulesetId,
    string Name,
    Guid? ArchetypeId = null,
    Guid? CareerId = null) : IRequest<CharacterDetailResponse>;
