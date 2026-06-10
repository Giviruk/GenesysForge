using GenesysForge.Domain.Rules;
using GenesysForge.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenesysForge.Infrastructure.Persistence.Configurations;

public sealed class RuleDefinitionConfiguration : IEntityTypeConfiguration<RuleDefinition>
{
    public void Configure(EntityTypeBuilder<RuleDefinition> builder)
    {
        builder.ToTable("RuleDefinitions");

        builder.HasKey(ruleDefinition => ruleDefinition.Id);

        builder.Property(ruleDefinition => ruleDefinition.Key)
            .HasMaxLength(RuleDefinition.MaxKeyLength)
            .IsRequired();

        builder.Property(ruleDefinition => ruleDefinition.ContentJson)
            .IsRequired();

        builder.HasOne<RuleEntity>()
            .WithMany()
            .HasForeignKey(ruleDefinition => ruleDefinition.RuleEntityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<RuleSourceVersion>()
            .WithMany()
            .HasForeignKey(ruleDefinition => ruleDefinition.SourceVersionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ruleDefinition => new
            {
                ruleDefinition.RuleEntityId,
                ruleDefinition.SourceVersionId,
                ruleDefinition.Key
            })
            .IsUnique();

        builder.HasData(RulesetSeedData.Definitions);
    }
}
