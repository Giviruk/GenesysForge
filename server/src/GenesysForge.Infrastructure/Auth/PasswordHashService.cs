using GenesysForge.Application.Auth;
using Microsoft.AspNetCore.Identity;

namespace GenesysForge.Infrastructure.Auth;

public sealed class PasswordHashService : IPasswordHashService
{
    private static readonly PasswordHashSubject Subject = new();

    private readonly PasswordHasher<PasswordHashSubject> passwordHasher = new();

    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be empty.", nameof(password));
        }

        return passwordHasher.HashPassword(Subject, password);
    }

    public bool VerifyPassword(string passwordHash, string password)
    {
        if (string.IsNullOrWhiteSpace(passwordHash) || string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        var result = passwordHasher.VerifyHashedPassword(Subject, passwordHash, password);
        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }

    private sealed class PasswordHashSubject
    {
    }
}
