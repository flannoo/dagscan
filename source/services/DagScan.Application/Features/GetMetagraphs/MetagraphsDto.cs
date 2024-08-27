namespace DagScan.Application.Features.GetMetagraphs;

public sealed record MetagraphsDto(
    string? MetagraphAddress,
    string Name,
    string Symbol,
    string? FeeAddress,
    string? CompanyName,
    string? Website,
    string? Description);
