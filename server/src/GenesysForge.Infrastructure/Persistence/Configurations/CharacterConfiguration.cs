using GenesysForge.Domain.Characters;
using GenesysForge.Domain.Rules;
using GenesysForge.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenesysForge.Infrastructure.Persistence.Configurations;

public sealed class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        builder.ToTable("Characters");

        builder.HasKey(character => character.Id);

        builder.Property(character => character.Name)
            .HasMaxLength(Character.MaxNameLength)
            .IsRequired();

        builder.Property(character => character.Status)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(character => character.CreatedAt)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(character => character.OwnerUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Ruleset>()
            .WithMany()
            .HasForeignKey(character => character.RulesetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(character => character.OwnerUserId);
        builder.HasIndex(character => character.RulesetId);

        builder.Navigation(character => character.Snapshots)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(character => character.XpLedgerEntries)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(character => character.Skills)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(character => character.Talents)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
