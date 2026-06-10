using GenesysForge.Domain.Rules;
using GenesysForge.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenesysForge.Infrastructure.Persistence.Configurations;

public sealed class RuleEntityConfiguration : IEntityTypeConfiguration<RuleEntity>
{
    public void Configure(EntityTypeBuilder<RuleEntity> builder)
    {
        builder.ToTable("RuleEntities");

        builder.HasKey(ruleEntity => ruleEntity.Id);

        builder.Property(ruleEntity => ruleEntity.EntityType)
            .HasMaxLength(RuleEntity.MaxEntityTypeLength)
            .IsRequired();

        builder.Property(ruleEntity => ruleEntity.Key)
            .HasMaxLength(RuleEntity.MaxKeyLength)
            .IsRequired();

        builder.Property(ruleEntity => ruleEntity.Name)
            .HasMaxLength(RuleEntity.MaxNameLength)
            .IsRequired();

        builder.Property(ruleEntity => ruleEntity.Description)
            .HasMaxLength(RuleEntity.MaxDescriptionLength);

        builder.HasOne<Ruleset>()
            .WithMany()
            .HasForeignKey(ruleEntity => ruleEntity.RulesetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ruleEntity => new { ruleEntity.RulesetId, ruleEntity.EntityType, ruleEntity.Key })
            .IsUnique();

        builder.HasData(RulesetSeedData.Entities);
    }
}
