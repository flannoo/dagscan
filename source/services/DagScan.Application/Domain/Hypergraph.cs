using Ardalis.GuardClauses;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class HypergraphId : ValueObject
{
    public Guid Value { get; }

    public HypergraphId(Guid value)
    {
        Guard.Against.Default(value, nameof(value));

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public sealed class Hypergraph : Aggregate<HypergraphId>
{
    public string Name { get; private init; } = default!;
    public string ApiBaseAddress { get; private init; } = default!;
    public bool DataSyncEnabled { get; private init; }
    public List<HypergraphValidatorNode> HypergraphValidatorNodes { get; private set; } = [];

    public static Hypergraph Create(string name, string apiBaseAddress, bool dataSyncEnabled)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Guard.Against.NullOrWhiteSpace(apiBaseAddress, nameof(apiBaseAddress));

        return new Hypergraph()
        {
            Id = new HypergraphId(Guid.NewGuid()),
            Name = name,
            ApiBaseAddress = apiBaseAddress,
            DataSyncEnabled = dataSyncEnabled
        };
    }

    public void UpdateHypergraphValidatorNodes(List<HypergraphValidatorNode> hypergraphValidatorNodes)
    {
        Guard.Against.Null(hypergraphValidatorNodes, nameof(hypergraphValidatorNodes));

        HypergraphValidatorNodes = hypergraphValidatorNodes;
    }
}
