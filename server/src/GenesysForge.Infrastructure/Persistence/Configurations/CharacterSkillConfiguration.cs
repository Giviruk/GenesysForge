using GenesysForge.Domain.Characters;
using GenesysForge.Domain.Rules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenesysForge.Infrastructure.Persistence.Configurations;

public sealed class CharacterSkillConfiguration : IEntityTypeConfiguration<CharacterSkill>
{
    public void Configure(EntityTypeBuilder<CharacterSkill> builder)
    {
        builder.ToTable("CharacterSkills");

        builder.HasKey(skill => skill.Id);

        builder.Property(skill => skill.XpSpent)
            .IsRequired();

        builder.Property(skill => skill.UpdatedAt)
            .IsRequired();

        builder.HasOne<Character>()
            .WithMany(character => character.Skills)
            .HasForeignKey(skill => skill.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<RuleEntity>()
            .WithMany()
            .HasForeignKey(skill => skill.RuleEntityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(skill => new { skill.CharacterId, skill.RuleEntityId })
            .IsUnique();
    }
}
