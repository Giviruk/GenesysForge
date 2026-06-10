using GenesysForge.Domain.Rules;
using GenesysForge.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenesysForge.Infrastructure.Persistence.Configurations;

public sealed class SourceBookConfiguration : IEntityTypeConfiguration<SourceBook>
{
    public void Configure(EntityTypeBuilder<SourceBook> builder)
    {
        builder.ToTable("SourceBooks");

        builder.HasKey(sourceBook => sourceBook.Id);

        builder.Property(sourceBook => sourceBook.Key)
            .HasMaxLength(SourceBook.MaxKeyLength)
            .IsRequired();

        builder.Property(sourceBook => sourceBook.Name)
            .HasMaxLength(SourceBook.MaxNameLength)
            .IsRequired();

        builder.HasOne<Ruleset>()
            .WithMany()
            .HasForeignKey(sourceBook => sourceBook.RulesetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(sourceBook => new { sourceBook.RulesetId, sourceBook.Key })
            .IsUnique();

        builder.HasData(RulesetSeedData.SourceBook);
    }
}
