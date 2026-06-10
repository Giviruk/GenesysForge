namespace GenesysForge.Application.Auth;

public sealed class InvalidCredentialsException()
    : UnauthorizedAccessException("Неверный email или пароль.");
