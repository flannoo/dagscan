namespace DagScan.Application.Features.GetMetagraphValidatorNodes;

public sealed record MetagraphValidatorNodesDto(
    string WalletAddress,
    string WalletId,
    string IpAddress,
    string NodeStatus,
    string? ServiceProvider,
    string? Country,
    string? City,
    double? Latitude,
    double? Longitude);
