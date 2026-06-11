using GenesysForge.Application.Characters;
using GenesysForge.Application.Characters.CreateCharacterDraft;
using GenesysForge.Application.Characters.GetCharacterById;
using GenesysForge.Application.Characters.ListMyCharacters;
using GenesysForge.Application.Rules;
using GenesysForge.Domain.Characters;
using GenesysForge.Domain.Users;
using GenesysForge.Infrastructure.Auth;
using GenesysForge.Infrastructure.Characters;
using GenesysForge.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;

namespace GenesysForge.Tests.Application;

public sealed class CharacterCommandTests
{
    [Fact]
    public async Task CreateDraftStoresCharacterForCurrentOwner()
    {
        await using var fixture = await CharacterTestFixture.CreateAsync();
        var createHandler = new CreateCharacterDraftCommandHandler(fixture.CharactersRepository);
        var listHandler = new ListMyCharactersQueryHandler(fixture.CharactersRepository);
        var getHandler = new GetCharacterByIdQueryHandler(fixture.CharactersRepository);
        var guardianArchetypeId = await fixture.DbContext.RuleEntities
            .Where(entity => entity.Key == "guardian")
            .Select(entity => entity.Id)
            .SingleAsync();
        var warriorCareerId = await fixture.DbContext.RuleEntities
            .Where(entity => entity.Key == "warrior")
            .Select(entity => entity.Id)
            .SingleAsync();

        var created = await createHandler.Handle(
            new CreateCharacterDraftCommand(
                fixture.OwnerUserId,
                fixture.RulesetId,
                "Mira Vale",
                guardianArchetypeId,
                warriorCareerId),
            CancellationToken.None);

        var ownerCharacters = await listHandler.Handle(
            new ListMyCharactersQuery(fixture.OwnerUserId),
            CancellationToken.None);
        var otherUserCharacters = await listHandler.Handle(
            new ListMyCharactersQuery(fixture.OtherUserId),
            CancellationToken.None);
        var loaded = await getHandler.Handle(
            new GetCharacterByIdQuery(fixture.OwnerUserId, created.Id),
            CancellationToken.None);

        var ownerCharacter = Assert.Single(ownerCharacters);
        Assert.Equal(created.Id, ownerCharacter.Id);
        Assert.Equal("Mira Vale", loaded.Name);
        Assert.Equal(fixture.RulesetId, loaded.RulesetId);
        Assert.NotNull(loaded.DraftProfile);
        Assert.NotNull(loaded.RuleSnapshot);
        Assert.NotNull(loaded.CalculatedStats);
        Assert.Single(loaded.RuleSnapshot.SourceVersionIds);
        Assert.Equal(25, loaded.RuleSnapshot.RuleEntityIds.Count);
        Assert.Equal(25, loaded.RuleSnapshot.RuleDefinitionIds.Count);
        Assert.Equal(20, loaded.Skills.Count);
        Assert.Contains(loaded.Skills, skill => skill.IsCareerSkill);
        Assert.Contains(loaded.Skills, skill => !skill.IsCareerSkill);
        Assert.Equal(3, loaded.CalculatedStats.Characteristics["brawn"]);
        Assert.Equal(13, loaded.CalculatedStats.DerivedStats["woundThreshold"]);
        Assert.Empty(otherUserCharacters);
    }

