using Ardalis.GuardClauses;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public enum MetagraphTypes
{
    DagL1,
    MetagraphL0,
    MetagraphCurrencyL1,
    MetagraphDataL1
}

public sealed class MetagraphId : ValueObject
{
    public Guid Value { get; }

    public MetagraphId(Guid value)
    {
        Guard.Against.Default(value, nameof(value));

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public sealed class MetagraphEndpoint : ValueObject
{
    public string ApiBaseAddress { get; private init; } = default!;
    public MetagraphTypes MetagraphType { get; private init; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ApiBaseAddress;
        yield return MetagraphType;
    }
}

public sealed class Metagraph : Aggregate<MetagraphId>
{
    public string Address { get; private init; } = default!;
    public string Name { get; private init; } = default!;
    public string Symbol { get; private init; } = default!;
    public MetagraphEndpoint[] MetagraphEndpoint { get; private init; } = [];
    public string? CompanyName { get; private init; }
    public string? Website { get; private init; }
    public string? Description { get; private init; }
    public bool DataSyncEnabled { get; private init; }

    public List<MetagraphValidatorNode> MetagraphValidatorNodes { get; private init; } = [];
}
