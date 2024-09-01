using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class RewardTransaction : Entity<RewardTransactionId>
{
    public RewardTransactionConfigId RewardTransactionConfigId { get; private init; } = default!;
    public MetagraphId MetagraphId { get; init; } = default!;
    public MetagraphAddress? MetagraphAddress { get; init; }
    public WalletAddress WalletAddress { get; init; } = default!;
    public string RewardCategory { get; init; } = default!;
    public string TransactionHash { get; init; } = default!;
    public long Amount { get; init; }
    public DateTime TransactionDate { get; init; }

    public static RewardTransaction Create(RewardTransactionConfigId rewardTransactionConfigId, MetagraphId metagraphId,
        MetagraphAddress metagraphAddress, WalletAddress walletAddress, string rewardCategory, string transactionHash,
        long amount, DateTime transactionDate)
    {
        return new RewardTransaction()
        {
            Id = new RewardTransactionId(Guid.NewGuid()),
            RewardTransactionConfigId = rewardTransactionConfigId,
            MetagraphId = metagraphId,
            MetagraphAddress = metagraphAddress,
            WalletAddress = walletAddress,
            RewardCategory = rewardCategory,
            TransactionHash = transactionHash,
            Amount = amount,
            TransactionDate = transactionDate
        };
    }
}
