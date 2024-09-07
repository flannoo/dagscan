namespace DagScan.Application.Features.GetHypergraphSnapshotMetrics;

public sealed record HypergraphSnapshotMetricsDto(
    DateOnly SnapshotDate,
    string? MetagraphAddress,
    bool IsTimeTriggered,
    long TotalSnapshotCount,
    long TotalSnapshotFeeAmount,
    long TotalTransactionCount,
    long TotalTransactionAmount,
    long TotalTransactionFeeAmount);
