using GenesysForge.Domain.Characters;

namespace GenesysForge.Application.Characters;

public interface ICharactersRepository
{
    Task AddAsync(Character character, CancellationToken cancellationToken);

    Task<Character?> GetByIdForOwnerAsync(
        Guid characterId,
        Guid ownerUserId,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Character>> ListForOwnerAsync(
        Guid ownerUserId,
        CancellationToken cancellationToken);

    Task<bool> RulesetExistsAsync(Guid rulesetId, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
