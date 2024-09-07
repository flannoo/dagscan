namespace DagScan.Application.Features.GetHypergraphValidatorNodesUptime;

public sealed record HypergraphValidatorNodesUptimeDto(
    DateOnly SnapshotDate,
    long SnapshotCountParticipated,
    long SnapshotCountTotal,
    int UptimePercentage);
