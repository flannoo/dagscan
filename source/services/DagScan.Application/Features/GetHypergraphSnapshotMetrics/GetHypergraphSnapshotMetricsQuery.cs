using DagScan.Application.Data;
using DagScan.Application.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DagScan.Application.Features.GetHypergraphSnapshotMetrics;

public sealed record GetHypergraphSnapshotMetricsQuery(
    string HypergraphName,
    DateOnly? FromDate,
    DateOnly? ToDate)
    : IRequest<List<HypergraphSnapshotMetric>>;

public sealed class GetHypergraphSnapshotMetricsQueryHandler(ReadOnlyDagContext dagContext)
    : IRequestHandler<GetHypergraphSnapshotMetricsQuery, List<HypergraphSnapshotMetric>>
{
    public async Task<List<HypergraphSnapshotMetric>> Handle(GetHypergraphSnapshotMetricsQuery request,
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

        var snapshots = await dagContext.HypergraphSnapshotMetrics
            .Where(x => x.SnapshotDate >= startDate && x.SnapshotDate <= endDate)
            .OrderBy(x => x.SnapshotDate).ToListAsync(cancellationToken);

        return snapshots;
    }
}
