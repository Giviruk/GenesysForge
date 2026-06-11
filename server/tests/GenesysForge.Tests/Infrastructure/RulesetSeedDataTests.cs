using System.Text.Json;
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
            Assert.Equal(25, entities.Count);
            Assert.Contains(entities, entity => entity.EntityType == "archetype" && entity.Key == "guardian");
            Assert.Contains(entities, entity => entity.EntityType == "archetype" && entity.Key == "mystic");
            Assert.Contains(entities, entity => entity.EntityType == "career" && entity.Key == "warrior");
            Assert.Contains(entities, entity => entity.EntityType == "career" && entity.Key == "scholar");
            Assert.Contains(entities, entity => entity.EntityType == "skill" && entity.Key == "resolve");
            Assert.Contains(entities, entity => entity.EntityType == "skill" && entity.Key == "tactics");
            Assert.Contains(entities, entity => entity.EntityType == "skill" && entity.Key == "brawl");
            Assert.Contains(entities, entity => entity.EntityType == "skill" && entity.Key == "charm");
            Assert.Contains(entities, entity => entity.EntityType == "skill" && entity.Key == "education");
            Assert.Contains(entities, entity => entity.EntityType == "talent" && entity.Key == "steady-stance");
            Assert.Equal(25, definitions.Count);
            Assert.All(definitions, definition => Assert.Equal(sourceVersion.Id, definition.SourceVersionId));

            var resolve = entities.Single(entity => entity.Key == "resolve");
            var resolveProfile = definitions.Single(definition => definition.RuleEntityId == resolve.Id);
            using var resolveJson = JsonDocument.Parse(resolveProfile.ContentJson);
            Assert.Equal("general", resolveJson.RootElement.GetProperty("category").GetString());
            Assert.Equal("rank-table", resolveJson.RootElement.GetProperty("cost").GetProperty("type").GetString());
            Assert.Equal(10, resolveJson.RootElement.GetProperty("cost").GetProperty("career").GetProperty("2").GetInt32());
            Assert.Equal(15, resolveJson.RootElement.GetProperty("cost").GetProperty("nonCareer").GetProperty("2").GetInt32());

            var brawl = entities.Single(entity => entity.Key == "brawl");
            var brawlProfile = definitions.Single(definition => definition.RuleEntityId == brawl.Id);
            using var brawlJson = JsonDocument.Parse(brawlProfile.ContentJson);
            Assert.Equal("combat", brawlJson.RootElement.GetProperty("category").GetString());

            var charm = entities.Single(entity => entity.Key == "charm");
            var charmProfile = definitions.Single(definition => definition.RuleEntityId == charm.Id);
            using var charmJson = JsonDocument.Parse(charmProfile.ContentJson);
            Assert.Equal("social", charmJson.RootElement.GetProperty("category").GetString());

            var education = entities.Single(entity => entity.Key == "education");
            var educationProfile = definitions.Single(definition => definition.RuleEntityId == education.Id);
            using var educationJson = JsonDocument.Parse(educationProfile.ContentJson);
            Assert.Equal("knowledge", educationJson.RootElement.GetProperty("category").GetString());

            var warrior = entities.Single(entity => entity.Key == "warrior");
            var warriorProfile = definitions.Single(definition => definition.RuleEntityId == warrior.Id);
            using var warriorJson = JsonDocument.Parse(warriorProfile.ContentJson);
            Assert.Equal(4, warriorJson.RootElement.GetProperty("careerSkillRanksToAssign").GetInt32());
            Assert.Contains(
                warriorJson.RootElement.GetProperty("careerSkillKeys").EnumerateArray(),
                key => key.GetString() == "brawl");

            var steadyStance = entities.Single(entity => entity.Key == "steady-stance");
            var steadyStanceProfile = definitions.Single(definition => definition.RuleEntityId == steadyStance.Id);
            using var steadyStanceJson = JsonDocument.Parse(steadyStanceProfile.ContentJson);
            Assert.Equal("fixed", steadyStanceJson.RootElement.GetProperty("cost").GetProperty("type").GetString());
            Assert.Equal(5, steadyStanceJson.RootElement.GetProperty("cost").GetProperty("xp").GetInt32());
        }
    }
}
