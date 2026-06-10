namespace GenesysForge.Application.Auth;

public sealed class EmailAlreadyRegisteredException(string email)
    : InvalidOperationException($"Email '{email}' is already registered.");
