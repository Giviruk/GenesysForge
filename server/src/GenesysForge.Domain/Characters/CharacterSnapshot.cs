namespace GenesysForge.Domain.Characters;

public sealed class CharacterSnapshot : Entity
{
    private CharacterSnapshot()
    {
    }

    private CharacterSnapshot(Guid id, Guid characterId, Guid rulesetId, string contentJson, DateTimeOffset createdAt)
    {
        Id = id;
        CharacterId = Character.RequireGuid(characterId, nameof(characterId));
        RulesetId = Character.RequireGuid(rulesetId, nameof(rulesetId));
        ContentJson = Character.NormalizeRequired(contentJson, nameof(contentJson), int.MaxValue);
        CreatedAt = createdAt;
    }

    public Guid CharacterId { get; private set; }

    public Guid RulesetId { get; private set; }

    public string ContentJson { get; private set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; private set; }

    public static CharacterSnapshot Create(
        Guid characterId,
        Guid rulesetId,
        string contentJson,
        DateTimeOffset? createdAt = null,
        Guid? id = null)
    {
        return new CharacterSnapshot(id ?? Guid.NewGuid(), characterId, rulesetId, contentJson, createdAt ?? DateTimeOffset.UtcNow);
    }
}
