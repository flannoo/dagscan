using DagScan.Application.Data;
using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DagScan.Application.Features.GetMetagraphValidatorNodes;

public sealed record GetMetagraphValidatorNodesQuery(string HypergraphName, string? MetagraphAddress)
    : IRequest<List<MetagraphValidatorNodesDto>>;

internal sealed class GetMetagraphValidatorNodesQueryHandler(ReadOnlyDagContext dagContext)
    : IRequestHandler<GetMetagraphValidatorNodesQuery, List<MetagraphValidatorNodesDto>>
{
    public async Task<List<MetagraphValidatorNodesDto>> Handle(GetMetagraphValidatorNodesQuery request,
        CancellationToken cancellationToken)
    {
        var hypergraph = await dagContext.Hypergraphs.FirstOrDefaultAsync(x => x.Name == request.HypergraphName,
            cancellationToken: cancellationToken);

        if (hypergraph is null)
        {
            return [];
        }

        var metagraphs = await dagContext.Metagraphs
            .Where(x => x.HypergraphId == hypergraph.Id)
            .ToListAsync(cancellationToken);

        var metagraphAddress = request.MetagraphAddress != null ? new MetagraphAddress(request.MetagraphAddress) : null;
        var metagraph = metagraphs.FirstOrDefault(
            x => x.MetagraphAddress == metagraphAddress);

        if (metagraph is null)
        {
            return [];
        }

        var validatorNodes = await dagContext.MetagraphValidatorNodes.Where(x => x.MetagraphId == metagraph.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        var validatorNodeDtos = validatorNodes.Select(node => new MetagraphValidatorNodesDto(
            WalletAddress: node.WalletAddress.Value,
            WalletId: node.WalletId.Value,
            MetagraphType: node.MetagraphType.ToString(),
            IpAddress: node.IpAddress,
            NodeStatus: node.NodeStatus,
            ServiceProvider: node.ServiceProvider,
            Country: node.Country,
            City: node.City,
            Latitude: node.Coordinates?.Latitude,
            Longitude: node.Coordinates?.Longitude
        )).ToList();

        return validatorNodeDtos;
    }
}
