namespace GenesysForge.Domain.Characters;

public sealed class Character : Entity
{
    public const int MaxNameLength = 120;

    private readonly List<CharacterSnapshot> _snapshots = [];
    private readonly List<XpLedgerEntry> _xpLedgerEntries = [];
    private readonly List<CharacterSkill> _skills = [];
    private readonly List<CharacterTalent> _talents = [];

    private Character()
    {
    }

    private Character(Guid id, Guid ownerUserId, Guid rulesetId, string name, DateTimeOffset createdAt)
    {
        Id = id;
        OwnerUserId = RequireGuid(ownerUserId, nameof(ownerUserId));
        RulesetId = RequireGuid(rulesetId, nameof(rulesetId));
        Name = NormalizeRequired(name, nameof(name), MaxNameLength);
        Status = CharacterStatus.Draft;
        CreatedAt = createdAt;
    }

    public Guid OwnerUserId { get; private set; }

    public Guid RulesetId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public CharacterStatus Status { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset? UpdatedAt { get; private set; }

    public IReadOnlyCollection<CharacterSnapshot> Snapshots => _snapshots;

    public IReadOnlyCollection<XpLedgerEntry> XpLedgerEntries => _xpLedgerEntries;

    public IReadOnlyCollection<CharacterSkill> Skills => _skills;

    public IReadOnlyCollection<CharacterTalent> Talents => _talents;

    public int TotalXp => _xpLedgerEntries.Where(entry => entry.Amount > 0).Sum(entry => entry.Amount);

    public int SpentXp => _xpLedgerEntries.Where(entry => entry.Amount < 0).Sum(entry => Math.Abs(entry.Amount));

    public int AvailableXp => _xpLedgerEntries.Sum(entry => entry.Amount);

    public static Character CreateDraft(
        Guid ownerUserId,
        Guid rulesetId,
        string name,
        DateTimeOffset? createdAt = null,
        Guid? id = null)
    {
        return new Character(id ?? Guid.NewGuid(), ownerUserId, rulesetId, name, createdAt ?? DateTimeOffset.UtcNow);
    }

    public void Rename(string name, DateTimeOffset? updatedAt = null)
    {
        Name = NormalizeRequired(name, nameof(name), MaxNameLength);
        UpdatedAt = updatedAt ?? DateTimeOffset.UtcNow;
    }

    public CharacterSnapshot AddSnapshot(string contentJson, DateTimeOffset? createdAt = null, Guid? id = null)
    {
        var snapshot = CharacterSnapshot.Create(Id, RulesetId, contentJson, createdAt, id);
        _snapshots.Add(snapshot);
        return snapshot;
    }

    public XpLedgerEntry GrantXp(int amount, string reason, DateTimeOffset? createdAt = null, Guid? id = null)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "XP grant amount must be positive.");
        }

        var entry = XpLedgerEntry.Create(Id, amount, reason, createdAt, id);
        _xpLedgerEntries.Add(entry);
        return entry;
    }

    public XpLedgerEntry SpendXp(int amount, string reason, DateTimeOffset? createdAt = null, Guid? id = null)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "XP spend amount must be positive.");
        }

        if (amount > AvailableXp)
        {
            throw new InvalidOperationException("Cannot spend more XP than the character has available.");
        }

        var entry = XpLedgerEntry.Create(Id, -amount, reason, createdAt, id);
        _xpLedgerEntries.Add(entry);
        return entry;
    }

    public CharacterSkill SetSkillRank(
        Guid ruleEntityId,
        int rank,
        int xpSpent,
        bool isCareerSkill,
        DateTimeOffset? updatedAt = null,
        Guid? id = null)
    {
        var existingSkill = _skills.SingleOrDefault(skill => skill.RuleEntityId == ruleEntityId);
        if (existingSkill is not null)
        {
            existingSkill.ChangeRank(rank, xpSpent, isCareerSkill, updatedAt);
            return existingSkill;
        }

        var skill = CharacterSkill.Create(Id, ruleEntityId, rank, xpSpent, isCareerSkill, updatedAt, id);
        _skills.Add(skill);
        return skill;
    }

    public CharacterTalent PurchaseTalent(Guid ruleEntityId, int xpCost, DateTimeOffset? purchasedAt = null, Guid? id = null)
    {
        if (_talents.Any(talent => talent.RuleEntityId == ruleEntityId))
        {
            throw new InvalidOperationException("Talent is already purchased by this character.");
        }

        var talent = CharacterTalent.Create(Id, ruleEntityId, xpCost, purchasedAt, id);
        _talents.Add(talent);
        return talent;
    }

    internal static Guid RequireGuid(Guid value, string parameterName)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Value cannot be empty.", parameterName);
        }

        return value;
    }

    internal static string NormalizeRequired(string value, string parameterName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be empty.", parameterName);
        }

        var normalized = value.Trim();
        if (normalized.Length > maxLength)
        {
            throw new ArgumentOutOfRangeException(parameterName, $"Value cannot exceed {maxLength} characters.");
        }

        return normalized;
    }

    internal static int NormalizeNonNegativeXp(int value, string parameterName)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(parameterName, "XP value cannot be negative.");
        }

        return value;
    }

    internal static int NormalizePositiveXp(int value, string parameterName)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(parameterName, "XP value must be positive.");
        }

        return value;
    }
}
