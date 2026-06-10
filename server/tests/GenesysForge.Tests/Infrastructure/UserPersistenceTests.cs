using GenesysForge.Domain.Users;
using GenesysForge.Infrastructure.Auth;
using GenesysForge.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GenesysForge.Tests.Infrastructure;

public sealed class UserPersistenceTests
{
    [Fact]
    public async Task UserCanBeSavedAndLoaded()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;
        var passwordHashService = new PasswordHashService();
        var passwordHash = passwordHashService.HashPassword("CorrectHorseBatteryStaple!42");

        await using (var dbContext = new AppDbContext(options))
        {
            await dbContext.Database.EnsureCreatedAsync();

            var user = User.Create("  Player@Example.com  ", "Player One", passwordHash);
            dbContext.Users.Add(user);

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new AppDbContext(options))
        {
            var savedUser = await dbContext.Users.SingleAsync();

            Assert.Equal("player@example.com", savedUser.Email);
            Assert.Equal("PLAYER@EXAMPLE.COM", savedUser.NormalizedEmail);
            Assert.Equal("Player One", savedUser.DisplayName);
            Assert.True(passwordHashService.VerifyPassword(savedUser.PasswordHash, "CorrectHorseBatteryStaple!42"));
        }
    }
}
