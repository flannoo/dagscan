using Ardalis.GuardClauses;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public enum MetagraphTypes
{
    DagL1,
    MetagraphL0,
    MetagraphCurrencyL1,
    MetagraphDataL1
}

public sealed class Metagraph : Aggregate<MetagraphId>
{
    public HypergraphId HypergraphId { get; private set; } = default!;
    public string Name { get; private init; } = default!;
    public string Symbol { get; private init; } = default!;
    public WalletAddress? FeeAddress { get; private init; }
    public WalletAddress? StakingAddress { get; private init; }
    public MetagraphEndpoint[] MetagraphEndpoint { get; private init; } = [];
    public string? CompanyName { get; private init; }
    public string? Website { get; private init; }
    public string? Description { get; private init; }
    public bool DataSyncEnabled { get; private init; }
}
