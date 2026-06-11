using GenesysForge.Domain.Characters;
using GenesysForge.Domain.Rules;

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

    Task<IReadOnlyCollection<RuleEntity>> ListRuleEntitiesAsync(
        Guid rulesetId,
        string entityType,
        CancellationToken cancellationToken);

    Task<IReadOnlyDictionary<Guid, string>> GetRuleDefinitionContentByEntityIdsAsync(
        IReadOnlyCollection<Guid> ruleEntityIds,
        string definitionKey,
        CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
