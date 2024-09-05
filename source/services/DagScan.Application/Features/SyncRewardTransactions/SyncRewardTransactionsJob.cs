using System.Text.Json;
using DagScan.Application.Data;
using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.CQRS;
using DagScan.Core.Scheduling;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Features.SyncRewardTransactions;

[AutomaticRetry(Attempts = 0)]
public sealed class SyncRewardTransactionsJob(
    IHttpClientFactory httpClientFactory,
    IServiceScopeFactory scopeFactory,
    ILogger<SyncRewardTransactionsJob> logger) : IJob
{
    public string Schedule => Cron.Hourly();

    public async Task Execute()
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var dagContext = scope.ServiceProvider.GetRequiredService<DagContext>();
        var rewardTransactionConfigs = await dagContext.RewardTransactionConfigs.ToListAsync();

        var tasks = rewardTransactionConfigs
            .Where(x => x.IsEnabled)
            .Select(async rewardTransactionConfig =>
            {
                if (rewardTransactionConfig.IsProcessing)
                {
                    logger.LogInformation("RewardTransaction {rewardTransactionConfigId} still in process",
                        rewardTransactionConfig.MetagraphId);
                    return;
                }

                await ProcessRewardTransactions(rewardTransactionConfig);
            });

        await Task.WhenAll(tasks);
    }

    private async Task ProcessRewardTransactions(RewardTransactionConfig rewardTransactionConfig)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var dagContext = scope.ServiceProvider.GetRequiredService<DagContext>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        rewardTransactionConfig =
            await dagContext.RewardTransactionConfigs.FirstAsync(x => x.Id == rewardTransactionConfig.Id);

        try
        {
            var metagraph = await dagContext.Metagraphs
                .FirstOrDefaultAsync(m => m.Id == rewardTransactionConfig.MetagraphId);

            if (metagraph == null)
            {
                logger.LogInformation("Could not find metagraph with id {MetagraphId}",
                    rewardTransactionConfig.MetagraphId);
                return;
            }

            var hypergraph = await dagContext.Hypergraphs
                .FirstOrDefaultAsync(m => m.Id == metagraph.HypergraphId);

            if (hypergraph?.BlockExplorerApiBaseAddress is null)
            {
                logger.LogInformation(
                    "Could not find hypergraph with id {HypergraphId} or block explorer is empty",
                    metagraph.HypergraphId);
                return;
            }

            var lastTransaction =
                await GetLastTransaction(hypergraph.BlockExplorerApiBaseAddress, rewardTransactionConfig);

            if (lastTransaction is null || lastTransaction.Hash == rewardTransactionConfig.LastProcessedHash)
            {
                logger.LogInformation("No new transactions found for config id {ConfigId}",
                    rewardTransactionConfig.Id.Value);
                return;
            }

            rewardTransactionConfig.SetProcessing(true);
            await dagContext.SaveChangesAsync();

            var rewardTransactions = new List<RewardTransactionDataDto>();

            if (rewardTransactionConfig.ToWalletAddress is null ||
                rewardTransactionConfig.ToWalletAddress.Value == lastTransaction.Destination)
            {
                rewardTransactions.Add(lastTransaction);
            }

            rewardTransactions = await GetTransactions(hypergraph.BlockExplorerApiBaseAddress, rewardTransactionConfig,
                lastTransaction.Hash, rewardTransactions);

            if (rewardTransactions is null)
            {
                logger.LogInformation(
                    "Something went wrong while retrieving reward transactions for config id {ConfigId}",
                    rewardTransactionConfig.Id.Value);

                rewardTransactionConfig.SetProcessing(false);
                await dagContext.SaveChangesAsync();

                return;
            }

            var commandResponse = await mediator.Send(
                new InsertRewardTransactionsCommand()
                {
                    MetagraphId = metagraph.Id,
                    MetagraphAddress = metagraph.MetagraphAddress,
                    RewardTransactionConfigId = rewardTransactionConfig.Id,
                    LastProcessedHash = lastTransaction.Hash,
                    LastProcessedDateTime = lastTransaction.Timestamp,
                    RewardTransactions = rewardTransactions
                });

            if (!commandResponse)
            {
                logger.LogError(
                    "Something went wrong while syncing transaction rewards for config {RewardTransactionConfigId}",
                    rewardTransactionConfig.Id.Value);

                rewardTransactionConfig.SetProcessing(false);
                await dagContext.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            logger.LogError(e,
                "Something went wrong while syncing transaction rewards for config {RewardTransactionConfigId}",
                rewardTransactionConfig.Id.Value);

            rewardTransactionConfig.SetProcessing(false);
            await dagContext.SaveChangesAsync();
        }
    }

    private async Task<RewardTransactionDataDto?> GetLastTransaction(string blockExplorerApiBaseAddress,
        RewardTransactionConfig config)
    {
        using var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(blockExplorerApiBaseAddress);

        var urlLastTransaction = $"/addresses/{config.FromWalletAddress.Value}/transactions/sent?limit=1";
        if (config.MetagraphAddress is not null)
        {
            urlLastTransaction =
                $"/currency/{config.MetagraphAddress.Value}/addresses/{config.FromWalletAddress.Value}/transactions/sent?limit=1";
        }

        var response = await httpClient.GetAsync(urlLastTransaction);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Error occurred while retrieving transaction for url {Url}: {ResponseBody}",
                urlLastTransaction, responseBody);
            return null;
        }

        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var result = JsonSerializer.Deserialize<RewardTransactionDto>(responseBody, options);

        return result?.RewardTransactions.FirstOrDefault();
    }

    private async Task<List<RewardTransactionDataDto>?> GetTransactions(string blockExplorerApiBaseAddress,
        RewardTransactionConfig config, string lastProcessedHash, List<RewardTransactionDataDto> rewardTransactions,
        string? next = null)
    {
        logger.LogInformation("Retrieving reward transactions for config id {ConfigId} with next parameter '{Next}'",
            config.Id.Value, next);

        using var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(blockExplorerApiBaseAddress);

        var urlTransactions =
            $"/addresses/{config.FromWalletAddress.Value}/transactions/sent?limit=500&search_before={lastProcessedHash}";
        if (config.MetagraphAddress is not null)
        {
            urlTransactions =
                $"/currency/{config.MetagraphAddress.Value}/addresses/{config.FromWalletAddress.Value}/transactions/sent?limit=500&search_before={lastProcessedHash}";
        }

        if (!string.IsNullOrEmpty(next))
        {
            urlTransactions += $"&next={next}";
        }

        var response = await httpClient.GetAsync(urlTransactions);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Error occurred while retrieving transactions for url {Url}: {ResponseBody}",
                urlTransactions, responseBody);
            return null;
        }

        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var result = JsonSerializer.Deserialize<RewardTransactionDto>(responseBody, options);

        if (result is null)
        {
            logger.LogError("Error occurred while retrieving transactions for url {Url}: {ResponseBody}",
                urlTransactions, responseBody);
            return null;
        }

        var isCompleted = false;

        foreach (var reward in result.RewardTransactions)
        {
            if (reward.Hash == lastProcessedHash)
            {
                isCompleted = true;
                break;
            }

            if (config.ToWalletAddress is not null)
            {
                if (reward.Destination == config.ToWalletAddress.Value)
                {
                    rewardTransactions.Add(reward);
                }
            }
            else
            {
                rewardTransactions.Add(reward);
            }
        }

        next = result.Meta?.Next;

        if (isCompleted || string.IsNullOrEmpty(next))
        {
            return rewardTransactions;
        }

        return await GetTransactions(blockExplorerApiBaseAddress, config, lastProcessedHash, rewardTransactions, next);
    }
}

