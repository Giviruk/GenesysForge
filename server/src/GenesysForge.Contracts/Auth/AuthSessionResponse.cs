namespace GenesysForge.Contracts.Auth;

public sealed record AuthSessionResponse(
    string AccessToken,
    DateTimeOffset ExpiresAt,
    UserProfileDto User);
