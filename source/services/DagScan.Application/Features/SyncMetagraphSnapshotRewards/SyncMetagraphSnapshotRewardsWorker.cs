using System.Text.Json;
using DagScan.Application.Data;
using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.CQRS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Features.SyncMetagraphSnapshotRewards;

public sealed class SyncMetagraphSnapshotRewardsWorker(
    IServiceScopeFactory scopeFactory,
    IHttpClientFactory httpClientFactory,
    IMediator mediator,
    ILogger<SyncMetagraphSnapshotRewardsWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                var dagContext = scope.ServiceProvider.GetRequiredService<DagContext>();

                var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                var errorOccurred = false;
                var metagraphs = dagContext.Metagraphs.ToList();

                foreach (var metagraph in metagraphs)
                {
                    if (!metagraph.DataSyncEnabled || metagraph.MetagraphAddress is null)
                    {
                        logger.LogInformation(
                            "Skipping snapshot reward sync for {Metagraph} because datasync is disabled or metagraph address is empty.",
                            metagraph.Name);
                        continue;
                    }

                    var lastSyncedSnapshot = metagraph.LastSnapshotSynced;

                    var hypergraph =
                        await dagContext.Hypergraphs.FirstOrDefaultAsync(x => x.Id == metagraph.HypergraphId,
                            cancellationToken);

                    if (hypergraph == null || hypergraph.BlockExplorerApiBaseAddress == null)
                    {
                        logger.LogInformation(
                            "Skipping snapshot reward sync for {Metagraph} because the hypergraph with Id {HypergraphId} was not found or the blockexplorer API is not configured.",
                            metagraph.Name, metagraph.HypergraphId);
                        continue;
                    }

                    using var httpClient = httpClientFactory.CreateClient();
                    httpClient.BaseAddress = new Uri(hypergraph.BlockExplorerApiBaseAddress);
                    var response = await httpClient.GetAsync(
                        $"currency/{metagraph.MetagraphAddress.Value}/snapshots?limit=1000&search_after={lastSyncedSnapshot}",
                        cancellationToken);

                    var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

                    if (!response.IsSuccessStatusCode)
                    {
                        logger.LogError("Error occurred while retrieving snapshots: {ResponseBody}", responseBody);
                        errorOccurred = true;
                        continue;
                    }

                    var result = JsonSerializer.Deserialize<MetagraphSnapshotDto>(responseBody, options);

                    if (result is null)
                    {
                        logger.LogError("No snapshots returned from API: {ResponseBody}", responseBody);
                        errorOccurred = true;
                        continue;
                    }

                    var commandResponse =
                        await mediator.Send(
                            new InsertMetagraphSnapshotsCommand()
                            {
                                MetagraphId = metagraph.Id,
                                MetagraphAddress = metagraph.MetagraphAddress,
                                MetagraphSnapshots = result.MetagraphSnapshotData
                            }, cancellationToken);

                    if (!commandResponse)
                    {
                        logger.LogError(
                            "Something went wrong while syncing snapshot rewards for metagraph {MetagraphName}",
                            metagraph.Name);
                    }
                }

                if (errorOccurred)
                {
                    await Task.Delay(30_000, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while syncing snapshots");
                await Task.Delay(30_000, cancellationToken);
            }
        }
    }
}

public sealed record InsertMetagraphSnapshotsCommand : ICommand<bool>
{
    public MetagraphId MetagraphId { get; init; } = default!;
    public MetagraphAddress MetagraphAddress { get; init; } = default!;
    public List<MetagraphSnapshotDtoData> MetagraphSnapshots { get; init; } = [];
}

public sealed class InsertMetagraphSnapshotsCommandHandler(
    DagContext dagContext,
    ILogger<InsertMetagraphSnapshotsCommandHandler> logger)
    : IRequestHandler<InsertMetagraphSnapshotsCommand, bool>
{
    public async Task<bool> Handle(InsertMetagraphSnapshotsCommand request, CancellationToken cancellationToken)
    {
        var metagraph =
            await dagContext.Metagraphs.FirstOrDefaultAsync(x => x.Id == request.MetagraphId, cancellationToken);

        if (metagraph == null)
        {
            return false;
        }

        var lastSnapshotOrdinal = request.MetagraphSnapshots.Max(x => x.Ordinal);

        foreach (var snapshot in request.MetagraphSnapshots)
        {
            if (snapshot.MetagraphSnapshotRewards.Count == 0)
            {
                continue;
            }

            foreach (var reward in snapshot.MetagraphSnapshotRewards)
            {
                var metagraphSnapshotReward = MetagraphSnapshotReward.Create(request.MetagraphId,
                    request.MetagraphAddress, snapshot.Timestamp, snapshot.Ordinal,
                    new WalletAddress(reward.Destination), reward.Amount);

                await dagContext.MetagraphSnapshotRewards.AddAsync(metagraphSnapshotReward, cancellationToken);
            }
        }

        metagraph.UpdateLastSnapshotSynced(lastSnapshotOrdinal);

        return true;
    }
}
