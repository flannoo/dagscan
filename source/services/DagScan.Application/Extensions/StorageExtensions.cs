using DagScan.Application.Data;
using DagScan.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Extensions;

public static class StorageExtensions
{
    public static async Task ApplyDatabaseMigrations(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<DagContext>();
        var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<DagContext>>();

        logger.LogInformation("Updating database...");

        await dbContext.Database.MigrateAsync();

        logger.LogInformation("Updated database");
    }

    public static async Task ApplySeedDatabase(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var seeders = serviceScope.ServiceProvider.GetServices<IDataSeeder>();
        var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<DagContext>>();

        foreach (var seeder in seeders.OrderBy(x => x.Order))
        {
            logger.LogInformation("Seeding '{Seed}' started...", seeder.GetType().Name);

            await seeder.SeedAsync();

            logger.LogInformation("Seeding '{Seed}' ended...", seeder.GetType().Name);
        }
    }

    public static async Task ApplyRequiredSeedData(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var seeders = serviceScope.ServiceProvider.GetServices<IRequiredDataSeeder>();
        var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<DagContext>>();

        foreach (var seeder in seeders.OrderBy(x => x.Order))
        {
            logger.LogInformation("Seeding '{Seed}' started...", seeder.GetType().Name);

            await seeder.SeedAsync();

            logger.LogInformation("Seeding '{Seed}' ended...", seeder.GetType().Name);
        }
    }
}
