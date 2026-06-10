namespace GenesysForge.Domain.Users;

public sealed class User : Entity
{
    public const int MaxEmailLength = 320;
    public const int MaxNormalizedEmailLength = 320;
    public const int MaxDisplayNameLength = 100;
    public const int MaxPasswordHashLength = 512;

    private User()
    {
    }

    private User(string email, string displayName, string passwordHash, DateTimeOffset createdAt)
    {
        Email = NormalizeRequired(email, nameof(email), MaxEmailLength).ToLowerInvariant();
        NormalizedEmail = Email.ToUpperInvariant();
        DisplayName = NormalizeRequired(displayName, nameof(displayName), MaxDisplayNameLength);
        PasswordHash = NormalizeRequired(passwordHash, nameof(passwordHash), MaxPasswordHashLength);
        CreatedAt = createdAt;
    }

    public string Email { get; private set; } = string.Empty;

    public string NormalizedEmail { get; private set; } = string.Empty;

    public string DisplayName { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset? UpdatedAt { get; private set; }

    public static User Create(string email, string displayName, string passwordHash, DateTimeOffset? createdAt = null)
    {
        return new User(email, displayName, passwordHash, createdAt ?? DateTimeOffset.UtcNow);
    }

    public void Rename(string displayName, DateTimeOffset? updatedAt = null)
    {
        DisplayName = NormalizeRequired(displayName, nameof(displayName), MaxDisplayNameLength);
        UpdatedAt = updatedAt ?? DateTimeOffset.UtcNow;
    }

    public void ChangePasswordHash(string passwordHash, DateTimeOffset? updatedAt = null)
    {
        PasswordHash = NormalizeRequired(passwordHash, nameof(passwordHash), MaxPasswordHashLength);
        UpdatedAt = updatedAt ?? DateTimeOffset.UtcNow;
    }

    private static string NormalizeRequired(string value, string parameterName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be empty.", parameterName);
        }

        var normalized = value.Trim();
        if (normalized.Length > maxLength)
        {
            throw new ArgumentOutOfRangeException(parameterName, $"Value cannot exceed {maxLength} characters.");
        }

        return normalized;
    }
}
