namespace GenesysForge.Domain.Rules;

public sealed class SourceBook : Entity
{
    public const int MaxKeyLength = 80;
    public const int MaxNameLength = 160;

    private SourceBook()
    {
    }

    private SourceBook(Guid id, Guid rulesetId, string key, string name)
    {
        Id = id;
        RulesetId = rulesetId;
        Key = RuleText.Key(key, nameof(key), MaxKeyLength);
        Name = RuleText.Required(name, nameof(name), MaxNameLength);
    }

    public Guid RulesetId { get; private set; }

    public string Key { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public static SourceBook Create(Guid rulesetId, string key, string name, Guid? id = null)
    {
        return new SourceBook(id ?? Guid.NewGuid(), rulesetId, key, name);
    }
}
