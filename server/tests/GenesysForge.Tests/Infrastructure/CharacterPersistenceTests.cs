using GenesysForge.Domain.Characters;
using GenesysForge.Domain.Users;
using GenesysForge.Infrastructure.Auth;
using GenesysForge.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GenesysForge.Tests.Infrastructure;

public sealed class CharacterPersistenceTests
{
    [Fact]
    public async Task DraftCharacterCanBeSavedAndLoaded()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;
        var passwordHash = new PasswordHashService().HashPassword("CorrectHorseBatteryStaple!42");

        Guid characterId;

        await using (var dbContext = new AppDbContext(options))
        {
            await dbContext.Database.EnsureCreatedAsync();

            var user = User.Create("player@example.com", "Player One", passwordHash);
            var ruleset = await dbContext.Rulesets.SingleAsync();
            var resolveSkill = await dbContext.RuleEntities.SingleAsync(entity => entity.Key == "resolve");
            var guardianTalent = await dbContext.RuleEntities.SingleAsync(entity => entity.Key == "guardian");

            var character = Character.CreateDraft(user.Id, ruleset.Id, "Mira Vale");
            character.GrantXp(100, "Starting XP");
            character.SpendXp(25, "Bought starting talent");
            character.SetSkillRank(resolveSkill.Id, 2, xpSpent: 25, isCareerSkill: true);
            character.PurchaseTalent(guardianTalent.Id, xpCost: 25);
            character.AddSnapshot("""{"sourceVersions":[],"entities":[],"rules":[]}""");

            dbContext.Users.Add(user);
            dbContext.Characters.Add(character);

            await dbContext.SaveChangesAsync();
            characterId = character.Id;
        }

        await using (var dbContext = new AppDbContext(options))
        {
            var savedCharacter = await dbContext.Characters
                .Include(character => character.Snapshots)
                .Include(character => character.XpLedgerEntries)
                .Include(character => character.Skills)
                .Include(character => character.Talents)
                .SingleAsync(character => character.Id == characterId);

            Assert.Equal("Mira Vale", savedCharacter.Name);
            Assert.Equal(CharacterStatus.Draft, savedCharacter.Status);
            Assert.Equal(100, savedCharacter.TotalXp);
            Assert.Equal(25, savedCharacter.SpentXp);
            Assert.Equal(75, savedCharacter.AvailableXp);
            Assert.Single(savedCharacter.Snapshots);
            var skill = Assert.Single(savedCharacter.Skills);
            var talent = Assert.Single(savedCharacter.Talents);
            Assert.Equal(25, skill.XpSpent);
            Assert.Equal(25, talent.XpCost);
        }
    }
}
