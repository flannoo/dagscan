using DagScan.Application.Data;
using DagScan.Application.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DagScan.Application.Features.GetWalletBalances;

public sealed record GetWalletBalancesQuery(string HypergraphName, string WalletAddress)
    : IRequest<WalletBalancesDto?>;

internal sealed class GetMetagraphsQueryHandler(ReadOnlyDagContext dagContext)
    : IRequestHandler<GetWalletBalancesQuery, WalletBalancesDto?>
{
    public async Task<WalletBalancesDto?> Handle(GetWalletBalancesQuery request,
        CancellationToken cancellationToken)
    {
        var hypergraph = await dagContext.Hypergraphs.FirstOrDefaultAsync(x => x.Name == request.HypergraphName,
            cancellationToken: cancellationToken);

        if (hypergraph is null)
        {
            return null;
        }

        var hypergraphBalance = dagContext.HypergraphBalances.FirstOrDefault(x =>
            x.HypergraphId == hypergraph.Id && x.WalletAddress == new WalletAddress(request.WalletAddress));

        var metagraphs = await dagContext.Metagraphs.Where(x => x.HypergraphId == hypergraph.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        var metagraphBalancesDto = new List<MetagraphBalancesDto>();
        foreach (var metagraph in metagraphs)
        {
            var metagraphBalance = await dagContext.MetagraphBalances.FirstOrDefaultAsync(x =>
                x.WalletAddress == new WalletAddress(request.WalletAddress), cancellationToken);

            if (metagraph.MetagraphAddress is null)
            {
                continue;
            }

            metagraphBalancesDto.Add(new MetagraphBalancesDto(metagraph.MetagraphAddress.Value, metagraph.Symbol,
                metagraphBalance?.Balance ?? 0));
        }

        var walletBalancesDto =
            new WalletBalancesDto(request.WalletAddress, hypergraphBalance?.Balance ?? 0, metagraphBalancesDto);

        return walletBalancesDto;
    }
}
