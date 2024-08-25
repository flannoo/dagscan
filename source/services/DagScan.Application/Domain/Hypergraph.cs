using Ardalis.GuardClauses;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class Hypergraph : Aggregate<HypergraphId>
{
    public string Name { get; private init; } = default!;
    public string ApiBaseAddress { get; private init; } = default!;
    public string? BlockExplorerApiBaseAddress { get; private init; }
    public bool DataSyncEnabled { get; private init; }

    public static Hypergraph Create(string name, string apiBaseAddress, string? blockExplorerApiAddress, bool dataSyncEnabled)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Guard.Against.NullOrWhiteSpace(apiBaseAddress, nameof(apiBaseAddress));

        return new Hypergraph()
        {
            Id = new HypergraphId(Guid.NewGuid()),
            Name = name,
            ApiBaseAddress = apiBaseAddress,
            BlockExplorerApiBaseAddress = blockExplorerApiAddress,
            DataSyncEnabled = dataSyncEnabled
        };
    }
}
