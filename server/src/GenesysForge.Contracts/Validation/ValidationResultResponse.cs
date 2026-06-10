namespace GenesysForge.Contracts.Validation;

public sealed record ValidationResultResponse(
    bool IsValid,
    IReadOnlyCollection<ValidationMessageDto> Messages);
