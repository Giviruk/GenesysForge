using GenesysForge.Domain.Characters;
using GenesysForge.Domain.Rules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenesysForge.Infrastructure.Persistence.Configurations;

public sealed class CharacterTalentConfiguration : IEntityTypeConfiguration<CharacterTalent>
{
    public void Configure(EntityTypeBuilder<CharacterTalent> builder)
    {
        builder.ToTable("CharacterTalents");

        builder.HasKey(talent => talent.Id);

        builder.Property(talent => talent.XpCost)
            .IsRequired();

        builder.Property(talent => talent.PurchasedAt)
            .IsRequired();

        builder.HasOne<Character>()
            .WithMany(character => character.Talents)
            .HasForeignKey(talent => talent.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<RuleEntity>()
            .WithMany()
            .HasForeignKey(talent => talent.RuleEntityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(talent => new { talent.CharacterId, talent.RuleEntityId })
            .IsUnique();
    }
}
