namespace GenesysForge.Contracts.Auth;

public sealed record RegisterRequest(
    string Email,
    string Password,
    string DisplayName);
