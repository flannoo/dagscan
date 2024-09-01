using Ardalis.GuardClauses;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain.ValueObjects;

public sealed class RewardTransactionConfigId : ValueObject
{
    public Guid Value { get; }

    public RewardTransactionConfigId(Guid value)
    {
        Guard.Against.Default(value, nameof(value));

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
