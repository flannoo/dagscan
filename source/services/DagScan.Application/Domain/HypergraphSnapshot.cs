using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class HypergraphSnapshot : Entity<HypergraphSnapshotId>
{
    public HypergraphId HypergraphId { get; private init; } = default!;
    public long Ordinal { get; private init; }
    public int Blocks { get; private init; }
    public string Hash { get; private init; } = default!;
    public DateTime Timestamp { get; private init; }
    public bool IsTimeTriggeredSnapshot { get; private init; }
    public long FeeAmount { get; private set; }
    public long TransactionCount { get; private set; }
    public long TransactionAmount { get; private set; }
    public long TransactionFeeAmount { get; private set; }
    public WalletAddress? MetagraphAddress { get; private set; }
    public bool IsMetadataSynced { get; private set; }

    public static HypergraphSnapshot Create(HypergraphId hypergraphId, long ordinal, int blocks, string hash, DateTime timestamp, bool isTimeTriggeredSnapshot, bool isMetadataSynced)
    {
        return new HypergraphSnapshot()
        {
            Id = new HypergraphSnapshotId(Guid.NewGuid()),
            HypergraphId = hypergraphId,
            Ordinal = ordinal,
            Blocks = blocks,
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

    public void SetTransactionInfo(long transactionCount, long transactionAmount, long transactionFeeAmount)
    {
        TransactionCount = transactionCount;
        TransactionAmount = transactionAmount;
        TransactionFeeAmount = transactionFeeAmount;
    }

    public void MarkMetadataAsSynced()
    {
        IsMetadataSynced = true;
    }
}
