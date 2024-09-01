using DagScan.Application.Data;
using DagScan.Core.Constants;
using DagScan.Core.Scheduling;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Features.SyncHypergraphBalances;

public sealed class SyncHypergraphBalancesJob(
    DagContext dagContext,
    IMediator mediator,
    ILogger<SyncHypergraphBalancesJob> logger) : IJob
{
    public string Schedule => Constants.CronExpression.EveryFifteenMinutes;

    public async Task Execute()
    {
        logger.LogInformation("Start {JobName} Execution", nameof(SyncHypergraphBalancesJob));

        var hypergraphs = dagContext.Hypergraphs
            .ToList();

        foreach (var hypergraph in hypergraphs)
        {
            if (!hypergraph.DataSyncEnabled)
            {
                continue;
            }

            var response = await mediator.Send(new RefreshHypergraphBalancesCommand()
            {
                HypergraphId = hypergraph.Id.Value
            });

            if (!response)
            {
                logger.LogError(
                    "Something went wrong while syncing Hypergraph Balances for hypergraph {HypergraphName}",
                    hypergraph.Name);
            }
        }

        logger.LogInformation("Completed {JobName} Execution", nameof(SyncHypergraphBalancesJob));
    }
}
