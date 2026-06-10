using GenesysForge.Domain.Characters;
using GenesysForge.Domain.Rules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenesysForge.Infrastructure.Persistence.Configurations;

public sealed class CharacterSnapshotConfiguration : IEntityTypeConfiguration<CharacterSnapshot>
{
    public void Configure(EntityTypeBuilder<CharacterSnapshot> builder)
    {
        builder.ToTable("CharacterSnapshots");

        builder.HasKey(snapshot => snapshot.Id);

        builder.Property(snapshot => snapshot.ContentJson)
            .IsRequired();

        builder.Property(snapshot => snapshot.CreatedAt)
            .IsRequired();

        builder.HasOne<Character>()
            .WithMany(character => character.Snapshots)
            .HasForeignKey(snapshot => snapshot.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Ruleset>()
            .WithMany()
            .HasForeignKey(snapshot => snapshot.RulesetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(snapshot => snapshot.CharacterId);
    }
}
