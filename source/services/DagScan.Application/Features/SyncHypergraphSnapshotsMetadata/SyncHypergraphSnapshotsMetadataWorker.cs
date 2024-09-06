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
    ILogger<SyncHypergraphSnapshotsMetadataWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            const int parallelProcessingCount = 30;
            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                var dagContext = scope.ServiceProvider.GetRequiredService<DagContext>();

                var errorOccurred = false;
                var hypergraphs = await dagContext.Hypergraphs.ToListAsync(cancellationToken);
                var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);

                var tasks = hypergraphs.Where(x => x.DataSyncEnabled)
                    .Select(async hypergraph =>
                    {
                        var completedSuccessfully = await ProcessSnapshotsMetadata(cancellationToken, dagContext,
                            hypergraph,
                            parallelProcessingCount, options);

                        if (!errorOccurred && !completedSuccessfully)
                        {
                            errorOccurred = true;
                        }
                    });

                await Task.WhenAll(tasks);

                if (errorOccurred)
                {
                    logger.LogError("Some error while syncing snapshot metadata, delaying next execution");
                    await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while syncing snapshot metadata");
                await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
            }
        }
    }

    private async Task<bool> ProcessSnapshotsMetadata(CancellationToken cancellationToken, DagContext dagContext,
        Hypergraph hypergraph, int parallelProcessingCount, JsonSerializerOptions options)
    {
        try
        {
            var completedSuccessfully = true;

            var snapshotsToSync = await dagContext.HypergraphSnapshots
                .Where(x => !x.IsMetadataSynced && x.HypergraphId == hypergraph.Id)
                .OrderBy(x => x.Ordinal)
                .Take(parallelProcessingCount)
                .ToListAsync(cancellationToken);

            logger.LogInformation("Processing {RecordCount} metadata records for hypergraph snapshots",
                snapshotsToSync.Count);

            var commandTasks = snapshotsToSync.Select(async snapshotToSync =>
                await ProcessSnapshotAsync(snapshotToSync, hypergraph, options, cancellationToken));

            var mediatorCommands = await Task.WhenAll(commandTasks);

            foreach (var command in mediatorCommands)
            {
                if (command is null)
                {
                    completedSuccessfully = false;
                    continue;
                }

                await using var scope = scopeFactory.CreateAsyncScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var commandResponse = await mediator.Send(command, cancellationToken);

                if (!commandResponse)
                {
                    completedSuccessfully = false;
                    logger.LogError(
                        "Something went wrong while syncing snapshot metadata for hypergraph {HypergraphName}, snapshot ordinal {SnapshotId}",
                        hypergraph.Name, command.HypergraphSnapshotId);
                }
            }

            if (snapshotsToSync.Count < parallelProcessingCount)
            {
                await Task.Delay(TimeSpan.FromSeconds(20), cancellationToken);
            }

            return completedSuccessfully;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Something went wrong while syncing snapshot metadata for hypergraph {HypergraphName}",
                hypergraph.Name);
            return false;
        }
    }

    private async Task<InsertGlobalSnapshotMetadataCommand?> ProcessSnapshotAsync(HypergraphSnapshot snapshotToSync,
        Hypergraph hypergraph, JsonSerializerOptions options, CancellationToken cancellationToken)
    {
        try
        {
            using var httpClient = httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(hypergraph.ApiBaseAddress);

            var response = await httpClient.GetAsync($"global-snapshots/{snapshotToSync.Ordinal}", cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Error occurred while retrieving snapshot info: {ResponseBody}", responseBody);
                return null;
            }

            var result = JsonSerializer.Deserialize<NodeSnapshotDto>(responseBody, options);
            if (result is null)
            {
                logger.LogError("No snapshots returned from API: {ResponseBody}", responseBody);
                return null;
            }

            return new InsertGlobalSnapshotMetadataCommand
            {
                HypergraphSnapshotId = snapshotToSync.Id, NodeSnapshot = result
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error processing snapshot for hypergraph {HypergraphName}, snapshot ordinal {SnapshotId}",
                hypergraph.Name, snapshotToSync.Ordinal);
            return null;
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