    [Fact]
    public async Task CreateDraftSnapshotsRuleDefinitionsIndependentlyFromCurrentSeedData()
    {
        await using var fixture = await CharacterTestFixture.CreateAsync();
        var createHandler = new CreateCharacterDraftCommandHandler(fixture.CharactersRepository);
        var getHandler = new GetCharacterByIdQueryHandler(fixture.CharactersRepository);
        var guardianArchetypeId = await fixture.DbContext.RuleEntities
            .Where(entity => entity.Key == "guardian")
            .Select(entity => entity.Id)
            .SingleAsync();

        var created = await createHandler.Handle(
            new CreateCharacterDraftCommand(
                fixture.OwnerUserId,
                fixture.RulesetId,
                "Mira Vale",
                guardianArchetypeId),
            CancellationToken.None);
        var snapshotBefore = await fixture.DbContext.CharacterSnapshots
            .AsNoTracking()
            .Where(snapshot => snapshot.CharacterId == created.Id)
            .Select(snapshot => snapshot.ContentJson)
            .SingleAsync();

        var changedDefinitionJson = """{"changed":true}""";
        await fixture.DbContext.Database.ExecuteSqlInterpolatedAsync(
            $"""
            UPDATE "RuleDefinitions"
            SET "ContentJson" = {changedDefinitionJson}
            WHERE "RuleEntityId" = {guardianArchetypeId}
            """);

        var currentDefinition = await fixture.DbContext.RuleDefinitions
            .AsNoTracking()
            .Where(definition => definition.RuleEntityId == guardianArchetypeId)
            .Select(definition => definition.ContentJson)
            .SingleAsync();
        var loadedAfterDefinitionChange = await getHandler.Handle(
            new GetCharacterByIdQuery(fixture.OwnerUserId, created.Id),
            CancellationToken.None);
        var snapshotAfter = await fixture.DbContext.CharacterSnapshots
            .AsNoTracking()
            .Where(snapshot => snapshot.CharacterId == created.Id)
            .Select(snapshot => snapshot.ContentJson)
            .SingleAsync();

        Assert.Equal("{\"changed\":true}", currentDefinition);
        Assert.Equal(snapshotBefore, snapshotAfter);
        Assert.Contains("\"woundThreshold\":13", snapshotAfter);
        Assert.DoesNotContain("\"changed\":true", snapshotAfter);
        Assert.NotNull(loadedAfterDefinitionChange.RuleSnapshot);
        Assert.Equal(25, loadedAfterDefinitionChange.RuleSnapshot.RuleDefinitionIds.Count);
    }

    [Fact]
    public async Task GetByIdCalculatesStatsFromRuleSnapshotDefinitions()
    {
        await using var fixture = await CharacterTestFixture.CreateAsync();
        var createHandler = new CreateCharacterDraftCommandHandler(fixture.CharactersRepository);
        var getHandler = new GetCharacterByIdQueryHandler(fixture.CharactersRepository);
        var guardianArchetypeId = await fixture.DbContext.RuleEntities
            .Where(entity => entity.Key == "guardian")
            .Select(entity => entity.Id)
            .SingleAsync();

        var created = await createHandler.Handle(
            new CreateCharacterDraftCommand(
                fixture.OwnerUserId,
                fixture.RulesetId,
                "Mira Vale",
                guardianArchetypeId),
            CancellationToken.None);
        var snapshot = await fixture.DbContext.CharacterSnapshots
            .Where(snapshot => snapshot.CharacterId == created.Id)
            .SingleAsync();
        var content = JsonNode.Parse(snapshot.ContentJson)!;
        content["draftProfile"]!["characteristics"]!["brawn"] = 1;
        content["draftProfile"]!["derivedStats"]!["woundThreshold"] = 99;
        var changedSnapshotJson = content.ToJsonString();
        await fixture.DbContext.Database.ExecuteSqlInterpolatedAsync(
            $"""
            UPDATE "CharacterSnapshots"
            SET "ContentJson" = {changedSnapshotJson}
            WHERE "Id" = {snapshot.Id}
            """);

        var loaded = await getHandler.Handle(
            new GetCharacterByIdQuery(fixture.OwnerUserId, created.Id),
            CancellationToken.None);

        Assert.NotNull(loaded.CalculatedStats);
        Assert.Equal(3, loaded.CalculatedStats.Characteristics["brawn"]);
        Assert.Equal(13, loaded.CalculatedStats.DerivedStats["woundThreshold"]);
    }

