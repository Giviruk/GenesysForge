using GenesysForge.Domain.Rules;
using GenesysForge.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenesysForge.Infrastructure.Persistence.Configurations;

public sealed class RulesetConfiguration : IEntityTypeConfiguration<Ruleset>
{
    public void Configure(EntityTypeBuilder<Ruleset> builder)
    {
        builder.ToTable("Rulesets");

        builder.HasKey(ruleset => ruleset.Id);

        builder.Property(ruleset => ruleset.Name)
            .HasMaxLength(Ruleset.MaxNameLength)
            .IsRequired();

        builder.Property(ruleset => ruleset.Version)
            .HasMaxLength(Ruleset.MaxVersionLength)
            .IsRequired();

        builder.Property(ruleset => ruleset.Description)
            .HasMaxLength(Ruleset.MaxDescriptionLength);

        builder.HasData(RulesetSeedData.Ruleset);
    }
}
