using GenesysForge.Domain.Users;
using GenesysForge.Domain.Rules;
using Microsoft.EntityFrameworkCore;

namespace GenesysForge.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<RuleDefinition> RuleDefinitions => Set<RuleDefinition>();

    public DbSet<RuleEntity> RuleEntities => Set<RuleEntity>();

    public DbSet<Ruleset> Rulesets => Set<Ruleset>();

    public DbSet<RuleSourceVersion> RuleSourceVersions => Set<RuleSourceVersion>();

    public DbSet<SourceBook> SourceBooks => Set<SourceBook>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
