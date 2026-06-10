namespace GenesysForge.Application.Auth;

public sealed record AccessToken(
    string Value,
    DateTimeOffset ExpiresAt);
