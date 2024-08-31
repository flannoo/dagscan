using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class HypergraphSnapshot : Entity<HypergraphSnapshotId>
{
    public HypergraphId HypergraphId { get; private init; } = default!;
    public long Ordinal { get; private init; }
    public string Hash { get; private init; } = default!;
    public DateTime Timestamp { get; private init; }
    public bool IsTimeTriggeredSnapshot { get; private init; }
    public long? FeeAmount { get; private set; }
    public WalletAddress? MetagraphAddress { get; private set; }
    public bool IsMetadataSynced { get; private set; }

    public static HypergraphSnapshot Create(HypergraphId hypergraphId, long ordinal, string hash, DateTime timestamp, bool isTimeTriggeredSnapshot, bool isMetadataSynced)
    {
        return new HypergraphSnapshot()
        {
            Id = new HypergraphSnapshotId(Guid.NewGuid()),
            HypergraphId = hypergraphId,
            Ordinal = ordinal,
            Hash = hash,
            IsTimeTriggeredSnapshot = isTimeTriggeredSnapshot,
            Timestamp = timestamp,
            IsMetadataSynced = isMetadataSynced
        };
    }

    public void SetFee(long amount)
    {
        FeeAmount = amount;
    }

    public void SetMetagraphAddress(WalletAddress walletAddress)
    {
        MetagraphAddress = walletAddress;
    }

    public void MarkMetadataAsSynced()
    {
        IsMetadataSynced = true;
    }
}
