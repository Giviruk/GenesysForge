using GenesysForge.Application.Auth;
using GenesysForge.Application.Characters;
using GenesysForge.Application.Rules;
using GenesysForge.Infrastructure.Auth;
using GenesysForge.Infrastructure.Characters;
using GenesysForge.Infrastructure.Persistence;
using GenesysForge.Infrastructure.Rules;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GenesysForge.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        }

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(
                connectionString,
                npgsqlOptions => npgsqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
        });
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddScoped<ICharactersRepository, CharactersRepository>();
        services.AddScoped<IRulesRepository, RulesRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<IPasswordHashService, PasswordHashService>();
        services.AddSingleton<IAccessTokenService, JwtAccessTokenService>();

        return services;
    }
}
