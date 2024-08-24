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
    public string ApiBaseAddress { get; private init; } = default!;
    public HypergraphValidatorNode[] HypergraphValidatorNodes { get; private init; } = [];
}
