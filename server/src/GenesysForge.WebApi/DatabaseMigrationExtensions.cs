using GenesysForge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GenesysForge.WebApi;

internal static class DatabaseMigrationExtensions
{
    public static WebApplication ApplyDatabaseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();

        return app;
    }
}
