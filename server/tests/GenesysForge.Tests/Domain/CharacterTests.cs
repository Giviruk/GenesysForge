using GenesysForge.Domain.Characters;

namespace GenesysForge.Tests.Domain;

public sealed class CharacterTests
{
    [Fact]
    public void DraftCharacterCanTrackCoreState()
    {
        var ownerUserId = Guid.NewGuid();
        var rulesetId = Guid.NewGuid();
        var skillEntityId = Guid.NewGuid();
        var talentEntityId = Guid.NewGuid();
        var createdAt = new DateTimeOffset(2026, 6, 10, 12, 0, 0, TimeSpan.Zero);

        var character = Character.CreateDraft(ownerUserId, rulesetId, "  Mira Vale  ", createdAt);

        character.GrantXp(100, "Starting XP", createdAt.AddMinutes(1));
        character.SpendXp(25, "Bought starting talent", createdAt.AddMinutes(2));
        character.SetSkillRank(skillEntityId, 2, xpSpent: 25, isCareerSkill: true, updatedAt: createdAt.AddMinutes(3));
        character.PurchaseTalent(talentEntityId, xpCost: 25, purchasedAt: createdAt.AddMinutes(4));
        character.AddSnapshot("""{"sourceVersions":[],"entities":[],"rules":[]}""", createdAt.AddMinutes(5));

        Assert.Equal(ownerUserId, character.OwnerUserId);
        Assert.Equal(rulesetId, character.RulesetId);
        Assert.Equal("Mira Vale", character.Name);
        Assert.Equal(CharacterStatus.Draft, character.Status);
        Assert.Equal(100, character.TotalXp);
        Assert.Equal(25, character.SpentXp);
        Assert.Equal(75, character.AvailableXp);
        var skill = Assert.Single(character.Skills);
        var talent = Assert.Single(character.Talents);
        Assert.Equal(25, skill.XpSpent);
        Assert.Equal(25, talent.XpCost);
        Assert.Single(character.Snapshots);
    }

    [Fact]
    public void CharacterCannotSpendMoreXpThanAvailable()
    {
        var character = Character.CreateDraft(Guid.NewGuid(), Guid.NewGuid(), "Mira Vale");

        var exception = Assert.Throws<InvalidOperationException>(() => character.SpendXp(1, "Too much"));

        Assert.Equal("Cannot spend more XP than the character has available.", exception.Message);
    }

    [Fact]
    public void CharacterCannotPurchaseTheSameTalentTwice()
    {
        var talentEntityId = Guid.NewGuid();
        var character = Character.CreateDraft(Guid.NewGuid(), Guid.NewGuid(), "Mira Vale");
        character.PurchaseTalent(talentEntityId, xpCost: 5);

        var exception = Assert.Throws<InvalidOperationException>(() => character.PurchaseTalent(talentEntityId, xpCost: 5));

        Assert.Equal("Talent is already purchased by this character.", exception.Message);
    }

    [Fact]
    public void CharacterRejectsInvalidSkillAndTalentCosts()
    {
        var character = Character.CreateDraft(Guid.NewGuid(), Guid.NewGuid(), "Mira Vale");

        Assert.Throws<ArgumentOutOfRangeException>(
            () => character.SetSkillRank(Guid.NewGuid(), rank: 1, xpSpent: -1, isCareerSkill: true));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => character.PurchaseTalent(Guid.NewGuid(), xpCost: 0));
    }
}
