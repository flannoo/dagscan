namespace DagScan.Application.Features.GetHypergraphValidatorNodes;

public sealed record HypergraphValidatorNodesDto(
    string WalletAddress,
    string WalletId,
    string IpAddress,
    string NodeStatus,
    bool IsInConsensus,
    string? ServiceProvider,
    string? Country,
    string? City,
    double? Latitude,
    double? Longitude);
