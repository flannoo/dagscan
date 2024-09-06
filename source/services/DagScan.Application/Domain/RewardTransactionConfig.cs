using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class RewardTransactionConfig : Entity<RewardTransactionConfigId>
{
    public string RewardCategory { get; private init; } = default!;
    public MetagraphId MetagraphId { get; private init; } = default!;
    public MetagraphAddress? MetagraphAddress { get; private init; }
    public WalletAddress FromWalletAddress { get; private init; } = default!;
    public WalletAddress? ToWalletAddress { get; private init; }
    public string? LastProcessedHash { get; private set; }
    public bool IsEnabled { get; private init; }
    public DateTime LastProcessedDate { get; private set; }
    public bool IsProcessing { get; private set; }

    public static RewardTransactionConfig Create(string rewardCategory, MetagraphId metagraphId,
        MetagraphAddress? metagraphAddress,
        WalletAddress fromWalletAddress, WalletAddress? toWalletAddress,
        string? lastProcessedHash, bool isEnabled)
    {
        return new RewardTransactionConfig()
        {
            Id = new RewardTransactionConfigId(Guid.NewGuid()),
            RewardCategory = rewardCategory,
            MetagraphId = metagraphId,
            MetagraphAddress = metagraphAddress,
            FromWalletAddress = fromWalletAddress,
            ToWalletAddress = toWalletAddress,
            LastProcessedHash = lastProcessedHash,
            IsEnabled = isEnabled
        };
    }

    public void UpdateLastProcessedHash(string hash, DateTime lastProcessedDate)
    {
        LastProcessedHash = hash;
        LastProcessedDate = lastProcessedDate;
    }
}
