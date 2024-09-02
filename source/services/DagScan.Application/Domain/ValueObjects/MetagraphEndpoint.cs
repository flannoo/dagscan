using Ardalis.GuardClauses;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain.ValueObjects;

public sealed class MetagraphEndpoint : ValueObject
{
    public string ApiBaseAddress { get; private init; }
    public MetagraphTypes MetagraphType { get; private init; }

    public MetagraphEndpoint(string apiBaseAddress, MetagraphTypes metagraphType)
    {
        Guard.Against.NullOrWhiteSpace(apiBaseAddress, nameof(apiBaseAddress));
        Guard.Against.Null(metagraphType, nameof(metagraphType));

        ApiBaseAddress = apiBaseAddress;
        MetagraphType = metagraphType;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ApiBaseAddress;
        yield return MetagraphType;
    }
}
