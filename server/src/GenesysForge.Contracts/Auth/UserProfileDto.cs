namespace GenesysForge.Contracts.Auth;

public sealed record UserProfileDto(
    Guid Id,
    string Email,
    string DisplayName);
