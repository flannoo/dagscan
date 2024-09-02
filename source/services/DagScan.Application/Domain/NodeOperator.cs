using Ardalis.GuardClauses;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class NodeOperatorId : ValueObject
{
    public Guid Value { get; }

    public NodeOperatorId(Guid value)
    {
        Guard.Against.Default(value, nameof(value));

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public sealed class NodeOperator : Entity<NodeOperatorId>
{
    public string Name { get; private init; } = default!;
    public string? Description { get; private init; }
    public string? Website { get; private init; }
}
