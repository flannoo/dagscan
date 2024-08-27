using DagScan.Application.Data;
using DagScan.Application.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DagScan.Application.Features.GetHypergraphValidatorNodes;

public sealed record GetHypergraphValidatorNodesQuery(string HypergraphName) : IRequest<List<HypergraphValidatorNodesDto>>;

internal sealed class GetHypergraphValidatorNodesQueryHandler(ReadOnlyDagContext dagContext)
    : IRequestHandler<GetHypergraphValidatorNodesQuery, List<HypergraphValidatorNodesDto>>
{
    public async Task<List<HypergraphValidatorNodesDto>> Handle(GetHypergraphValidatorNodesQuery request,
        CancellationToken cancellationToken)
    {
        var hypergraph = await dagContext.Hypergraphs.FirstOrDefaultAsync(x => x.Name == request.HypergraphName,
            cancellationToken: cancellationToken);

        if (hypergraph is null)
        {
            return [];
        }

        var validatorNodes = await dagContext.HypergraphValidatorNodes.Where(x => x.HypergraphId == hypergraph.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        var validatorNodeDtos = validatorNodes.Select(node => new HypergraphValidatorNodesDto(
            WalletAddress: node.WalletAddress.Value,
            WalletId: node.WalletId.Value,
            IpAddress: node.IpAddress,
            NodeStatus: node.NodeStatus,
            IsInConsensus: node.IsInConsensus,
            ServiceProvider: node.ServiceProvider,
            Country: node.Country,
            City: node.City,
            Latitude: node.Coordinates?.Latitude,
            Longitude: node.Coordinates?.Longitude
        )).ToList();

        return validatorNodeDtos;
    }
}
