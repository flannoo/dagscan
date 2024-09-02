using System.Text.Json.Serialization;

namespace DagScan.Application.Features.SyncMetagraphSnapshotRewards;

public sealed class MetagraphSnapshotDto
{
    [JsonPropertyName("data")] public List<MetagraphSnapshotDtoData> MetagraphSnapshotData { get; init; } = default!;
}

public sealed class MetagraphSnapshotDtoData
{
    public string Hash { get; init; } = default!;
    public long Ordinal { get; init; }
    public List<string> Blocks { get; init; } = [];
    [JsonPropertyName("rewards")] public List<Reward> MetagraphSnapshotRewards { get; init; } = [];
    public DateTime Timestamp { get; init; }
}

public sealed class Reward
{
    public string Destination { get; init; } = default!;
    public long Amount { get; init; }
}
