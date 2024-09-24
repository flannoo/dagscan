using DagScan.Application.Data;
using DagScan.Application.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DagScan.Application.Features.GetWallets;

public sealed record GetWalletsQuery(string HypergraphName, string? MetagraphAddress)
    : IRequest<List<WalletDto>>;

internal sealed class GetWalletsQueryHandler(ReadOnlyDagContext dagContext)
    : IRequestHandler<GetWalletsQuery, List<WalletDto>>
{
    public async Task<List<WalletDto>> Handle(GetWalletsQuery request,
        CancellationToken cancellationToken)
    {
        var hypergraph = await dagContext.Hypergraphs.FirstOrDefaultAsync(x => x.Name == request.HypergraphName,
            cancellationToken: cancellationToken);

        if (hypergraph is null)
        {
            return [];
        }

        var walletTags = await dagContext.WalletTags.ToListAsync(cancellationToken);

        var walletBalances = new List<WalletDto>();
        List<WalletInfo> wallets;

        var currencyPrice = 0m;

        if (request.MetagraphAddress is null)
        {
            var persistedCurrencyPrice =
                await dagContext.CurrencyPrices.OrderByDescending(x => x.Date).FirstOrDefaultAsync(
                    x => x.MetagraphAddress == null, cancellationToken);

            currencyPrice = persistedCurrencyPrice?.Price ?? 0m;

            wallets = await dagContext.HypergraphBalances
                .Where(x => x.HypergraphId == hypergraph.Id && x.Balance > 0)
                .OrderByDescending(x => x.Balance).Select(x => new WalletInfo(x.WalletAddress, x.Balance))
                .ToListAsync(cancellationToken);
        }
        else
        {
            var persistedCurrencyPrice =
                await dagContext.CurrencyPrices.OrderByDescending(x => x.Date).FirstOrDefaultAsync(
                    x => x.MetagraphAddress == new MetagraphAddress(request.MetagraphAddress), cancellationToken);

            currencyPrice = persistedCurrencyPrice?.Price ?? 0m;

            wallets = await dagContext.MetagraphBalances
                .Where(x => x.MetagraphAddress == new MetagraphAddress(request.MetagraphAddress) && x.Balance > 0)
                .OrderByDescending(x => x.Balance).Select(x => new WalletInfo(x.WalletAddress, x.Balance))
                .ToListAsync(cancellationToken);
        }

        var totalBalance = wallets.Sum(x => x.Balance);

        var rank = 1;
        foreach (var wallet in wallets)
        {
            var supplyPercentage = totalBalance != 0 ? (double)wallet.Balance / totalBalance * 100 : 0;
            var walletTag = walletTags.FirstOrDefault(x => x.WalletAddress == wallet.WalletAddress);
            var usdPrice = wallet.Balance != 0 ? (wallet.Balance / 100_000_000m) * currencyPrice : 0;
            walletBalances.Add(new WalletDto(
                rank,
                wallet.WalletAddress.Value,
                walletTag?.Tag,
                wallet.Balance,
                usdPrice,
                supplyPercentage));
            rank++;
        }

        return walletBalances;
    }
}
