using Ardalis.GuardClauses;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.CQRS;
using DagScan.Core.DDD;
using MediatR;

namespace DagScan.Application.Domain;

public sealed class HypergraphValidatorNode : Aggregate<HypergraphValidatorNodeId>
{
    public HypergraphId HypergraphId { get; private init; } = default!;
    public WalletAddress WalletAddress { get; private init; } = default!;
    public WalletId WalletId { get; private init; } = default!;
    public string IpAddress { get; private set; } = default!;
    public string NodeStatus { get; private set; } = default!;
    public bool IsInConsensus { get; private set; }
    public DateTime LastModifiedUtc { get; private set; }
    public string? ServiceProvider { get; private set; }
    public string? Country { get; private set; }
    public string? City { get; private set; }
    public Coordinate? Coordinates { get; private set; }
    public NodeOperator? NodeOperator { get; private set; }

    public static HypergraphValidatorNode Create(HypergraphId hypergraphId, WalletId walletId,
        WalletAddress walletAddress,
        string nodeStatus, string ipAddress, bool isInConsensus)
    {
        Guard.Against.NullOrWhiteSpace(nodeStatus, nameof(nodeStatus));
        Guard.Against.NullOrWhiteSpace(ipAddress, nameof(ipAddress));
        Guard.Against.Null(hypergraphId, nameof(hypergraphId));
        Guard.Against.Null(walletId, nameof(walletId));
        Guard.Against.Null(walletAddress, nameof(walletAddress));

        return new HypergraphValidatorNode()
        {
            Id = new HypergraphValidatorNodeId(Guid.NewGuid()),
            HypergraphId = hypergraphId,
            WalletAddress = walletAddress,
            WalletId = walletId,
            NodeStatus = nodeStatus,
            IpAddress = ipAddress,
            IsInConsensus = isInConsensus,
            LastModifiedUtc = DateTime.UtcNow
        };
    }

    public void Created()
    {
        AddDomainEvent(new HypergraphValidatorNodeAdded(Id));
    }

    public void UpdateNodeStatus(string nodeStatus, bool isInConsensus)
    {
        Guard.Against.NullOrWhiteSpace(nodeStatus, nameof(nodeStatus));

        NodeStatus = nodeStatus;
        IsInConsensus = isInConsensus;
        LastModifiedUtc = DateTime.UtcNow;
    }

    public void UpdateNodeIpAddress(string ipAddress)
    {
        Guard.Against.NullOrWhiteSpace(ipAddress, nameof(ipAddress));

        IpAddress = ipAddress;
        LastModifiedUtc = DateTime.UtcNow;

        AddDomainEvent(new HypergraphValidatorNodeIpAddressChanged(Id));
    }

    public void UpdateServiceProviderInfo(string serviceProvider, string country, string city, Coordinate coordinates)
    {
        Guard.Against.NullOrWhiteSpace(serviceProvider, nameof(serviceProvider));
        Guard.Against.NullOrWhiteSpace(country, nameof(country));
        Guard.Against.NullOrWhiteSpace(city, nameof(city));

        ServiceProvider = serviceProvider;
        Country = country;
        City = city;
        Coordinates = coordinates;
        LastModifiedUtc = DateTime.UtcNow;
    }

    public void UpdateNodeOperator(NodeOperator nodeOperator)
    {
        NodeOperator = nodeOperator;
    }
}

public sealed record HypergraphValidatorNodeAdded(
    HypergraphValidatorNodeId HypergraphValidatorNodeId)
    : DomainEvent, INotification;

public sealed record HypergraphValidatorNodeIpAddressChanged(
    HypergraphValidatorNodeId HypergraphValidatorNodeId)
    : DomainEvent, INotification;
