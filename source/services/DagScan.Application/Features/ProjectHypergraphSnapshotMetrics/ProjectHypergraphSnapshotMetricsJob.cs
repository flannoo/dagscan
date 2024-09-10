using DagScan.Application.Data;
using DagScan.Application.Domain;
using DagScan.Core.Scheduling;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Features.ProjectHypergraphSnapshotMetrics;

[AutomaticRetry(Attempts = 0)]
public sealed class ProjectHypergraphSnapshotMetricsJob(
    IServiceScopeFactory scopeFactory,
    ILogger<ProjectHypergraphSnapshotMetricsJob> logger) : IJob
{
    public string Schedule => Cron.Hourly();

    public async Task Execute()
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var dagContext = scope.ServiceProvider.GetRequiredService<DagContext>();

        var startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-3));
        var endDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

        if (!dagContext.HypergraphSnapshotMetrics.Any())
        {
            startDate = DateOnly.MinValue;
        }

        var hypergraph = await dagContext.Hypergraphs.FirstOrDefaultAsync(x => x.Name == "mainnet");

        if (hypergraph == null)
        {
            return;
        }

        await using var transaction = await dagContext.Database.BeginTransactionAsync();

        try
        {
            var snapshots = await dagContext.HypergraphSnapshots.Where(x => x.HypergraphId == hypergraph.Id)
                .Where(snapshot => snapshot.Timestamp >= startDate.ToDateTime(TimeOnly.MinValue) &&
                                   snapshot.Timestamp <= endDate.ToDateTime(TimeOnly.MaxValue))
                .GroupBy(snapshot =>
                    new { snapshot.Timestamp.Date, snapshot.IsTimeTriggeredSnapshot, snapshot.MetagraphAddress })
                .Select(snapshot => new HypergraphSnapshotMetric()
                {
                    SnapshotDate = DateOnly.FromDateTime(snapshot.Key.Date),
                    MetagraphAddress = snapshot.Key.MetagraphAddress,
                    IsTimeTriggered = snapshot.Key.IsTimeTriggeredSnapshot,
                    TotalSnapshotCount = snapshot.Count(),
                    TotalSnapshotFeeAmount = snapshot.Sum(x => x.FeeAmount ?? 0),
                    TotalTransactionCount = snapshot.Sum(x => x.TransactionCount ?? 0),
                    TotalTransactionAmount = snapshot.Sum(x => x.TransactionAmount ?? 0),
                    TotalTransactionFeeAmount = snapshot.Sum(x => x.TransactionFeeAmount ?? 0)
                })
                .ToListAsync();

            // Clear the table
            dagContext.HypergraphSnapshotMetrics.RemoveRange(
                dagContext.HypergraphSnapshotMetrics.IgnoreQueryFilters()
                    .Where(x => x.SnapshotDate >= startDate && x.SnapshotDate <= endDate)
            );

            await dagContext.SaveChangesAsync();

            // Add new records
            await dagContext.AddRangeAsync(snapshots);
            await dagContext.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
