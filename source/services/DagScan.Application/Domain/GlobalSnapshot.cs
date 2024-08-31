using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class GlobalSnapshot : Aggregate<GlobalSnapshotId>
{
    public HypergraphId HypergraphId { get; private init; } = default!;
    public long Ordinal { get; private init; }
    public string Hash { get; private init; } = default!;
    public DateTime Timestamp { get; private init; }
    public bool IsTimeTriggeredSnapshot { get; private init; }
    public long? FeeAmount { get; private set; }
    public WalletAddress? MetagraphAddress { get; private set; }
    public bool IsSynced { get; private set; }

    public static GlobalSnapshot Create(HypergraphId hypergraphId, long ordinal, string hash, DateTime timestamp, bool isTimeTriggeredSnapshot)
    {
        return new GlobalSnapshot()
        {
            Id = new GlobalSnapshotId(Guid.NewGuid()),
            HypergraphId = hypergraphId,
            Ordinal = ordinal,
            Hash = hash,
            IsTimeTriggeredSnapshot = isTimeTriggeredSnapshot,
            Timestamp = timestamp,
            IsSynced = false
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

    public void MarkAsSynced()
    {
        IsSynced = true;
    }
}
