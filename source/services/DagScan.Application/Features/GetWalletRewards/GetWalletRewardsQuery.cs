using DagScan.Application.Data;
using DagScan.Application.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DagScan.Application.Features.GetWalletRewards;

public sealed record GetWalletRewardsQuery(
    string HypergraphName,
    string WalletAddresses,
    DateOnly? FromDate,
    DateOnly? ToDate)
    : IRequest<List<WalletRewardDto>>;

public sealed class GetWalletRewardsQueryHandler(ReadOnlyDagContext dagContext)
    : IRequestHandler<GetWalletRewardsQuery, List<WalletRewardDto>>
{
    public async Task<List<WalletRewardDto>> Handle(GetWalletRewardsQuery request, CancellationToken cancellationToken)
    {
        var hypergraph = await dagContext.Hypergraphs.FirstOrDefaultAsync(x => x.Name == request.HypergraphName,
            cancellationToken: cancellationToken);

        if (hypergraph is null)
        {
            return [];
        }

        var startDate = request.FromDate ?? DateOnly.FromDateTime(DateTime.Today.AddDays(-7));
        var endDate = request.ToDate ?? DateOnly.FromDateTime(DateTime.Today);

        var metagraphs = await dagContext.Metagraphs.Where(x => x.HypergraphId == hypergraph.Id)
            .Select(x => new { x.Id, x.MetagraphAddress, x.Symbol })
            .ToListAsync(cancellationToken);

        var metagraphLookup = metagraphs.ToDictionary(m => m.Id, m => m.Symbol);

        var walletAddresses = request.WalletAddresses
            .Split(',')
            .Select(address => address.Trim())
            .Select(address => new WalletAddress(address))
            .ToList();

        var hypergraphSnapshotRewards = await dagContext.HypergraphSnapshotReward.Where(x =>
                x.HypergraphId == hypergraph.Id &&
                walletAddresses.Contains(x.WalletAddress) &&
                x.RewardDate >= startDate &&
                x.RewardDate <= endDate)
            .Select(x => new WalletRewardDto(x.WalletAddress.Value, x.LastReceivedUtc, null,
                x.RewardAmount, "Protocol Reward", null, null, "DAG"))
            .ToListAsync(cancellationToken);

        var metagraphSnapshotRewards = await dagContext.MetagraphSnapshotRewards.Where(x =>
                metagraphs.Select(metagraph => metagraph.Id).Contains(x.MetagraphId) &&
                walletAddresses.Contains(x.WalletAddress) &&
                x.RewardDate >= startDate.ToDateTime(TimeOnly.MinValue) &&
                x.RewardDate <= endDate.ToDateTime(TimeOnly.MaxValue))
            .Select(x => new WalletRewardDto(x.WalletAddress.Value, x.RewardDate, x.MetagraphAddress.Value,
                x.RewardAmount, "Metagraph Reward", null, x.SnapshotOrdinal, metagraphLookup[x.MetagraphId]))
            .ToListAsync(cancellationToken);


        var walletTransactionRewards = await dagContext.RewardTransactions.Where(x =>
                metagraphs.Select(metagraph => metagraph.Id).Contains(x.MetagraphId) &&
                walletAddresses.Contains(x.WalletAddress) &&
                x.TransactionDate >= startDate.ToDateTime(TimeOnly.MinValue) &&
                x.TransactionDate <= endDate.ToDateTime(TimeOnly.MaxValue))
            .Select(x => new WalletRewardDto(x.WalletAddress.Value, x.TransactionDate,
                x.MetagraphAddress != null ? x.MetagraphAddress.Value : null,
                x.Amount, x.RewardCategory, x.TransactionHash, null, metagraphLookup[x.MetagraphId]))
            .ToListAsync(cancellationToken);

        var rewards = hypergraphSnapshotRewards
            .Concat(metagraphSnapshotRewards)
            .Concat(walletTransactionRewards);

        return rewards.OrderByDescending(x => x.TransactionDate).ToList();
    }
}
