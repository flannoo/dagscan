using Ardalis.GuardClauses;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class HypergraphValidatorNodeId : ValueObject
{
    public Guid Value { get; }

    public HypergraphValidatorNodeId(Guid value)
    {
        Guard.Against.Default(value, nameof(value));

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public sealed class HypergraphValidatorNode : Entity<HypergraphValidatorNodeId>
{
    public string WalletHash { get; private init; } = default!;
    public string WalletAddress { get; private init; } = default!;
    public string Version { get; private init; } = default!;
    public string State { get; private init; } = default!;
    public string? Provider { get; private init; }
    public string? Country { get; private init; }
    public string? City { get; private init; }
    public double? Latitude { get; private init; }
    public double? Longitude { get; private init; }
    public NodeOperator? NodeOperator { get; private init; }
}
