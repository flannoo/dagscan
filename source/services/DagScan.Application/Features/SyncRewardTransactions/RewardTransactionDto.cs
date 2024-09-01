using System.Text.Json.Serialization;

namespace DagScan.Application.Features.SyncRewardTransactions;

public sealed class RewardTransactionDto
{
    [JsonPropertyName("data")] public List<RewardTransactionDataDto> RewardTransactions { get; init; } = default!;
}

public sealed class RewardTransactionDataDto
{
    public string Hash { get; init; } = default!;
    public DateTime Timestamp { get; init; }
    public long Amount { get; init; }
    public string Source { get; init; } = default!;
    public string Destination { get; init; } = default!;
}
