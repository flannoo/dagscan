using DagScan.Application.Data;
using DagScan.Core.Constants;
using DagScan.Core.Scheduling;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Features.SyncMetagraphValidatorNodes;

public sealed class SyncMetagraphValidatorNodesJob(
    DagContext dagContext,
    IServiceScopeFactory scopeFactory,
    ILogger<SyncMetagraphValidatorNodesJob> logger) : IJob
{
    public string Schedule => Constants.CronExpression.EveryFifteenMinutes;

    public async Task Execute()
    {
        logger.LogInformation("Start {JobName} Execution", nameof(SyncMetagraphValidatorNodesJob));

        var metagraphs = dagContext.Metagraphs
            .ToList();

        var tasks = metagraphs
            .Where(metagraph => metagraph.DataSyncEnabled)
            .Select(async metagraph =>
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var response = await mediator.Send(new UpsertMetagraphValidatorNodesCommand()
                {
                    MetagraphId = metagraph.Id.Value
                });

                if (!response)
                {
                    logger.LogError(
                        "Something went wrong while syncing Metagraph Validator Nodes for metagraph {MetagraphName}",
                        metagraph.Name);
                }
            });

        await Task.WhenAll(tasks);

        logger.LogInformation("Completed {JobName} Execution", nameof(SyncMetagraphValidatorNodesJob));
    }
}
