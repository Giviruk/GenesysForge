namespace GenesysForge.Domain.Characters;

public sealed class CharacterTalent : Entity
{
    private CharacterTalent()
    {
    }

    private CharacterTalent(Guid id, Guid characterId, Guid ruleEntityId, int xpCost, DateTimeOffset purchasedAt)
    {
        Id = id;
        CharacterId = Character.RequireGuid(characterId, nameof(characterId));
        RuleEntityId = Character.RequireGuid(ruleEntityId, nameof(ruleEntityId));
        XpCost = Character.NormalizePositiveXp(xpCost, nameof(xpCost));
        PurchasedAt = purchasedAt;
    }

    public Guid CharacterId { get; private set; }

    public Guid RuleEntityId { get; private set; }

    public int XpCost { get; private set; }

    public DateTimeOffset PurchasedAt { get; private set; }

    public static CharacterTalent Create(
        Guid characterId,
        Guid ruleEntityId,
        int xpCost,
        DateTimeOffset? purchasedAt = null,
        Guid? id = null)
    {
        return new CharacterTalent(id ?? Guid.NewGuid(), characterId, ruleEntityId, xpCost, purchasedAt ?? DateTimeOffset.UtcNow);
    }
}
