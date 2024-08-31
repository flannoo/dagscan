using Ardalis.GuardClauses;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain.ValueObjects;

public sealed class HypergraphValidatorNodeParticipantId : ValueObject
{
    public Guid Value { get; }

    public HypergraphValidatorNodeParticipantId(Guid value)
    {
        Guard.Against.Default(value, nameof(value));

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