public sealed record InsertRewardTransactionsCommand : ICommand<bool>
{
    public RewardTransactionConfigId RewardTransactionConfigId { get; init; } = default!;
    public MetagraphId MetagraphId { get; init; } = default!;
    public MetagraphAddress? MetagraphAddress { get; init; }
    public List<RewardTransactionDataDto> RewardTransactions { get; init; } = [];
    public string LastProcessedHash { get; init; } = default!;
    public DateTime LastProcessedDateTime { get; init; }
}

public sealed class InsertRewardTransactionsCommandHandler(
    DagContext dagContext,
    ILogger<InsertRewardTransactionsCommandHandler> logger)
    : IRequestHandler<InsertRewardTransactionsCommand, bool>
{
    public async Task<bool> Handle(InsertRewardTransactionsCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing reward transactions for config {RewardConfigId}",
            request.RewardTransactionConfigId.Value);

        var rewardTransactionConfig = await dagContext.RewardTransactionConfigs
            .FirstOrDefaultAsync(x => x.Id == request.RewardTransactionConfigId, cancellationToken);

        if (rewardTransactionConfig is null)
        {
            logger.LogError("RewardConfig {RewardConfigId} not found.", request.RewardTransactionConfigId.Value);
            return false;
        }

        foreach (var transaction in request.RewardTransactions)
        {
            var rewardTransaction = RewardTransaction.Create(rewardTransactionConfig.Id, request.MetagraphId,
                request.MetagraphAddress, new WalletAddress(transaction.Destination),
                rewardTransactionConfig.RewardCategory, transaction.Hash, transaction.Amount, transaction.Timestamp);

            await dagContext.RewardTransactions.AddAsync(rewardTransaction, cancellationToken);
        }

        rewardTransactionConfig.UpdateLastProcessedHash(request.LastProcessedHash, request.LastProcessedDateTime);
        rewardTransactionConfig.SetProcessing(false);

        return true;
    }
}
