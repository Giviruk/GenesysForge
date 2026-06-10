namespace GenesysForge.Domain.Rules;

public sealed class Ruleset : Entity
{
    public const int MaxNameLength = 120;
    public const int MaxVersionLength = 40;
    public const int MaxDescriptionLength = 1000;

    private Ruleset()
    {
    }

    private Ruleset(Guid id, string name, string version, string? description)
    {
        Id = id;
        Name = RuleText.Required(name, nameof(name), MaxNameLength);
        Version = RuleText.Required(version, nameof(version), MaxVersionLength);
        Description = RuleText.Optional(description, nameof(description), MaxDescriptionLength);
    }

    public string Name { get; private set; } = string.Empty;

    public string Version { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public static Ruleset Create(string name, string version, string? description = null, Guid? id = null)
    {
        return new Ruleset(id ?? Guid.NewGuid(), name, version, description);
    }
}
