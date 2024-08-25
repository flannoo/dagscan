using Ardalis.GuardClauses;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class MetagraphValidatorNode : Aggregate<MetagraphValidatorNodeId>
{
    public MetagraphId MetagraphId { get; private init; } = default!;
    public MetagraphTypes MetagraphType { get; private init; }
    public WalletAddress WalletAddress { get; private init; } = default!;
    public string WalletId { get; private init; } = default!;
    public string IpAddress { get; private set; } = default!;
    public string NodeStatus { get; private set; } = default!;
    public string? Version { get; private set; }
    public string? ServiceProvider { get; private set; }
    public string? Country { get; private set; }
    public string? City { get; private set; }
    public Coordinate? Coordinates { get; private set; }
    public NodeOperator? NodeOperator { get; private set; }
}
