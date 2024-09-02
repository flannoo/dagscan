using Ardalis.GuardClauses;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.CQRS;
using DagScan.Core.DDD;
using MediatR;

namespace DagScan.Application.Domain;

public sealed class MetagraphValidatorNode : Aggregate<MetagraphValidatorNodeId>
{
    public MetagraphId MetagraphId { get; private init; } = default!;
    public MetagraphTypes MetagraphType { get; private init; }
    public WalletAddress WalletAddress { get; private init; } = default!;
    public WalletId WalletId { get; private init; } = default!;
    public string IpAddress { get; private set; } = default!;
    public string NodeStatus { get; private set; } = default!;
    public DateTime LastModifiedUtc { get; private set; }
    public string? ServiceProvider { get; private set; }
    public string? Country { get; private set; }
    public string? City { get; private set; }
    public Coordinate? Coordinates { get; private set; }
    public NodeOperator? NodeOperator { get; private set; }

    public static MetagraphValidatorNode Create(HypergraphId hypergraphId, MetagraphId metagraphId,
        MetagraphTypes metagraphType, WalletId walletId, WalletAddress walletAddress,
        string nodeStatus, string ipAddress)
    {
        Guard.Against.NullOrWhiteSpace(nodeStatus, nameof(nodeStatus));
        Guard.Against.NullOrWhiteSpace(ipAddress, nameof(ipAddress));
        Guard.Against.Null(hypergraphId, nameof(hypergraphId));
        Guard.Against.Null(metagraphId, nameof(metagraphId));
        Guard.Against.Null(walletId, nameof(walletId));
        Guard.Against.Null(walletAddress, nameof(walletAddress));
        Guard.Against.Null(metagraphType, nameof(metagraphType));

        return new MetagraphValidatorNode()
        {
            Id = new MetagraphValidatorNodeId(Guid.NewGuid()),
            MetagraphId = metagraphId,
            MetagraphType = metagraphType,
            WalletAddress = walletAddress,
            WalletId = walletId,
            NodeStatus = nodeStatus,
            IpAddress = ipAddress,
            LastModifiedUtc = DateTime.UtcNow
        };
    }

    public void Created()
    {
        AddDomainEvent(new MetagraphValidatorNodeAdded(Id));
    }

    public void UpdateNodeStatus(string nodeStatus)
    {
        Guard.Against.NullOrWhiteSpace(nodeStatus, nameof(nodeStatus));

        NodeStatus = nodeStatus;
        LastModifiedUtc = DateTime.UtcNow;
    }

    public void UpdateNodeIpAddress(string ipAddress)
    {
        Guard.Against.NullOrWhiteSpace(ipAddress, nameof(ipAddress));

        IpAddress = ipAddress;
        LastModifiedUtc = DateTime.UtcNow;

        AddDomainEvent(new MetagraphValidatorNodeIpAddressChanged(Id));
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

public sealed record MetagraphValidatorNodeAdded(
    MetagraphValidatorNodeId MetagraphValidatorNodeId)
    : DomainEvent, INotification;

public sealed record MetagraphValidatorNodeIpAddressChanged(
    MetagraphValidatorNodeId MetagraphValidatorNodeId)
    : DomainEvent, INotification;
