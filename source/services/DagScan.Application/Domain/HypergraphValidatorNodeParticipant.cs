using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class HypergraphValidatorNodeParticipant : Entity<HypergraphValidatorNodeParticipantId>
{
    public HypergraphId HypergraphId { get; private init; } = default!;
    public WalletAddress WalletAddress { get; private init; } = default!;
    public WalletId WalletId { get; private init; } = default!;
    public DateOnly SnapshotDate { get; private init; }
    public long SnapshotCount { get; private set; }

    public static HypergraphValidatorNodeParticipant Create(HypergraphId hypergraphId, WalletAddress walletAddress,
        WalletId walletId, DateOnly snapshotDate)
    {
        return new HypergraphValidatorNodeParticipant()
        {
            Id = new HypergraphValidatorNodeParticipantId(Guid.NewGuid()),
            HypergraphId = hypergraphId,
            WalletAddress = walletAddress,
            WalletId = walletId,
            SnapshotDate = snapshotDate,
            SnapshotCount = 1
        };
    }

    public void IncrementSnapshotCount()
    {
        SnapshotCount++;
    }
}
