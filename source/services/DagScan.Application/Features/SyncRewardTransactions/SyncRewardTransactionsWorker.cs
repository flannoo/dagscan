using System.Text.Json;
using DagScan.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Features.SyncRewardTransactions;

public sealed class SyncRewardTransactionsWorker(
    IServiceScopeFactory scopeFactory,
    IHttpClientFactory httpClientFactory,
    IMediator mediator,
    ILogger<SyncRewardTransactionsWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(30_000, cancellationToken);
            /*
            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                var dagContext = scope.ServiceProvider.GetRequiredService<DagContext>();

                var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                var errorOccurred = false;

                var rewardTransactionConfigs = await dagContext.RewardTransactionConfigs.ToListAsync(cancellationToken);
                var metagraphs = await dagContext.Metagraphs.ToListAsync(cancellationToken);
                foreach (var rewardTransactionConfig in rewardTransactionConfigs)
                {
                    var metagraph =
                        await dagContext.Metagraphs.FirstOrDefaultAsync(
                            m => m.Id == rewardTransactionConfig.MetagraphId, cancellationToken);

                    if (metagraph == null)
                    {
                        logger.LogInformation("Could not find metagraph with id {MetagraphId}",
                            rewardTransactionConfig.MetagraphId);
                        continue;
                    }

                    var hypergraph =
                        await dagContext.Hypergraphs.FirstOrDefaultAsync(m => m.Id == metagraph.HypergraphId,
                            cancellationToken);

                    if (hypergraph is null || hypergraph.BlockExplorerApiBaseAddress is null)
                    {
                        logger.LogInformation(
                            "Could not find hypergraph with id {HypergraphId} or block explorer is empty",
                            metagraph.HypergraphId);
                        continue;
                    }

                    using var httpClient = httpClientFactory.CreateClient();
                    httpClient.BaseAddress = new Uri(hypergraph.BlockExplorerApiBaseAddress);

                    var url =
                        $"/addresses/{rewardTransactionConfig.FromWalletAddress.Value}/transactions/sent?limit=1000";
                    if (rewardTransactionConfig.MetagraphAddress?.Value is not null)
                    {
                        url =
                            $"/currency/{rewardTransactionConfig.MetagraphAddress.Value}/addresses/{rewardTransactionConfig.FromWalletAddress.Value}/transactions/sent?limit=1000";
                    }

                    var response = await httpClient.GetAsync(
                        $"global-snapshots?limit=1000&search_after={lastSyncedSnapshot}",
                        cancellationToken);
                }

                if (errorOccurred)
                {
                    await Task.Delay(30_000, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while syncing reward transactions");
                await Task.Delay(30_000, cancellationToken);
            }*/
        }
    }
}
