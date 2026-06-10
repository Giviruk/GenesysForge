using GenesysForge.Infrastructure.Auth;

namespace GenesysForge.Tests.Infrastructure;

public sealed class PasswordHashServiceTests
{
    [Fact]
    public void HashPasswordCreatesVerifiableHash()
    {
        var service = new PasswordHashService();
        const string password = "CorrectHorseBatteryStaple!42";

        var hash = service.HashPassword(password);

        Assert.NotEqual(password, hash);
        Assert.True(service.VerifyPassword(hash, password));
        Assert.False(service.VerifyPassword(hash, "wrong-password"));
    }
}
