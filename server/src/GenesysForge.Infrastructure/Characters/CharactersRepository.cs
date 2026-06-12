using GenesysForge.Application.Characters;
using GenesysForge.Application.Characters.RuleSnapshots;
using GenesysForge.Domain.Characters;
using GenesysForge.Domain.Rules;
using GenesysForge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GenesysForge.Infrastructure.Characters;

public sealed class CharactersRepository(AppDbContext dbContext) : ICharactersRepository
{
    public async Task AddAsync(Character character, CancellationToken cancellationToken)
    {
        await dbContext.Characters.AddAsync(character, cancellationToken);
    }

    public Task<Character?> GetByIdForOwnerAsync(
        Guid characterId,
        Guid ownerUserId,
        CancellationToken cancellationToken)
    {
        return dbContext.Characters
            .AsNoTracking()
            .Include(character => character.XpLedgerEntries)
            .Include(character => character.Skills)
            .Include(character => character.Talents)
            .Include(character => character.Snapshots)
            .Where(character => character.Id == characterId && character.OwnerUserId == ownerUserId)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task<Character?> GetByIdForOwnerForUpdateAsync(
        Guid characterId,
        Guid ownerUserId,
        CancellationToken cancellationToken)
    {
        return dbContext.Characters
            .Include(character => character.XpLedgerEntries)
            .Include(character => character.Skills)
            .Include(character => character.Talents)
            .Include(character => character.Snapshots)
            .Where(character => character.Id == characterId && character.OwnerUserId == ownerUserId)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Character>> ListForOwnerAsync(
        Guid ownerUserId,
        CancellationToken cancellationToken)
    {
        return await dbContext.Characters
            .AsNoTracking()
            .Where(character => character.OwnerUserId == ownerUserId)
            .OrderBy(character => character.Name)
            .ThenBy(character => character.Id)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> RulesetExistsAsync(Guid rulesetId, CancellationToken cancellationToken)
    {
        return dbContext.Rulesets.AnyAsync(ruleset => ruleset.Id == rulesetId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<RuleEntity>> ListRuleEntitiesAsync(
        Guid rulesetId,
        string entityType,
        CancellationToken cancellationToken)
    {
        return await dbContext.RuleEntities
            .AsNoTracking()
            .Where(entity => entity.RulesetId == rulesetId && entity.EntityType == entityType)
            .OrderBy(entity => entity.Name)
            .ThenBy(entity => entity.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyDictionary<Guid, string>> GetRuleDefinitionContentByEntityIdsAsync(
        IReadOnlyCollection<Guid> ruleEntityIds,
        string definitionKey,
        CancellationToken cancellationToken)
    {
        return await dbContext.RuleDefinitions
            .AsNoTracking()
            .Where(definition => ruleEntityIds.Contains(definition.RuleEntityId) && definition.Key == definitionKey)
            .ToDictionaryAsync(
                definition => definition.RuleEntityId,
                definition => definition.ContentJson,
                cancellationToken);
    }

    public async Task<RuleSnapshotSource> GetRuleSnapshotSourceAsync(
        Guid rulesetId,
        CancellationToken cancellationToken)
    {
        var sourceBookIds = await dbContext.SourceBooks
            .AsNoTracking()
            .Where(sourceBook => sourceBook.RulesetId == rulesetId)
            .Select(sourceBook => sourceBook.Id)
            .ToArrayAsync(cancellationToken);

        var sourceVersions = await dbContext.RuleSourceVersions
            .AsNoTracking()
            .Where(sourceVersion => sourceBookIds.Contains(sourceVersion.SourceBookId) && sourceVersion.IsActive)
            .OrderBy(sourceVersion => sourceVersion.Id)
            .ToArrayAsync(cancellationToken);
        var sourceVersionIds = sourceVersions.Select(sourceVersion => sourceVersion.Id).ToArray();

        var ruleEntities = await dbContext.RuleEntities
            .AsNoTracking()
            .Where(entity => entity.RulesetId == rulesetId)
            .OrderBy(entity => entity.Id)
            .ToArrayAsync(cancellationToken);
        var ruleEntityIds = ruleEntities.Select(entity => entity.Id).ToArray();

        var ruleDefinitions = await dbContext.RuleDefinitions
            .AsNoTracking()
            .Where(definition =>
                ruleEntityIds.Contains(definition.RuleEntityId) &&
                sourceVersionIds.Contains(definition.SourceVersionId))
            .OrderBy(definition => definition.Id)
            .ToArrayAsync(cancellationToken);

        return new RuleSnapshotSource(
            rulesetId,
            sourceVersions,
            ruleEntities,
            ruleDefinitions);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }

}
