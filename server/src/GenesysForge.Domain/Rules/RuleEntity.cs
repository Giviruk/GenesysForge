namespace GenesysForge.Domain.Rules;

public sealed class RuleEntity : Entity
{
    public const int MaxEntityTypeLength = 60;
    public const int MaxKeyLength = 100;
    public const int MaxNameLength = 160;
    public const int MaxDescriptionLength = 1000;

    private RuleEntity()
    {
    }

    private RuleEntity(Guid id, Guid rulesetId, string entityType, string key, string name, string? description)
    {
        Id = id;
        RulesetId = rulesetId;
        EntityType = RuleText.Key(entityType, nameof(entityType), MaxEntityTypeLength);
        Key = RuleText.Key(key, nameof(key), MaxKeyLength);
        Name = RuleText.Required(name, nameof(name), MaxNameLength);
        Description = RuleText.Optional(description, nameof(description), MaxDescriptionLength);
    }

    public Guid RulesetId { get; private set; }

    public string EntityType { get; private set; } = string.Empty;

    public string Key { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public static RuleEntity Create(
        Guid rulesetId,
        string entityType,
        string key,
        string name,
        string? description = null,
        Guid? id = null)
    {
        return new RuleEntity(id ?? Guid.NewGuid(), rulesetId, entityType, key, name, description);
    }
}
