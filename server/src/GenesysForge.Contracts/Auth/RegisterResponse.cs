namespace GenesysForge.Contracts.Auth;

public sealed record RegisterResponse(
    Guid UserId,
    string Email,
    string DisplayName);
