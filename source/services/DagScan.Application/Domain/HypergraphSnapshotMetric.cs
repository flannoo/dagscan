using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class HypergraphSnapshotMetric : Entity<int>
{
    public DateOnly SnapshotDate { get; init; }
    public WalletAddress? MetagraphAddress { get; init; }
    public bool IsTimeTriggered { get; init; }
    public long TotalSnapshotCount { get; init; }
    public long TotalSnapshotFeeAmount { get; init; }
    public long TotalTransactionCount { get; init; }
    public long TotalTransactionAmount { get; init; }
    public long TotalTransactionFeeAmount { get; init; }
}
