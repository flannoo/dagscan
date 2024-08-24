using Ardalis.GuardClauses;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class HypergraphValidatorNodeId : ValueObject
{
    public Guid Value { get; }

    public HypergraphValidatorNodeId(Guid value)
    {
        Guard.Against.Default(value, nameof(value));

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public sealed class HypergraphValidatorNode : Entity<HypergraphValidatorNodeId>
{
    public string WalletHash { get; private init; } = default!;
    public string WalletId { get; private init; } = default!;
    public string IpAddress { get; private set; } = default!;
    public string State { get; private set; } = default!;
    public string? Version { get; private set; } = default!;
    public string? Provider { get; private set; }
    public string? Country { get; private set; }
    public string? City { get; private set; }
    public double? Latitude { get; private set; }
    public double? Longitude { get; private set; }
    public NodeOperator? NodeOperator { get; private set; }

    public static HypergraphValidatorNode Create(string walletId, string walletHash, string state,
        string ipAddress)
    {
        Guard.Against.NullOrWhiteSpace(walletHash, nameof(walletHash));
        Guard.Against.NullOrWhiteSpace(walletId, nameof(walletId));
        Guard.Against.NullOrWhiteSpace(state, nameof(state));
        Guard.Against.NullOrWhiteSpace(ipAddress, nameof(ipAddress));

        return new HypergraphValidatorNode()
        {
            Id = new HypergraphValidatorNodeId(Guid.NewGuid()),
            WalletHash = walletHash,
            WalletId = walletId,
            State = state,
            IpAddress = ipAddress
        };
    }

    public void UpdateNodeInfo(string state, string ipAddress)
    {
        Guard.Against.NullOrWhiteSpace(state, nameof(state));
        Guard.Against.NullOrWhiteSpace(ipAddress, nameof(ipAddress));

        State = state;
        IpAddress = ipAddress;
    }

    public void UpdateVersion(string version)
    {
        Guard.Against.NullOrWhiteSpace(version, nameof(version));
        Version = version;
    }

    public void UpdateProviderInfo(string provider, string country, string city, double latitude, double longitude)
    {
        Guard.Against.NullOrWhiteSpace(provider, nameof(provider));
        Guard.Against.NullOrWhiteSpace(country, nameof(country));
        Guard.Against.NullOrWhiteSpace(city, nameof(city));
        Guard.Against.Null(latitude, nameof(latitude));
        Guard.Against.Null(longitude, nameof(longitude));

        Provider = provider;
        Country = country;
        City = city;
        Latitude = latitude;
        Longitude = longitude;
    }

    public void UpdateNodeOperator(NodeOperator nodeOperator)
    {
        NodeOperator = nodeOperator;
    }
}
