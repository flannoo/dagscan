using System.Reflection;
using DagScan.Application;
using DagScan.Application.Data;
using DagScan.Application.Data.Seeders;
using DagScan.Application.Extensions;
using DagScan.Application.Features.SyncHypergraphSnapshots;
using DagScan.Application.Features.SyncHypergraphSnapshotsMetadata;
using DagScan.Application.Features.SyncMetagraphSnapshotRewards;
using DagScan.Core.CQRS;
using DagScan.Core.Messaging;
using DagScan.Core.Persistence;
using DagScan.Worker.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork<DagContext>>();
builder.Services.AddScoped<IEfUnitOfWork, EfUnitOfWork<DagContext>>();

var databaseConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                               throw new ArgumentException("DB_CONNECTION_STRING environment variable not found.");

builder.Services.AddDbContext<DagContext>(options =>
{
    options.UseSqlServer(databaseConnectionString);
    options.AddInterceptors(new ConcurrencyInterceptor());
});
builder.Services.AddDbContext<ReadOnlyDagContext>(options => { options.UseSqlServer(databaseConnectionString); });

builder.Services.AddScoped<IDataSeeder, HypergraphDataSeeder>();
builder.Services.AddScoped<IDataSeeder, MetagraphDataSeeder>();
builder.Services.AddScoped<IDataSeeder, RewardTransactionConfigDataSeeder>();

builder.Services
    .AddCqrs(
        new[] { Assembly.GetExecutingAssembly(), typeof(IApplicationMarker).Assembly },
        pipelines: new[] { typeof(UnitOfWorkBehavior<,>), typeof(PublishDomainEventBehavior<,>) }
    );

builder.AddHangfire(databaseConnectionString);

builder.Services.AddHttpClient();

//builder.Services.AddHostedService<SyncHypergraphSnapshotsWorker>();
//builder.Services.AddHostedService<SyncHypergraphSnapshotsMetadataWorker>();
//builder.Services.AddHostedService<SyncMetagraphSnapshotRewardsWorker>();

var host = builder.Build();

if (bool.TryParse(Environment.GetEnvironmentVariable("ENABLE_DB_MIGRATION") ?? "false", out var migrationEnabled) &&
    migrationEnabled)
{
    await host.Services.ApplyDatabaseMigrations();
}

if (bool.TryParse(Environment.GetEnvironmentVariable("ENABLE_DB_SEEDER") ?? "false", out var seedDataEnabled) &&
    seedDataEnabled)
{
    await host.Services.ApplySeedDatabase();
}

host.Services.InitRecurringJobs();

host.Run();
