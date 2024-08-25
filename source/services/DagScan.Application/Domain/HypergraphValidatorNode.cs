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
    public string WalletId { get; private init; } = default!;
    public string IpAddress { get; private set; } = default!;
    public string NodeStatus { get; private set; } = default!;
    public bool IsInConsensus { get; private set; }
    public DateTime LastModifiedUtc { get; private set; }
    public string? ServiceProvider { get; private set; }
    public string? Country { get; private set; }
    public string? City { get; private set; }
    public Coordinate? Coordinates { get; private set; }
    public NodeOperator? NodeOperator { get; private set; }

    public static HypergraphValidatorNode Create(HypergraphId hypergraphId, string walletId, WalletAddress walletAddress,
        string state, string ipAddress, bool isInConsensus)
    {
        Guard.Against.NullOrWhiteSpace(walletId, nameof(walletId));
        Guard.Against.NullOrWhiteSpace(state, nameof(state));
        Guard.Against.NullOrWhiteSpace(ipAddress, nameof(ipAddress));
        Guard.Against.Null(hypergraphId, nameof(hypergraphId));

        return new HypergraphValidatorNode()
        {
            Id = new HypergraphValidatorNodeId(Guid.NewGuid()),
            HypergraphId = hypergraphId,
            WalletAddress = walletAddress,
            WalletId = walletId,
            NodeStatus = state,
            IpAddress = ipAddress,
            IsInConsensus = isInConsensus,
            LastModifiedUtc = DateTime.UtcNow
        };
    }

    public void Created()
    {
        AddDomainEvent(new HypergraphValidatorNodeAdded(HypergraphId, Id));
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
    }

    public void MarkAsOffline()
    {
        NodeStatus = "Offline";
        IsInConsensus = false;
        LastModifiedUtc = DateTime.UtcNow;
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
    HypergraphId HypergraphId,
    HypergraphValidatorNodeId HypergraphValidatorNodeId)
    : DomainEvent, INotification;
