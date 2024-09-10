using System.Net.Http.Headers;
using System.Text.Json;
using DagScan.Application.Data;
using DagScan.Application.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DagScan.Application.Features.GetMetagraphSnapshotOnChainData;

public sealed record GetMetagraphSnapshotOnChainDataQuery(
    string HypergraphName,
    string MetagraphAddress,
    long Ordinal) : IRequest<int[]>;

internal sealed class GetHypergraphValidatorNodesUptimeQueryHandler(
    ReadOnlyDagContext dagContext,
    IHttpClientFactory httpClientFactory)
    : IRequestHandler<GetMetagraphSnapshotOnChainDataQuery, int[]>
{
    public async Task<int[]> Handle(GetMetagraphSnapshotOnChainDataQuery request,
        CancellationToken cancellationToken)
    {
        var hypergraph = await dagContext.Hypergraphs.FirstOrDefaultAsync(x => x.Name == request.HypergraphName,
            cancellationToken: cancellationToken);

        if (hypergraph is null)
        {
            return [];
        }

        var metagraphs = await dagContext.Metagraphs.Where(x =>
            x.HypergraphId == hypergraph.Id).ToListAsync(cancellationToken);

        var metagraph = metagraphs.FirstOrDefault(x =>
            x.MetagraphAddress != null &&
            x.MetagraphAddress.Value == request.MetagraphAddress);

        if (metagraph is null)
        {
            return [];
        }

        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);

        using var httpClient = httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.BaseAddress =
            new Uri(metagraph.MetagraphEndpoints.First(x => x.MetagraphType == MetagraphTypes.MetagraphL0)
                .ApiBaseAddress);

        var response = await httpClient.GetAsync($"snapshots/{request.Ordinal}", cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return [];
        }

        var result = JsonSerializer.Deserialize<MetagraphSnapshotDto>(responseBody, options);
        if (result is null)
        {
            return [];
        }

        return result.Value.DataApplication?.OnChainState ?? [];
    }
}
