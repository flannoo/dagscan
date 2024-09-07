using DagScan.Application.Data;
using DagScan.Core.Constants;
using DagScan.Core.Scheduling;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Features.SyncHypergraphValidatorNodes;

[AutomaticRetry(Attempts = 0)]
public sealed class SyncHypergraphValidatorNodesJob(
    DagContext dagContext,
    IMediator mediator,
    ILogger<SyncHypergraphValidatorNodesJob> logger) : IJob
{
    public string Schedule => Constants.CronExpression.EveryFiveMinutes;

    public async Task Execute()
    {
        logger.LogInformation("Start {JobName} Execution", nameof(SyncHypergraphValidatorNodesJob));

        var hypergraphs = dagContext.Hypergraphs
            .ToList();

        foreach (var hypergraph in hypergraphs)
        {
            if (!hypergraph.DataSyncEnabled)
            {
                continue;
            }

            var response = await mediator.Send(new UpsertHypergraphValidatorNodesCommand()
            {
                HypergraphId = hypergraph.Id.Value
            });

            if (!response)
            {
                logger.LogError("Something went wrong while syncing Hypergraph Validator Nodes for hypergraph {HypergraphName}", hypergraph.Name);
            }
        }

        logger.LogInformation("Completed {JobName} Execution", nameof(SyncHypergraphValidatorNodesJob));
    }
}
