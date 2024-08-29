using DagScan.Application.Data;
using DagScan.Core.Scheduling;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Features.SyncMetagraphBalances;

public sealed class SyncMetagraphBalancesJob(
    DagContext dagContext,
    IMediator mediator,
    ILogger<SyncMetagraphBalancesJob> logger) : IJob
{
    public string Schedule => Cron.Hourly();

    public async Task Execute()
    {
        logger.LogInformation("Start {JobName} Execution", nameof(SyncMetagraphBalancesJob));

        var metagraphs = dagContext.Metagraphs
            .ToList();

        foreach (var metagraph in metagraphs)
        {
            if (!metagraph.DataSyncEnabled || metagraph.MetagraphAddress is null)
            {
                continue;
            }

            var response = await mediator.Send(new RefreshMetagraphBalancesCommand()
            {
                MetagraphId = metagraph.Id.Value
            });

            if (!response)
            {
                logger.LogError(
                    "Something went wrong while syncing Metagraph Balances for metagraph {MetagraphName}",
                    metagraph.Name);
            }
        }

        logger.LogInformation("Completed {JobName} Execution", nameof(SyncMetagraphBalancesJob));
    }
}
