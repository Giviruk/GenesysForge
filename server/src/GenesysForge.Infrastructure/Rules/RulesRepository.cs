using GenesysForge.Application.Rules;
using GenesysForge.Contracts.Rules;
using GenesysForge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GenesysForge.Infrastructure.Rules;

public sealed class RulesRepository(AppDbContext dbContext) : IRulesRepository
{
    public async Task<RuleCatalogResponse?> GetCatalogAsync(Guid rulesetId, CancellationToken cancellationToken)
    {
        var ruleset = await dbContext.Rulesets
            .AsNoTracking()
            .Where(ruleset => ruleset.Id == rulesetId)
            .Select(ruleset => new RulesetDto(
                ruleset.Id,
                ruleset.Name,
                ruleset.Version,
                ruleset.Description))
            .SingleOrDefaultAsync(cancellationToken);

        if (ruleset is null)
        {
            return null;
        }

        var sourceBooks = await dbContext.SourceBooks
            .AsNoTracking()
            .Where(sourceBook => sourceBook.RulesetId == rulesetId)
            .OrderBy(sourceBook => sourceBook.Name)
            .Select(sourceBook => new SourceBookDto(
                sourceBook.Id,
                sourceBook.RulesetId,
                sourceBook.Key,
                sourceBook.Name))
            .ToListAsync(cancellationToken);
        var sourceBookIds = sourceBooks.Select(sourceBook => sourceBook.Id).ToArray();

        var sourceVersions = await dbContext.RuleSourceVersions
            .AsNoTracking()
            .Where(sourceVersion => sourceBookIds.Contains(sourceVersion.SourceBookId))
            .OrderByDescending(sourceVersion => sourceVersion.IsActive)
            .ThenBy(sourceVersion => sourceVersion.Version)
            .Select(sourceVersion => new RuleSourceVersionDto(
                sourceVersion.Id,
                sourceVersion.SourceBookId,
                sourceVersion.Version,
                sourceVersion.IsActive))
            .ToListAsync(cancellationToken);

        var entities = await dbContext.RuleEntities
            .AsNoTracking()
            .Where(entity => entity.RulesetId == rulesetId)
            .OrderBy(entity => entity.EntityType)
            .ThenBy(entity => entity.Name)
            .Select(entity => new RuleEntityDto(
                entity.Id,
                entity.RulesetId,
                entity.EntityType,
                entity.Key,
                entity.Name,
                entity.Description))
            .ToListAsync(cancellationToken);

        return new RuleCatalogResponse(
            [ruleset],
            sourceBooks,
            sourceVersions,
            entities);
    }

    public async Task<IReadOnlyCollection<RulesetDto>> ListRulesetsAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Rulesets
            .AsNoTracking()
            .OrderBy(ruleset => ruleset.Name)
            .Select(ruleset => new RulesetDto(
                ruleset.Id,
                ruleset.Name,
                ruleset.Version,
                ruleset.Description))
            .ToListAsync(cancellationToken);
    }
}
