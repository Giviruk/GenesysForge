namespace GenesysForge.Application.Auth;

public sealed class InvalidCredentialsException()
    : UnauthorizedAccessException("Invalid email or password.");
