namespace GenesysForge.Domain.Rules;

public sealed class RuleSourceVersion : Entity
{
    public const int MaxVersionLength = 40;

    private RuleSourceVersion()
    {
    }

    private RuleSourceVersion(Guid id, Guid sourceBookId, string version, bool isActive)
    {
        Id = id;
        SourceBookId = sourceBookId;
        Version = RuleText.Required(version, nameof(version), MaxVersionLength);
        IsActive = isActive;
    }

    public Guid SourceBookId { get; private set; }

    public string Version { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    public static RuleSourceVersion Create(Guid sourceBookId, string version, bool isActive, Guid? id = null)
    {
        return new RuleSourceVersion(id ?? Guid.NewGuid(), sourceBookId, version, isActive);
    }
}
