using GenesysForge.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenesysForge.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Email)
            .HasMaxLength(User.MaxEmailLength)
            .IsRequired();

        builder.Property(user => user.NormalizedEmail)
            .HasMaxLength(User.MaxNormalizedEmailLength)
            .IsRequired();

        builder.HasIndex(user => user.NormalizedEmail)
            .IsUnique();

        builder.Property(user => user.DisplayName)
            .HasMaxLength(User.MaxDisplayNameLength)
            .IsRequired();

        builder.Property(user => user.PasswordHash)
            .HasMaxLength(User.MaxPasswordHashLength)
            .IsRequired();

        builder.Property(user => user.CreatedAt)
            .IsRequired();
    }
}
