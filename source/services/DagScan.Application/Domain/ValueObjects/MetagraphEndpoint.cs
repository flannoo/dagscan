using DagScan.Core.DDD;

namespace DagScan.Application.Domain.ValueObjects;

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
