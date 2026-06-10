using GenesysForge.Application.Auth;
using GenesysForge.Application.Auth.Login;
using GenesysForge.Application.Auth.Register;
using GenesysForge.Domain.Users;
using GenesysForge.Infrastructure.Auth;
using GenesysForge.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GenesysForge.Tests.Application;

public sealed class AuthCommandTests
{
    [Fact]
    public async Task RegisterCreatesUser()
    {
        await using var fixture = await AuthTestFixture.CreateAsync();
        var handler = new RegisterCommandHandler(fixture.UserRepository, fixture.PasswordHashService);

        var response = await handler.Handle(
            new RegisterCommand("Player@Example.com", "CorrectHorseBatteryStaple!42", "Player One"),
            CancellationToken.None);

        var savedUser = await fixture.DbContext.Users.SingleAsync(CancellationToken.None);
        Assert.Equal(savedUser.Id, response.UserId);
        Assert.Equal("player@example.com", response.Email);
        Assert.Equal("Player One", response.DisplayName);
        Assert.True(fixture.PasswordHashService.VerifyPassword(savedUser.PasswordHash, "CorrectHorseBatteryStaple!42"));
    }

    [Fact]
    public async Task RegisterRejectsDuplicateEmail()
    {
        await using var fixture = await AuthTestFixture.CreateAsync();
        var handler = new RegisterCommandHandler(fixture.UserRepository, fixture.PasswordHashService);
        var command = new RegisterCommand("player@example.com", "CorrectHorseBatteryStaple!42", "Player One");

        await handler.Handle(command, CancellationToken.None);

        await Assert.ThrowsAsync<EmailAlreadyRegisteredException>(() =>
            handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task LoginReturnsSessionForValidCredentials()
    {
        await using var fixture = await AuthTestFixture.CreateAsync();
        var passwordHash = fixture.PasswordHashService.HashPassword("CorrectHorseBatteryStaple!42");
        var user = User.Create("player@example.com", "Player One", passwordHash);
        fixture.DbContext.Users.Add(user);
        await fixture.DbContext.SaveChangesAsync(CancellationToken.None);
        var handler = new LoginCommandHandler(
            fixture.UserRepository,
            fixture.PasswordHashService,
            fixture.AccessTokenService);

        var response = await handler.Handle(
            new LoginCommand("PLAYER@example.com", "CorrectHorseBatteryStaple!42"),
            CancellationToken.None);

        Assert.Equal("test-token", response.AccessToken);
        Assert.Equal(user.Id, response.User.Id);
        Assert.Equal("player@example.com", response.User.Email);
    }

    [Fact]
    public async Task LoginRejectsInvalidPassword()
    {
        await using var fixture = await AuthTestFixture.CreateAsync();
        var passwordHash = fixture.PasswordHashService.HashPassword("CorrectHorseBatteryStaple!42");
        fixture.DbContext.Users.Add(User.Create("player@example.com", "Player One", passwordHash));
        await fixture.DbContext.SaveChangesAsync(CancellationToken.None);
        var handler = new LoginCommandHandler(
            fixture.UserRepository,
            fixture.PasswordHashService,
            fixture.AccessTokenService);

        await Assert.ThrowsAsync<InvalidCredentialsException>(() =>
            handler.Handle(new LoginCommand("player@example.com", "wrong-password"), CancellationToken.None));
    }

    private sealed class AuthTestFixture : IAsyncDisposable
    {
        private AuthTestFixture(SqliteConnection connection, AppDbContext dbContext)
        {
            Connection = connection;
            DbContext = dbContext;
            UserRepository = new UserRepository(dbContext);
        }

        public SqliteConnection Connection { get; }

        public AppDbContext DbContext { get; }

        public UserRepository UserRepository { get; }

        public PasswordHashService PasswordHashService { get; } = new();

        public IAccessTokenService AccessTokenService { get; } = new FixedAccessTokenService();

        public static async Task<AuthTestFixture> CreateAsync()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;
            var dbContext = new AppDbContext(options);
            await dbContext.Database.EnsureCreatedAsync();

            return new AuthTestFixture(connection, dbContext);
        }

        public async ValueTask DisposeAsync()
        {
            await DbContext.DisposeAsync();
            await Connection.DisposeAsync();
        }
    }

    private sealed class FixedAccessTokenService : IAccessTokenService
    {
        public AccessToken CreateAccessToken(User user)
        {
            return new AccessToken("test-token", DateTimeOffset.UtcNow.AddMinutes(10));
        }
    }
}
