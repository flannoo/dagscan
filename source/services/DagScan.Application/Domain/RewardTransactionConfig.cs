using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class RewardTransactionConfig : Entity<RewardTransactionConfigId>
{
    public string RewardCategory { get; init; } = default!;
    public MetagraphId MetagraphId { get; init; } = default!;
    public MetagraphAddress? MetagraphAddress { get; init; }
    public WalletAddress FromWalletAddress { get; init; } = default!;
    public WalletAddress? ToWalletAddress { get; init; }
    public string? LastProcessedHash { get; set; }
    public bool IsEnabled { get; init; }
    public DateTime LastProcessedDate { get; set; }

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
