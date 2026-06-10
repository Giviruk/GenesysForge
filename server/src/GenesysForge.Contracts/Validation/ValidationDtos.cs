namespace GenesysForge.Contracts.Validation;

public enum ValidationSeverity
{
    Error = 0,
    Warning = 1,
}

public sealed record ValidationMessageDto(
    string Code,
    string Message,
    ValidationSeverity Severity,
    string? Path);

public sealed record ValidationResultResponse(
    bool IsValid,
    IReadOnlyCollection<ValidationMessageDto> Messages);
