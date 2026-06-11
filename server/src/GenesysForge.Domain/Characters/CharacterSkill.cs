namespace GenesysForge.Domain.Characters;

public sealed class CharacterSkill : Entity
{
    public const int MaxRank = 5;

    private CharacterSkill()
    {
    }

    private CharacterSkill(
        Guid id,
        Guid characterId,
        Guid ruleEntityId,
        int rank,
        int xpSpent,
        bool isCareerSkill,
        DateTimeOffset updatedAt)
    {
        Id = id;
        CharacterId = Character.RequireGuid(characterId, nameof(characterId));
        RuleEntityId = Character.RequireGuid(ruleEntityId, nameof(ruleEntityId));
        Rank = NormalizeRank(rank);
        XpSpent = Character.NormalizeNonNegativeXp(xpSpent, nameof(xpSpent));
        IsCareerSkill = isCareerSkill;
        UpdatedAt = updatedAt;
    }

    public Guid CharacterId { get; private set; }

    public Guid RuleEntityId { get; private set; }

    public int Rank { get; private set; }

    public int XpSpent { get; private set; }

    public bool IsCareerSkill { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static CharacterSkill Create(
        Guid characterId,
        Guid ruleEntityId,
        int rank,
        int xpSpent,
        bool isCareerSkill,
        DateTimeOffset? updatedAt = null,
        Guid? id = null)
    {
        return new CharacterSkill(
            id ?? Guid.NewGuid(),
            characterId,
            ruleEntityId,
            rank,
            xpSpent,
            isCareerSkill,
            updatedAt ?? DateTimeOffset.UtcNow);
    }

    public void ChangeRank(int rank, int xpSpent, bool isCareerSkill, DateTimeOffset? updatedAt = null)
    {
        Rank = NormalizeRank(rank);
        XpSpent = Character.NormalizeNonNegativeXp(xpSpent, nameof(xpSpent));
        IsCareerSkill = isCareerSkill;
        UpdatedAt = updatedAt ?? DateTimeOffset.UtcNow;
    }

    private static int NormalizeRank(int rank)
    {
        if (rank is < 0 or > MaxRank)
        {
            throw new ArgumentOutOfRangeException(nameof(rank), $"Skill rank must be between 0 and {MaxRank}.");
        }

        return rank;
    }
}
