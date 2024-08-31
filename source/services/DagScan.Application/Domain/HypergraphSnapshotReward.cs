using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class HypergraphSnapshotReward : Entity<HypergraphSnapshotRewardId>
{
    public HypergraphId HypergraphId { get; private set; } = default!;
    public DateOnly RewardDate { get; private set; }
    public WalletAddress WalletAddress { get; private set; } = default!;
    public long RewardAmount { get; private set; }
    public DateTime LastReceivedUtc { get; private set; }

    public static HypergraphSnapshotReward Create(HypergraphId hypergraphId, DateOnly rewardDate, WalletAddress walletAddress, long rewardAmount,
        DateTime lastReceivedUtc)
    {
        return new HypergraphSnapshotReward()
        {
            Id = new HypergraphSnapshotRewardId(Guid.NewGuid()),
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
