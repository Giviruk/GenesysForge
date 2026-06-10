using GenesysForge.Domain.Rules;
using GenesysForge.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenesysForge.Infrastructure.Persistence.Configurations;

public sealed class RuleSourceVersionConfiguration : IEntityTypeConfiguration<RuleSourceVersion>
{
    public void Configure(EntityTypeBuilder<RuleSourceVersion> builder)
    {
        builder.ToTable("RuleSourceVersions");

        builder.HasKey(sourceVersion => sourceVersion.Id);

        builder.Property(sourceVersion => sourceVersion.Version)
            .HasMaxLength(RuleSourceVersion.MaxVersionLength)
            .IsRequired();

        builder.Property(sourceVersion => sourceVersion.IsActive)
            .IsRequired();

        builder.HasOne<SourceBook>()
            .WithMany()
            .HasForeignKey(sourceVersion => sourceVersion.SourceBookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(sourceVersion => new { sourceVersion.SourceBookId, sourceVersion.Version })
            .IsUnique();

        builder.HasData(RulesetSeedData.SourceVersion);
    }
}
