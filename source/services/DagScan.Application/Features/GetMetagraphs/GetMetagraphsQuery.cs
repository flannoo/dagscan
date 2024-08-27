using DagScan.Application.Data;
using DagScan.Application.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DagScan.Application.Features.GetMetagraphs;

public sealed record GetMetagraphsQuery(string HypergraphName) : IRequest<List<MetagraphsDto>>;

internal sealed class GetMetagraphsQueryHandler(ReadOnlyDagContext dagContext)
    : IRequestHandler<GetMetagraphsQuery, List<MetagraphsDto>>
{
    public async Task<List<MetagraphsDto>> Handle(GetMetagraphsQuery request, CancellationToken cancellationToken)
    {
        var hypergraph = await dagContext.Hypergraphs.FirstOrDefaultAsync(x => x.Name == request.HypergraphName,
            cancellationToken: cancellationToken);

        if (hypergraph is null)
        {
            return [];
        }

        var metagraphs = await dagContext.Metagraphs.Where(x => x.HypergraphId == hypergraph.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        var metagraphDtos = metagraphs.Select(metagraph => new MetagraphsDto(
            MetagraphAddress: metagraph.MetagraphAddress?.Value,
            Name: metagraph.Name,
            Symbol: metagraph.Symbol,
            FeeAddress: metagraph.FeeAddress?.Value,
            CompanyName: metagraph.CompanyName,
            Website: metagraph.Website,
            Description: metagraph.Description,
            L0ApiUrl: metagraph.MetagraphEndpoints.FirstOrDefault(x => x.MetagraphType == MetagraphTypes.MetagraphL0)
                ?.ApiBaseAddress,
            L1DataApiUrl: metagraph.MetagraphEndpoints
                .FirstOrDefault(x => x.MetagraphType == MetagraphTypes.MetagraphDataL1)?.ApiBaseAddress,
            L1CurrencyApiUrl: metagraph.MetagraphEndpoints
                .FirstOrDefault(x => x.MetagraphType == MetagraphTypes.MetagraphCurrencyL1)?.ApiBaseAddress
        )).ToList();

        return metagraphDtos;
    }
}
