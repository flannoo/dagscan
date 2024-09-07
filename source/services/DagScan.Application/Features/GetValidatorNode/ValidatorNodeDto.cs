using DagScan.Application.Domain;

namespace DagScan.Application.Features.GetValidatorNode;

public sealed class ValidatorNodeDto()
{
    public HypergraphValidatorNodeDto? HypergraphValidatorNodeDto { get; set; }
    public List<MetagraphValidatorNodeDto>? MetagraphNodes { get; set; }
}

public sealed record MetagraphValidatorNodeDto(
    string WalletAddress,
    string WalletId,
    string? MetagraphAddress,
    string MetagraphType,
    string IpAddress,
    string NodeStatus,
    string? ServiceProvider,
    string? Country,
    string? City,
    double? Latitude,
    double? Longitude);

public sealed record HypergraphValidatorNodeDto(
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
