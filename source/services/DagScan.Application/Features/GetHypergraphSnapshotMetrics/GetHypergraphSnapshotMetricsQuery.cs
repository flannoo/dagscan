using DagScan.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DagScan.Application.Features.GetHypergraphSnapshotMetrics;

public sealed record GetHypergraphSnapshotMetricsQuery(
    string HypergraphName,
    DateOnly? FromDate,
    DateOnly? ToDate)
    : IRequest<List<HypergraphSnapshotMetricsDto>>;

public sealed class GetHypergraphSnapshotMetricsQueryHandler(ReadOnlyDagContext dagContext)
    : IRequestHandler<GetHypergraphSnapshotMetricsQuery, List<HypergraphSnapshotMetricsDto>>
{
    public async Task<List<HypergraphSnapshotMetricsDto>> Handle(GetHypergraphSnapshotMetricsQuery request,
        CancellationToken cancellationToken)
    {
        var hypergraph = await dagContext.Hypergraphs.FirstOrDefaultAsync(x => x.Name == request.HypergraphName,
            cancellationToken: cancellationToken);

        if (hypergraph is null)
        {
            return [];
        }

        var startDate = request.FromDate ?? DateOnly.FromDateTime(DateTime.Today.Date.AddMonths(-6));
        var endDate = request.ToDate ?? DateOnly.FromDateTime(DateTime.Today.Date.AddDays(1));

        var snapshots = await dagContext.HypergraphSnapshots.Where(x => x.HypergraphId == hypergraph.Id)
            .Where(snapshot => snapshot.Timestamp >= startDate.ToDateTime(TimeOnly.MinValue) &&
                               snapshot.Timestamp <= endDate.ToDateTime(TimeOnly.MaxValue))
            .GroupBy(snapshot =>
                new { snapshot.Timestamp.Date, snapshot.IsTimeTriggeredSnapshot, snapshot.MetagraphAddress })
            .Select(snapshot => new HypergraphSnapshotMetricsDto(
                DateOnly.FromDateTime(snapshot.Key.Date),
                snapshot.Key.MetagraphAddress != null ? snapshot.Key.MetagraphAddress.Value : null,
                snapshot.Key.IsTimeTriggeredSnapshot,
                snapshot.Count(),
                snapshot.Sum(x => x.FeeAmount ?? 0),
                snapshot.Sum(x => x.TransactionCount ?? 0),
                snapshot.Sum(x => x.TransactionAmount ?? 0),
                snapshot.Sum(x => x.TransactionFeeAmount ?? 0)))
            .ToListAsync(cancellationToken);

        return snapshots;
    }
}
