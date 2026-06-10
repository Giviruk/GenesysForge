namespace GenesysForge.Contracts.Validation;

public sealed record ValidationMessageDto(
    string Code,
    string Message,
    ValidationSeverity Severity,
    string? Path);
