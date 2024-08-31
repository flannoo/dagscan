using System.Text.Json.Serialization;

namespace DagScan.Application.Features.SyncHypergraphSnapshots;

public sealed class GlobalSnapshotDto
{
    [JsonPropertyName("data")] public List<GlobalSnapshotDtoData> GlobalSnapshotData { get; init; } = default!;
}

public sealed class GlobalSnapshotDtoData
{
    public string Hash { get; init; } = default!;
    public long Ordinal { get; init; }
    public List<string> Blocks { get; init; } = [];
    [JsonPropertyName("rewards")] public List<GlobalSnapshotDtoReward> GlobalSnapshotRewards { get; init; } = [];
    public DateTime Timestamp { get; init; }
}

public sealed class GlobalSnapshotDtoReward
{
    public string Destination { get; init; } = default!;
    public long Amount { get; init; }
}
