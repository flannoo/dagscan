using System.Reflection;
using DagScan.Application;
using DagScan.Application.Data;
using DagScan.Application.Data.Seeders;
using DagScan.Application.Extensions;
using DagScan.Core.CQRS;
using DagScan.Core.Persistence;
using DagScan.Worker.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork<DagContext>>();
builder.Services.AddScoped<IEfUnitOfWork, EfUnitOfWork<DagContext>>();

var databaseConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                               throw new ArgumentException("DB_CONNECTION_STRING environment variable not found.");

builder.Services.AddDbContext<DagContext>(options => { options.UseSqlServer(databaseConnectionString); });
builder.Services.AddDbContext<ReadOnlyDagContext>(options => { options.UseSqlServer(databaseConnectionString); });

builder.Services.AddScoped<IRequiredDataSeeder, HypergraphDataSeeder>();

builder.Services
    .AddCqrs(
        new[] { Assembly.GetExecutingAssembly(), typeof(IApplicationMarker).Assembly },
        pipelines: new[] { typeof(UnitOfWorkBehavior<,>), }
    );

builder.AddHangfire(databaseConnectionString);

builder.Services.AddHttpClient();

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

host.Services.InitRecurringJobs();

host.Run();