    [Fact]
    public async Task GetByIdCalculatesXpTotalsFromLedger()
    {
        await using var fixture = await CharacterTestFixture.CreateAsync();
        var createHandler = new CreateCharacterDraftCommandHandler(fixture.CharactersRepository);
        var getHandler = new GetCharacterByIdQueryHandler(fixture.CharactersRepository);

        var created = await createHandler.Handle(
            new CreateCharacterDraftCommand(fixture.OwnerUserId, fixture.RulesetId, "Mira Vale"),
            CancellationToken.None);
        fixture.DbContext.XpLedgerEntries.AddRange(
            XpLedgerEntry.Create(created.Id, 100, "Initial XP"),
            XpLedgerEntry.Create(created.Id, -35, "Skill purchases"));
        await fixture.DbContext.SaveChangesAsync();

        var loaded = await getHandler.Handle(
            new GetCharacterByIdQuery(fixture.OwnerUserId, created.Id),
            CancellationToken.None);

        Assert.NotNull(loaded.CalculatedStats);
        Assert.Equal(65, loaded.CalculatedStats.AvailableXp);
        Assert.Equal(35, loaded.CalculatedStats.SpentXp);
    }

    [Fact]
    public async Task GetByIdRejectsCharacterOwnedByAnotherUser()
    {
        await using var fixture = await CharacterTestFixture.CreateAsync();
        var createHandler = new CreateCharacterDraftCommandHandler(fixture.CharactersRepository);
        var getHandler = new GetCharacterByIdQueryHandler(fixture.CharactersRepository);
        var created = await createHandler.Handle(
            new CreateCharacterDraftCommand(fixture.OwnerUserId, fixture.RulesetId, "Mira Vale"),
            CancellationToken.None);

        await Assert.ThrowsAsync<CharacterNotFoundException>(() =>
            getHandler.Handle(
                new GetCharacterByIdQuery(fixture.OtherUserId, created.Id),
                CancellationToken.None));
    }

    [Fact]
    public async Task CreateDraftRejectsUnknownRuleset()
    {
        await using var fixture = await CharacterTestFixture.CreateAsync();
        var createHandler = new CreateCharacterDraftCommandHandler(fixture.CharactersRepository);

        await Assert.ThrowsAsync<RulesetNotFoundException>(() =>
            createHandler.Handle(
                new CreateCharacterDraftCommand(fixture.OwnerUserId, Guid.NewGuid(), "Mira Vale"),
                CancellationToken.None));
    }

    private sealed class CharacterTestFixture : IAsyncDisposable
    {
        private CharacterTestFixture(
            SqliteConnection connection,
            AppDbContext dbContext,
            Guid ownerUserId,
            Guid otherUserId,
            Guid rulesetId)
        {
            Connection = connection;
            DbContext = dbContext;
            OwnerUserId = ownerUserId;
            OtherUserId = otherUserId;
            RulesetId = rulesetId;
            CharactersRepository = new CharactersRepository(dbContext);
        }

        public SqliteConnection Connection { get; }

        public AppDbContext DbContext { get; }

        public CharactersRepository CharactersRepository { get; }

        public Guid OwnerUserId { get; }

        public Guid OtherUserId { get; }

        public Guid RulesetId { get; }

        public static async Task<CharacterTestFixture> CreateAsync()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;
            var dbContext = new AppDbContext(options);
            await dbContext.Database.EnsureCreatedAsync();

            var passwordHash = new PasswordHashService().HashPassword("CorrectHorseBatteryStaple!42");
            var owner = User.Create("owner@example.com", "Owner", passwordHash);
            var otherUser = User.Create("other@example.com", "Other", passwordHash);
            dbContext.Users.AddRange(owner, otherUser);
            await dbContext.SaveChangesAsync();

            var rulesetId = await dbContext.Rulesets
                .Select(ruleset => ruleset.Id)
                .SingleAsync();

            return new CharacterTestFixture(connection, dbContext, owner.Id, otherUser.Id, rulesetId);
        }

        public async ValueTask DisposeAsync()
        {
            await DbContext.DisposeAsync();
            await Connection.DisposeAsync();
        }
    }
}
