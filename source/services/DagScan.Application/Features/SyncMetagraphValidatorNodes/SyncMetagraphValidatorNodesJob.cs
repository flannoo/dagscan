using DagScan.Application.Data;
using DagScan.Core.Scheduling;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Features.SyncMetagraphValidatorNodes;

[DisableConcurrentExecution(timeoutInSeconds: 3600)]
public sealed class SyncMetagraphValidatorNodesJob(
    DagContext dagContext,
    IMediator mediator,
    ILogger<SyncMetagraphValidatorNodesJob> logger) : IJob
{
    public string Schedule => Cron.Hourly();

    public async Task Execute()
    {
        logger.LogInformation("Start {JobName} Execution", nameof(SyncMetagraphValidatorNodesJob));

        var metagraphs = dagContext.Metagraphs
            .ToList();

        foreach (var metagraph in metagraphs)
        {
            if (!metagraph.DataSyncEnabled)
            {
                continue;
            }

            var response = await mediator.Send(new UpsertMetagraphValidatorNodesCommand()
            {
                MetagraphId = metagraph.Id.Value
            });

            if (!response)
            {
                logger.LogError("Something went wrong while syncing Metagraph Validator Nodes for metagraph {MetagraphName}", metagraph.Name);
            }
        }

        logger.LogInformation("Completed {JobName} Execution", nameof(SyncMetagraphValidatorNodesJob));
    }
}
