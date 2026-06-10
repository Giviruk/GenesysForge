namespace GenesysForge.Contracts.Auth;

public sealed record RegisterRequest(
    string Email,
    string Password,
    string DisplayName);

public sealed record RegisterResponse(
    Guid UserId,
    string Email,
    string DisplayName);

public sealed record LoginRequest(
    string Email,
    string Password);

public sealed record AuthSessionResponse(
    string AccessToken,
    DateTimeOffset ExpiresAt,
    UserProfileDto User);

public sealed record UserProfileDto(
    Guid Id,
    string Email,
    string DisplayName);
