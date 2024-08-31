using System.Net.Http.Json;
using System.Text.Json;
using DagScan.Application.Data;
using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.CQRS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Features.SyncGlobalSnapshots;

public sealed class SyncGlobalSnapshotsWorker(
    DagContext dagContext,
    IHttpClientFactory httpClientFactory,
    IMediator mediator,
    ILogger<SyncGlobalSnapshotsWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var errorOccurred = false;
                var hypergraphs = await dagContext.Hypergraphs.ToListAsync(cancellationToken);

                foreach (var hypergraph in hypergraphs)
                {
                    if (hypergraph.BlockExplorerApiBaseAddress is null)
                    {
                        logger.LogInformation(
                            "Skipping snapshot sync for {Hypergraph} because no blockExplorer API is configured.",
                            hypergraph.Name);
                        continue;
                    }

                    var lastSyncedSnapshot = hypergraph.LastSnapshotSynced;

                    using var httpClient = httpClientFactory.CreateClient();
                    httpClient.BaseAddress = new Uri(hypergraph.BlockExplorerApiBaseAddress);
                    var response = await httpClient.GetAsync(
                        $"global-snapshots?limit=1000&search_after={lastSyncedSnapshot}",
                        cancellationToken);

                    var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
                    var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                    var result = new GlobalSnapshotResponse();

                    try
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            result.Snapshots = JsonSerializer.Deserialize<GlobalSnapshotDto>(responseBody, options);
                        }
                        else
                        {
                            result.Error = JsonSerializer.Deserialize<ErrorResponse>(responseBody, options);
                            errorOccurred = true;
                        }
                    }
                    catch (JsonException)
                    {
                        logger.LogError("Unable to parse response body: {ResponseBody}", responseBody);
                        errorOccurred = true;
                        continue;
                    }

                    if (result.Snapshots is null)
                    {
                        logger.LogError("No snapshots returned from API: {ResponseBody}", responseBody);
                        errorOccurred = true;
                        continue;
                    }

                    var commandResponse =
                        await mediator.Send(
                            new InsertGlobalSnapshotsCommand()
                            {
                                HypergraphId = hypergraph.Id, GlobalSnapshots = result.Snapshots.GlobalSnapshotData
                            }, cancellationToken);

                    if (!commandResponse)
                    {
                        logger.LogError(
                            "Something went wrong while syncing snapshots for hypergraph {HypergraphName}",
                            hypergraph.Name);
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

        foreach (var globalSnapshot in request.GlobalSnapshots)
        {
            var snapshot = GlobalSnapshot.Create(hypergraph.Id, globalSnapshot.Ordinal, globalSnapshot.Hash,
                globalSnapshot.Timestamp,
                globalSnapshot.GlobalSnapshotRewards.Count > 0);

            await dagContext.GlobalSnapshots.AddAsync(snapshot, cancellationToken);
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

                var globalSnapshotReward = await dagContext.GlobalSnapshotRewards
                    .FirstOrDefaultAsync(x =>
                            x.HypergraphId == hypergraph.Id &&
                            x.WalletAddress == walletAddress &&
                            x.RewardDate == DateOnly.FromDateTime(dateGroup.Key),
                        cancellationToken: cancellationToken);

                if (globalSnapshotReward is null)
                {
                    var newGlobalSnapshotReward = GlobalSnapshotReward.Create(hypergraph.Id,
                        DateOnly.FromDateTime(dateGroup.Key), walletAddress, rewardAmount, lastReceivedRewardDate);

                    await dagContext.GlobalSnapshotRewards.AddAsync(newGlobalSnapshotReward, cancellationToken);
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
