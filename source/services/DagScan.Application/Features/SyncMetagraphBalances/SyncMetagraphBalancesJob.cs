using DagScan.Application.Data;
using DagScan.Core.Constants;
using DagScan.Core.Scheduling;
using Hangfire;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Features.SyncMetagraphBalances;

[AutomaticRetry(Attempts = 0)]
public sealed class SyncMetagraphBalancesJob(
    DagContext dagContext,
    IServiceScopeFactory scopeFactory,
    ILogger<SyncMetagraphBalancesJob> logger) : IJob
{
    public string Schedule => Constants.CronExpression.EveryFifteenMinutes;

    public async Task Execute()
    {
        logger.LogInformation("Start {JobName} Execution", nameof(SyncMetagraphBalancesJob));

        var metagraphs = dagContext.Metagraphs
            .ToList();

        var tasks = metagraphs
            .Where(metagraph => metagraph.DataSyncEnabled && metagraph.MetagraphAddress != null)
            .Select(async metagraph =>
            {
                if (!metagraph.DataSyncEnabled || metagraph.MetagraphAddress is null)
                {
                    return;
                }

                await using var scope = scopeFactory.CreateAsyncScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
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
            });

        await Task.WhenAll(tasks);

        logger.LogInformation("Completed {JobName} Execution", nameof(SyncMetagraphBalancesJob));
    }
}
