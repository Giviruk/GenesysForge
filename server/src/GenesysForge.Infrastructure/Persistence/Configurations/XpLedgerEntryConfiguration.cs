using GenesysForge.Domain.Characters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenesysForge.Infrastructure.Persistence.Configurations;

public sealed class XpLedgerEntryConfiguration : IEntityTypeConfiguration<XpLedgerEntry>
{
    public void Configure(EntityTypeBuilder<XpLedgerEntry> builder)
    {
        builder.ToTable("XpLedgerEntries");

        builder.HasKey(entry => entry.Id);

        builder.Property(entry => entry.Reason)
            .HasMaxLength(XpLedgerEntry.MaxReasonLength)
            .IsRequired();

        builder.Property(entry => entry.CreatedAt)
            .IsRequired();

        builder.HasOne<Character>()
            .WithMany(character => character.XpLedgerEntries)
            .HasForeignKey(entry => entry.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(entry => entry.CharacterId);
    }
}
