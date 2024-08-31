using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class GlobalSnapshotReward : Aggregate<GlobalSnapshotRewardId>
{
    public HypergraphId HypergraphId { get; private set; } = default!;
    public DateOnly RewardDate { get; private set; }
    public WalletAddress WalletAddress { get; private set; } = default!;
    public long RewardAmount { get; private set; }
    public DateTime LastReceivedUtc { get; private set; }

    public static GlobalSnapshotReward Create(HypergraphId hypergraphId, DateOnly rewardDate, WalletAddress walletAddress, long rewardAmount,
        DateTime lastReceivedUtc)
    {
        return new GlobalSnapshotReward()
        {
            Id = new GlobalSnapshotRewardId(Guid.NewGuid()),
            HypergraphId = hypergraphId,
            RewardDate = rewardDate,
            WalletAddress = walletAddress,
            RewardAmount = rewardAmount,
            LastReceivedUtc = lastReceivedUtc
        };
    }

    public void AddReward(long rewardAmount, DateTime lastReceivedUtc)
    {
        RewardAmount += rewardAmount;
        LastReceivedUtc = lastReceivedUtc;
    }
}
