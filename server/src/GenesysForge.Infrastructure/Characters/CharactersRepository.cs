using GenesysForge.Application.Characters;
using GenesysForge.Domain.Characters;
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

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }

}
