namespace GenesysForge.Domain.Characters;

public sealed class XpLedgerEntry : Entity
{
    public const int MaxReasonLength = 240;

    private XpLedgerEntry()
    {
    }

    private XpLedgerEntry(Guid id, Guid characterId, int amount, string reason, DateTimeOffset createdAt)
    {
        if (amount == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "XP amount cannot be zero.");
        }

        Id = id;
        CharacterId = Character.RequireGuid(characterId, nameof(characterId));
        Amount = amount;
        Reason = Character.NormalizeRequired(reason, nameof(reason), MaxReasonLength);
        CreatedAt = createdAt;
    }

    public Guid CharacterId { get; private set; }

    public int Amount { get; private set; }

    public string Reason { get; private set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; private set; }

    public static XpLedgerEntry Create(
        Guid characterId,
        int amount,
        string reason,
        DateTimeOffset? createdAt = null,
        Guid? id = null)
    {
        return new XpLedgerEntry(id ?? Guid.NewGuid(), characterId, amount, reason, createdAt ?? DateTimeOffset.UtcNow);
    }
}
