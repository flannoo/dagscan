using DagScan.Application.Data;
using DagScan.Core.Scheduling;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Features.SyncHypergraphValidatorNodes;

[DisableConcurrentExecution(timeoutInSeconds: 3600)]
public sealed class SyncHypergraphValidatorNodesJob(
    DagContext dagContext,
    IMediator mediator,
    ILogger<SyncHypergraphValidatorNodesJob> logger) : IJob
{
    public string Schedule => Cron.Minutely();

    public async Task Execute()
    {
        logger.LogInformation("Start {JobName} Execution", nameof(SyncHypergraphValidatorNodesJob));

        var hypergraphs = dagContext.Hypergraphs
            .Include(x => x.HypergraphValidatorNodes)
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
                logger.LogError("Something went wrong while syncing Hypergraph Validator Nodes");
            }
        }

        await dagContext.SaveChangesAsync();

        logger.LogInformation("Completed {JobName} Execution", nameof(SyncHypergraphValidatorNodesJob));
    }
}
