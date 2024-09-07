using DagScan.Application.Data;
using DagScan.Application.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DagScan.Application.Features.GetHypergraphValidatorNodesUptime;

public sealed record GetHypergraphValidatorNodesUptimeQuery(
    string HypergraphName,
    string WalletAddress,
    DateOnly? FromDate,
    DateOnly? ToDate) : IRequest<List<HypergraphValidatorNodesUptimeDto>>;

internal sealed class GetHypergraphValidatorNodesUptimeQueryHandler(ReadOnlyDagContext dagContext)
    : IRequestHandler<GetHypergraphValidatorNodesUptimeQuery, List<HypergraphValidatorNodesUptimeDto>>
{
    public async Task<List<HypergraphValidatorNodesUptimeDto>> Handle(GetHypergraphValidatorNodesUptimeQuery request,
        CancellationToken cancellationToken)
    {
        var hypergraph = await dagContext.Hypergraphs.FirstOrDefaultAsync(x => x.Name == request.HypergraphName,
            cancellationToken: cancellationToken);

        if (hypergraph is null)
        {
            return [];
        }

        var startDate = request.FromDate ?? DateOnly.FromDateTime(DateTime.Today.AddDays(-7));
        var endDate = request.ToDate ?? DateOnly.FromDateTime(DateTime.Today);

        var maxSnapshotCountsByDay = await dagContext.HypergraphValidatorNodesParticipants
            .Where(x =>
                x.SnapshotDate >= startDate &&
                x.SnapshotDate <= endDate &&
                x.HypergraphId == hypergraph.Id)
            .GroupBy(x => x.SnapshotDate)
            .Select(g => new { Date = g.Key, MaxSnapshotCount = g.Max(x => x.SnapshotCount) })
            .ToListAsync(cancellationToken);

        var walletSnapshotCountsByDay = await dagContext.HypergraphValidatorNodesParticipants
            .Where(x =>
                x.SnapshotDate >= startDate &&
                x.SnapshotDate <= endDate &&
                x.WalletAddress == new WalletAddress(request.WalletAddress) &&
                x.HypergraphId == hypergraph.Id)
            .GroupBy(x => x.SnapshotDate)
            .Select(g => new { Date = g.Key, SnapshotCount = g.Sum(x => x.SnapshotCount) })
            .ToListAsync(cancellationToken);

        if (walletSnapshotCountsByDay.Count == 0)
        {
            return [];
        }

        var uptimes = from max in maxSnapshotCountsByDay
            join wallet in walletSnapshotCountsByDay on max.Date equals wallet.Date into walletGroup
            from w in walletGroup.DefaultIfEmpty()
            select new HypergraphValidatorNodesUptimeDto(
                max.Date,
                w.SnapshotCount,
                max.MaxSnapshotCount,
                (max.MaxSnapshotCount > 0 && w != null)
                    ? (int)((double)w.SnapshotCount / max.MaxSnapshotCount * 100)
                    : 0
            );

        return uptimes.ToList();
    }
}
