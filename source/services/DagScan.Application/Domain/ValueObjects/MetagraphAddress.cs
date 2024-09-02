using Ardalis.GuardClauses;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain.ValueObjects;

public sealed class MetagraphAddress : ValueObject
{
    public string Value { get; }

    public MetagraphAddress(string value)
    {
        Guard.Against.NullOrWhiteSpace(value, nameof(value));

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
