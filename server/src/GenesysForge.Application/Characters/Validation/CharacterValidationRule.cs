using GenesysForge.Contracts.Validation;

namespace GenesysForge.Application.Characters.Validation;

internal sealed record CharacterValidationRule(
    string Code,
    ValidationSeverity Severity,
    Func<CharacterValidationContext, string> Message,
    string? Path,
    Func<CharacterValidationContext, bool> IsBroken)
{
    public static CharacterValidationRule Error(
        string code,
        string message,
        string? path,
        Func<CharacterValidationContext, bool> isBroken)
    {
        return Error(code, _ => message, path, isBroken);
    }

    public static CharacterValidationRule Error(
        string code,
        Func<CharacterValidationContext, string> message,
        string? path,
        Func<CharacterValidationContext, bool> isBroken)
    {
        return new CharacterValidationRule(
            code,
            ValidationSeverity.Error,
            message,
            path,
            isBroken);
    }

    public static CharacterValidationRule Warning(
        string code,
        string message,
        string? path,
        Func<CharacterValidationContext, bool> isBroken)
    {
        return Warning(code, _ => message, path, isBroken);
    }

    public static CharacterValidationRule Warning(
        string code,
        Func<CharacterValidationContext, string> message,
        string? path,
        Func<CharacterValidationContext, bool> isBroken)
    {
        return new CharacterValidationRule(
            code,
            ValidationSeverity.Warning,
            message,
            path,
            isBroken);
    }

    public ValidationMessageDto ToMessage(CharacterValidationContext context)
    {
        return new ValidationMessageDto(Code, Message(context), Severity, Path);
    }
}
