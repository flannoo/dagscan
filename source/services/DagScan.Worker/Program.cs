using System.Reflection;
using DagScan.Application.Data;
using DagScan.Application.Extensions;
using DagScan.Core.CQRS;
using DagScan.Core.Persistence;
using DagScan.Worker;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork<DagContext>>();

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
builder.Services.AddDbContext<DagContext>(options => { options.UseSqlServer(connectionString); });
builder.Services.AddDbContext<ReadOnlyDagContext>(options => { options.UseSqlServer(connectionString); });

builder.Services
    .AddCqrs(
        new[] { Assembly.GetExecutingAssembly() },
        pipelines: new[] { typeof(UnitOfWorkBehavior<,>), }
    );

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

if (bool.TryParse(Environment.GetEnvironmentVariable("ENABLE_DB_MIGRATION") ?? "false", out var migrationEnabled) &&
    migrationEnabled)
{
    await host.Services.ApplyDatabaseMigrations();
}

await host.Services.ApplyRequiredSeedData();

if (bool.TryParse(Environment.GetEnvironmentVariable("ENABLE_DB_SEEDER") ?? "false", out var seedDataEnabled) &&
    seedDataEnabled)
{
    await host.Services.ApplySeedDatabase();
}

host.Run();
