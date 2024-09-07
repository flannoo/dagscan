using Ardalis.GuardClauses;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public enum MetagraphTypes
{
    DagL1,
    MetagraphL0,
    MetagraphCurrencyL1,
    MetagraphDataL1
}

public sealed class Metagraph : Aggregate<MetagraphId>
{
    public HypergraphId HypergraphId { get; private set; } = default!;
    public MetagraphAddress? MetagraphAddress { get; private set; }
    public string Name { get; private init; } = default!;
    public string Symbol { get; private init; } = default!;
    public WalletAddress? FeeAddress { get; private init; }
    public List<MetagraphEndpoint> MetagraphEndpoints { get; private init; } = [];
    public string? CompanyName { get; private init; }
    public string? Website { get; private init; }
    public string? Description { get; private init; }
    public bool DataSyncEnabled { get; private init; }
    public long LastSnapshotSynced { get; private set; }

    public static Metagraph Create(HypergraphId hypergraphId, MetagraphAddress? metagraphAddress, string name,
        string symbol, WalletAddress? feeAddress, bool dataSyncEnabled, long lastSnapshotSynced)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Guard.Against.NullOrWhiteSpace(symbol, nameof(symbol));

        return new Metagraph()
        {
            Id = new MetagraphId(Guid.NewGuid()),
            HypergraphId = hypergraphId,
            MetagraphAddress = metagraphAddress,
            Name = name,
            Symbol = symbol,
            FeeAddress = feeAddress,
            DataSyncEnabled = dataSyncEnabled,
            LastSnapshotSynced = lastSnapshotSynced
        };
    }

    public void AddMetagraphEndpoint(MetagraphEndpoint metagraphEndpoint)
    {
        Guard.Against.Null(metagraphEndpoint, nameof(metagraphEndpoint));
        MetagraphEndpoints.Add(metagraphEndpoint);
    }

    public void UpdateLastSnapshotSynced(long ordinal)
    {
        LastSnapshotSynced = ordinal;
    }
}
