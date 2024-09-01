using System.Net.Http.Headers;
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

namespace DagScan.Application.Features.SyncHypergraphSnapshotsMetadata;

public sealed class SyncHypergraphSnapshotsMetadataWorker(
    IServiceScopeFactory scopeFactory,
    IHttpClientFactory httpClientFactory,
    IMediator mediator,
    ILogger<SyncHypergraphSnapshotsMetadataWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                var dagContext = scope.ServiceProvider.GetRequiredService<DagContext>();

                var errorOccurred = false;
                var hypergraphs = await dagContext.Hypergraphs.ToListAsync(cancellationToken);
                var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);

                foreach (var hypergraph in hypergraphs)
                {
                    if (!hypergraph.DataSyncEnabled)
                    {
                        continue;
                    }

                    var snapshotsToSync = await dagContext.HypergraphSnapshots
                        .Where(x => !x.IsMetadataSynced && x.HypergraphId == hypergraph.Id)
                        .OrderBy(x => x.Ordinal)
                        .Take(50)
                        .ToListAsync(cancellationToken);

                    foreach (var snapshotToSync in snapshotsToSync)
                    {
                        if (snapshotsToSync == null)
                        {
                            continue;
                        }

                        using var httpClient = httpClientFactory.CreateClient();
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.BaseAddress = new Uri(hypergraph.ApiBaseAddress);

                        var response = await httpClient.GetAsync($"global-snapshots/{snapshotToSync.Ordinal}",
                            cancellationToken);
                        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

                        if (!response.IsSuccessStatusCode)
                        {
                            logger.LogError("Error occurred while retrieving snapshot info: {ResponseBody}",
                                responseBody);
                            errorOccurred = true;
                            continue;
                        }

                        var result = JsonSerializer.Deserialize<NodeSnapshotDto>(responseBody, options);

                        if (result is null)
                        {
                            logger.LogError("No snapshots returned from API: {ResponseBody}", responseBody);
                            errorOccurred = true;
                            continue;
                        }

                        var commandResponse =
                            await mediator.Send(
                                new InsertGlobalSnapshotMetadataCommand()
                                {
                                    HypergraphSnapshotId = snapshotToSync.Id, NodeSnapshot = result
                                }, cancellationToken);

                        if (!commandResponse)
                        {
                            logger.LogError(
                                "Something went wrong while syncing snapshot metadata for hypergraph {HypergraphName}, snapshot ordinal {SnapshotId}",
                                hypergraph.Name, snapshotToSync.Ordinal);
                        }
                    }
                }

                if (errorOccurred)
                {
                    await Task.Delay(30_000, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while syncing snapshot metadata");
                await Task.Delay(30_000, cancellationToken);
            }
        }
    }
}

public sealed record InsertGlobalSnapshotMetadataCommand : ICommand<bool>
{
    public HypergraphSnapshotId HypergraphSnapshotId { get; init; } = default!;
    public NodeSnapshotDto NodeSnapshot { get; init; } = default!;
}

public sealed class InsertGlobalSnapshotMetadataCommandHandler(
    DagContext dagContext,
    ILogger<InsertGlobalSnapshotMetadataCommandHandler> logger)
    : IRequestHandler<InsertGlobalSnapshotMetadataCommand, bool>
{
    public async Task<bool> Handle(InsertGlobalSnapshotMetadataCommand request, CancellationToken cancellationToken)
    {
        var snapshot = await dagContext.HypergraphSnapshots
            .FirstOrDefaultAsync(x => x.Id == request.HypergraphSnapshotId, cancellationToken);

        if (snapshot is null)
        {
            logger.LogError("HypergraphSnapshot {HypergraphSnapshotId} not found", request.HypergraphSnapshotId.Value);
            return false;
        }

        await InsertSnapshotParticipationRecords(snapshot.HypergraphId, DateOnly.FromDateTime(snapshot.Timestamp),
            request.NodeSnapshot.Proofs, cancellationToken);

        snapshot.SetFee(SumStateChannelSnapshotFees(request.NodeSnapshot));

        if (!snapshot.IsTimeTriggeredSnapshot)
        {
            var stateChannelSnapshot = request.NodeSnapshot.Value.StateChannelSnapshots?.FirstOrDefault();
            if (stateChannelSnapshot is not null)
            {
                var metagraphAddress = stateChannelSnapshot.Value.Key;
                if (!string.IsNullOrWhiteSpace(metagraphAddress))
                {
                    snapshot.SetMetagraphAddress(new WalletAddress(metagraphAddress));
                }
            }
        }

        if (request.NodeSnapshot.Value.Blocks is not null && request.NodeSnapshot.Value.Blocks.Count != 0)
        {
            var transactions = request.NodeSnapshot.Value.Blocks.Where(x => x.Block?.Value?.Transactions != null)
                .SelectMany(x => x.Block!.Value!.Transactions!).ToList();

            var transactionCount = transactions.Count;
            var transactionAmount = transactions.Sum(x => x.Value!.Amount ?? 0);
            var transactionFeeAmount = transactions.Sum(x => x.Value!.Fee ?? 0);

            snapshot.SetTransactionInfo(transactionCount, transactionAmount, transactionFeeAmount);
        }
        else
        {
            snapshot.SetTransactionInfo(0, 0, 0);
        }

        snapshot.MarkMetadataAsSynced();

        return true;
    }

    private async Task InsertSnapshotParticipationRecords(HypergraphId hypergraphId, DateOnly snapshotDate,
        List<Proof> proofs, CancellationToken cancellationToken)
    {
        var walletIds = proofs.Select(x => new WalletId(x.Id)).ToList();
        var existingParticipationRecords =
            await dagContext.HypergraphValidatorNodesParticipants.Where(x =>
                walletIds.Contains(x.WalletId) && x.SnapshotDate >= snapshotDate).ToListAsync(cancellationToken);

        foreach (var existingParticipationRecord in existingParticipationRecords)
        {
            existingParticipationRecord.IncrementSnapshotCount();
        }

        var existingWalletIds = existingParticipationRecords.Select(x => x.WalletId).ToHashSet();
        var newWalletIds = walletIds.Except(existingWalletIds).ToList();
        var newParticipationRecords = newWalletIds
            .Select(walletId => HypergraphValidatorNodeParticipant.Create(hypergraphId,
                WalletAddress.CreateFromWalletId(walletId.Value), walletId, snapshotDate));

        await dagContext.HypergraphValidatorNodesParticipants.AddRangeAsync(newParticipationRecords, cancellationToken);
    }

    private static long SumStateChannelSnapshotFees(NodeSnapshotDto nodeSnapshotDto)
    {
        // Check if the StateChannelSnapshots dictionary is null or empty
        if (nodeSnapshotDto.Value.StateChannelSnapshots == null)
            return 0;

        long totalFee = 0;

        // Iterate through each key-value pair in the dictionary
        foreach (var entry in nodeSnapshotDto.Value.StateChannelSnapshots)
        {
            foreach (var stateChannelSnapshot in entry.Value)
            {
                if (stateChannelSnapshot.Value != null)
                {
                    totalFee += stateChannelSnapshot.Value.Fee;
                }
            }
        }

        return totalFee;
    }
}
