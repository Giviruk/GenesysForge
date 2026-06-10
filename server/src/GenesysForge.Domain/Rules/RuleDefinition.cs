namespace GenesysForge.Domain.Rules;

public sealed class RuleDefinition : Entity
{
    public const int MaxKeyLength = 100;

    private RuleDefinition()
    {
    }

    private RuleDefinition(Guid id, Guid ruleEntityId, Guid sourceVersionId, string key, string contentJson)
    {
        Id = id;
        RuleEntityId = ruleEntityId;
        SourceVersionId = sourceVersionId;
        Key = RuleText.Key(key, nameof(key), MaxKeyLength);
        ContentJson = RuleText.Required(contentJson, nameof(contentJson), int.MaxValue);
    }

    public Guid RuleEntityId { get; private set; }

    public Guid SourceVersionId { get; private set; }

    public string Key { get; private set; } = string.Empty;

    public string ContentJson { get; private set; } = string.Empty;

    public static RuleDefinition Create(
        Guid ruleEntityId,
        Guid sourceVersionId,
        string key,
        string contentJson,
        Guid? id = null)
    {
        return new RuleDefinition(id ?? Guid.NewGuid(), ruleEntityId, sourceVersionId, key, contentJson);
    }
}
