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

namespace DagScan.Application.Features.SyncHypergraphSnapshots;

public sealed class SyncHypergraphSnapshotsWorker(
    IServiceScopeFactory scopeFactory,
    IHttpClientFactory httpClientFactory,
    ILogger<SyncHypergraphSnapshotsWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                var dagContext = scope.ServiceProvider.GetRequiredService<DagContext>();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                var errorOccurred = false;
                var hypergraphs = await dagContext.Hypergraphs.ToListAsync(cancellationToken);

                foreach (var hypergraph in hypergraphs)
                {
                    if (hypergraph.BlockExplorerApiBaseAddress is null || !hypergraph.DataSyncEnabled)
                    {
                        logger.LogInformation(
                            "Skipping snapshot sync for {Hypergraph} because no blockExplorer API is configured or datasync is disabled.",
                            hypergraph.Name);
                        continue;
                    }

                    var lastSyncedSnapshot = hypergraph.LastSnapshotSynced;

                    using var httpClient = httpClientFactory.CreateClient();
                    httpClient.BaseAddress = new Uri(hypergraph.BlockExplorerApiBaseAddress);
                    var response = await httpClient.GetAsync(
                        $"global-snapshots?limit=300&search_after={lastSyncedSnapshot}",
                        cancellationToken);

                    var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

                    if (!response.IsSuccessStatusCode)
                    {
                        logger.LogError(
                            "Error occurred while retrieving snapshot: {ResponseBody} for lastSnapshot {lastSyncedSnapshot}",
                            responseBody, lastSyncedSnapshot);
                        errorOccurred = true;
                        continue;
                    }

                    var result = JsonSerializer.Deserialize<GlobalSnapshotDto>(responseBody, options);

                    if (result is null)
                    {
                        logger.LogError("No snapshots returned from API: {ResponseBody}", responseBody);
                        errorOccurred = true;
                        continue;
                    }

                    // The following ordinals are returned twice, remove the ones with the invalid hash
                    // 682076: 1bb224e59a8a606bb42ae136be5ffb85c367bc298242cbe10a79b30b932fbdab
                    // 682007: e37bf18e6a22085f1a1bf6587179b2d57798b0c1db7846db79dcea184e988736
                    var excludedHashes = new List<string>()
                    {
                        "1bb224e59a8a606bb42ae136be5ffb85c367bc298242cbe10a79b30b932fbdab",
                        "e37bf18e6a22085f1a1bf6587179b2d57798b0c1db7846db79dcea184e988736"
                    };

                    var globalSnapshots = result.GlobalSnapshotData
                        .Where(snapshot => !excludedHashes.Contains(snapshot.Hash))
                        .ToList();

                    var commandResponse =
                        await mediator.Send(
                            new InsertGlobalSnapshotsCommand()
                            {
                                HypergraphId = hypergraph.Id, GlobalSnapshots = globalSnapshots
                            }, cancellationToken);

                    if (!commandResponse)
                    {
                        logger.LogError(
                            "Something went wrong while syncing snapshots for hypergraph {HypergraphName}",
                            hypergraph.Name);
                    }

                    // Sleep the worker for a minute
                    if (result.GlobalSnapshotData.Count < 300)
                    {
                        await Task.Delay(60_000, cancellationToken);
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

public sealed record InsertGlobalSnapshotsCommand : ICommand<bool>
{
    public HypergraphId HypergraphId { get; init; } = default!;
    public List<GlobalSnapshotDtoData> GlobalSnapshots { get; init; } = [];
}

public sealed class InsertGlobalSnapshotsCommandHandler(
    DagContext dagContext,
    ILogger<InsertGlobalSnapshotsCommandHandler> logger)
    : IRequestHandler<InsertGlobalSnapshotsCommand, bool>
{
    public async Task<bool> Handle(InsertGlobalSnapshotsCommand request, CancellationToken cancellationToken)
    {
        var hypergraph = await dagContext.Hypergraphs.FirstOrDefaultAsync(h => h.Id == request.HypergraphId,
            cancellationToken: cancellationToken);

        if (hypergraph is null)
        {
            logger.LogError("Hypergraph {HypergraphId} not found", request.HypergraphId.Value);
            return false;
        }

        logger.LogInformation("Processing {SnapshotCount} global snapshots", request.GlobalSnapshots.Count);

        foreach (var globalSnapshot in request.GlobalSnapshots)
        {
            var snapshot = HypergraphSnapshot.Create(hypergraph.Id, globalSnapshot.Ordinal, globalSnapshot.Blocks.Count,
                globalSnapshot.Hash,
                globalSnapshot.Timestamp,
                globalSnapshot.GlobalSnapshotRewards.Count > 0,
                globalSnapshot.Ordinal < hypergraph.StartSnapshotMetadataOrdinal);

            await dagContext.HypergraphSnapshots.AddAsync(snapshot, cancellationToken);
        }

        // group snapshots by date for reward aggregation
        var snapshotDateGroup = request.GlobalSnapshots.GroupBy(x => x.Timestamp.Date);
        var lastSnapshotOrdinal = request.GlobalSnapshots.Max(x => x.Ordinal);
        foreach (var dateGroup in snapshotDateGroup)
        {
            var snapshots = dateGroup.ToList();

            var rewardsByAddress = new Dictionary<string, long>();
            var lastReceivedByAddress = new Dictionary<string, DateTime>();

            foreach (var snapshot in snapshots)
            {
                var groupedRewards = snapshot.GlobalSnapshotRewards.GroupBy(x => x.Destination).ToList();

                foreach (var rewardsGroup in groupedRewards)
                {
                    var address = rewardsGroup.Key;
                    var sumAmount = rewardsGroup.Sum(x => x.Amount);

                    if (!rewardsByAddress.TryAdd(address, sumAmount))
                    {
                        rewardsByAddress[address] += sumAmount;
                    }

                    if (!lastReceivedByAddress.TryAdd(address, snapshot.Timestamp))
                    {
                        lastReceivedByAddress[address] = snapshot.Timestamp;
                    }
                }
            }

            foreach (var rewardByAddress in rewardsByAddress)
            {
                var walletAddress = new WalletAddress(rewardByAddress.Key);
                var rewardAmount = rewardByAddress.Value;

                var lastReceivedRewardDate = lastReceivedByAddress[rewardByAddress.Key];

                var globalSnapshotReward = await dagContext.HypergraphSnapshotRewards
                    .FirstOrDefaultAsync(x =>
                            x.HypergraphId == hypergraph.Id &&
                            x.WalletAddress == walletAddress &&
                            x.RewardDate == DateOnly.FromDateTime(dateGroup.Key),
                        cancellationToken: cancellationToken);

                if (globalSnapshotReward is null)
                {
                    var newGlobalSnapshotReward = HypergraphSnapshotReward.Create(hypergraph.Id,
                        DateOnly.FromDateTime(dateGroup.Key), walletAddress, rewardAmount, lastReceivedRewardDate);

                    await dagContext.HypergraphSnapshotRewards.AddAsync(newGlobalSnapshotReward, cancellationToken);
                }
                else
                {
                    globalSnapshotReward.AddReward(rewardAmount, lastReceivedRewardDate);
                }
            }
        }

        hypergraph.UpdateLastSnapshotSynced(lastSnapshotOrdinal);

        return true;
    }
}
