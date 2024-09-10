namespace DagScan.Application.Features.GetMetagraphSnapshotOnChainData;

public sealed class MetagraphSnapshotDto
{
    public Value Value { get; set; } = default!;
}

public sealed class Value
{
    public DataApplication? DataApplication { get; set; }
}

public sealed class DataApplication
{
    public int[]? OnChainState { get; set; }
}
