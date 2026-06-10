using GenesysForge.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GenesysForge.Tests.Infrastructure;

public sealed class RulesetSeedDataTests
{
    [Fact]
    public async Task DemoRulesetSeedDataExists()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        await using (var dbContext = new AppDbContext(options))
        {
            await dbContext.Database.EnsureCreatedAsync();
        }

        await using (var dbContext = new AppDbContext(options))
        {
            var ruleset = await dbContext.Rulesets.SingleAsync();
            var sourceBook = await dbContext.SourceBooks.SingleAsync();
            var sourceVersion = await dbContext.RuleSourceVersions.SingleAsync();
            var entities = await dbContext.RuleEntities.OrderBy(entity => entity.Key).ToListAsync();
            var definitions = await dbContext.RuleDefinitions.ToListAsync();

            Assert.Equal("Демо-набор Genesys Forge", ruleset.Name);
            Assert.Equal(ruleset.Id, sourceBook.RulesetId);
            Assert.Equal(sourceBook.Id, sourceVersion.SourceBookId);
            Assert.True(sourceVersion.IsActive);
            Assert.Equal(4, entities.Count);
            Assert.Contains(entities, entity => entity.EntityType == "archetype" && entity.Key == "guardian");
            Assert.Contains(entities, entity => entity.EntityType == "archetype" && entity.Key == "mystic");
            Assert.Contains(entities, entity => entity.EntityType == "skill" && entity.Key == "resolve");
            Assert.Contains(entities, entity => entity.EntityType == "skill" && entity.Key == "tactics");
            Assert.Equal(4, definitions.Count);
            Assert.All(definitions, definition => Assert.Equal(sourceVersion.Id, definition.SourceVersionId));
        }
    }
}
