using Ardalis.GuardClauses;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class MetagraphValidatorNodeId : ValueObject
{
    public Guid Value { get; }

    public MetagraphValidatorNodeId(Guid value)
    {
        Guard.Against.Default(value, nameof(value));

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public sealed class MetagraphValidatorNode : Entity<MetagraphValidatorNodeId>
{
    public MetagraphTypes MetagraphType { get; private init; }
    public string WalletHash { get; private init; } = default!;
    public string WalletAddress { get; private init; } = default!;
    public string IpAddress { get; private set; } = default!;
    public string State { get; private set; } = default!;
    public string? Version { get; private set; }
    public string? Provider { get; private set; }
    public string? Country { get; private set; }
    public string? City { get; private set; }
    public double? Latitude { get; private set; }
    public double? Longitude { get; private set; }
    public NodeOperator? NodeOperator { get; private set; }
}
