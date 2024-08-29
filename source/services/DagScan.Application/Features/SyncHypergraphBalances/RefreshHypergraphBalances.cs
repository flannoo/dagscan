using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using DagScan.Application.Data;
using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.CQRS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DagScan.Application.Features.SyncHypergraphBalances;

public sealed record RefreshHypergraphBalancesCommand : ICommand<bool>
{
    public Guid HypergraphId { get; init; }
}

public sealed class RefreshHypergraphBalancesCommandHandler(
    DagContext dagContext,
    IHttpClientFactory httpClientFactory)
    : IRequestHandler<RefreshHypergraphBalancesCommand, bool>
{
    public async Task<bool> Handle(RefreshHypergraphBalancesCommand request, CancellationToken cancellationToken)
    {
        using var httpClient = httpClientFactory.CreateClient();

        var hypergraph = await dagContext.Hypergraphs
            .FirstOrDefaultAsync(x => x.Id == new HypergraphId(request.HypergraphId), cancellationToken);

        if (hypergraph is null)
        {
            return false;
        }

        httpClient.BaseAddress = new Uri(hypergraph.ApiBaseAddress);
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var latestSnapshot = await httpClient.GetFromJsonAsync<dynamic>("global-snapshots/latest/combined",
            new JsonSerializerOptions(JsonSerializerDefaults.Web), cancellationToken: cancellationToken);

        if (latestSnapshot is null)
        {
            return false;
        }

        var hypergraphBalances = new List<HypergraphBalance>();
        var balancesElement = latestSnapshot[1].GetProperty("balances");

        foreach (var balance in balancesElement.EnumerateObject())
        {
            hypergraphBalances.Add(HypergraphBalance.Create(hypergraph.Id,
                new WalletAddress(balance.Name),
                balance.Value.GetInt64()));
        }

        hypergraph.RefreshBalances(hypergraphBalances);

        return true;
    }
}
