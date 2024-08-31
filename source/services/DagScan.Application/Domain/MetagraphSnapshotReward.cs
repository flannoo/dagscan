using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class MetagraphSnapshotReward : Entity<MetagraphSnapshotRewardId>
{
    public MetagraphId MetagraphId { get; private set; } = default!;
    public MetagraphAddress MetagraphAddress { get; private set; } = default!;
    public DateTime RewardDate { get; private set; }
    public long SnapshotOrdinal { get; private set; }
    public WalletAddress WalletAddress { get; private set; } = default!;
    public long RewardAmount { get; private set; }

    public static MetagraphSnapshotReward Create(MetagraphId metagraphId, MetagraphAddress metagraphAddress,
        DateTime rewardDate, long snapshotOrdinal,
        WalletAddress walletAddress, long rewardAmount)
    {
        return new MetagraphSnapshotReward()
        {
            Id = new MetagraphSnapshotRewardId(Guid.NewGuid()),
            MetagraphId = metagraphId,
            MetagraphAddress = metagraphAddress,
            SnapshotOrdinal = snapshotOrdinal,
            RewardDate = rewardDate,
            WalletAddress = walletAddress,
            RewardAmount = rewardAmount
        };
    }
}
