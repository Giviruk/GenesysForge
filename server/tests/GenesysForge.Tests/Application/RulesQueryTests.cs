using GenesysForge.Application.Rules;
using GenesysForge.Application.Rules.GetRuleCatalog;
using GenesysForge.Application.Rules.GetRulesets;
using GenesysForge.Infrastructure.Persistence;
using GenesysForge.Infrastructure.Rules;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GenesysForge.Tests.Application;

public sealed class RulesQueryTests
{
    [Fact]
    public async Task GetRulesetsReturnsDemoRuleset()
    {
        await using var fixture = await RulesTestFixture.CreateAsync();
        var handler = new GetRulesetsQueryHandler(fixture.RulesRepository);

        var rulesets = await handler.Handle(new GetRulesetsQuery(), CancellationToken.None);

        var ruleset = Assert.Single(rulesets);
        Assert.Equal("Демо-набор Genesys Forge", ruleset.Name);
    }

    [Fact]
    public async Task GetRuleCatalogReturnsDemoCatalog()
    {
        await using var fixture = await RulesTestFixture.CreateAsync();
        var handler = new GetRuleCatalogQueryHandler(fixture.RulesRepository);
        var rulesetId = await fixture.DbContext.Rulesets
            .Where(ruleset => ruleset.Name == "Демо-набор Genesys Forge")
            .Select(ruleset => ruleset.Id)
            .SingleAsync(CancellationToken.None);

        var catalog = await handler.Handle(
            new GetRuleCatalogQuery(rulesetId),
            CancellationToken.None);

        Assert.Single(catalog.Rulesets);
        Assert.Single(catalog.SourceBooks);
        Assert.Single(catalog.SourceVersions);
        Assert.Equal(25, catalog.Entities.Count);
        Assert.Equal(25, catalog.Definitions.Count);
        Assert.Contains(catalog.Entities, entity => entity.EntityType == "archetype" && entity.Key == "guardian");
        Assert.Contains(catalog.Entities, entity => entity.EntityType == "career" && entity.Key == "warrior");
        Assert.Contains(catalog.Entities, entity => entity.EntityType == "skill" && entity.Key == "resolve");
        Assert.Contains(catalog.Entities, entity => entity.EntityType == "skill" && entity.Key == "education");
        Assert.Contains(catalog.Entities, entity => entity.EntityType == "talent" && entity.Key == "steady-stance");
        Assert.Contains(catalog.Definitions, definition => definition.Key == "career-profile");
        Assert.Contains(catalog.Definitions, definition => definition.Key == "skill-profile");
    }

    [Fact]
    public async Task GetRuleCatalogRejectsUnknownRuleset()
    {
        await using var fixture = await RulesTestFixture.CreateAsync();
        var handler = new GetRuleCatalogQueryHandler(fixture.RulesRepository);

        await Assert.ThrowsAsync<RulesetNotFoundException>(() =>
            handler.Handle(new GetRuleCatalogQuery(Guid.NewGuid()), CancellationToken.None));
    }

    private sealed class RulesTestFixture : IAsyncDisposable
    {
        private RulesTestFixture(SqliteConnection connection, AppDbContext dbContext)
        {
            Connection = connection;
            DbContext = dbContext;
            RulesRepository = new RulesRepository(dbContext);
        }

        public SqliteConnection Connection { get; }

        public AppDbContext DbContext { get; }

        public RulesRepository RulesRepository { get; }

        public static async Task<RulesTestFixture> CreateAsync()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;
            var dbContext = new AppDbContext(options);
            await dbContext.Database.EnsureCreatedAsync();

            return new RulesTestFixture(connection, dbContext);
        }

        public async ValueTask DisposeAsync()
        {
            await DbContext.DisposeAsync();
            await Connection.DisposeAsync();
        }
    }
}
