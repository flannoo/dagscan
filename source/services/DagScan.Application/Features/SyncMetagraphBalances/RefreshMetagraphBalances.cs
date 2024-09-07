using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using DagScan.Application.Data;
using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.CQRS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Features.SyncMetagraphBalances;

public sealed record RefreshMetagraphBalancesCommand : ICommand<bool>
{
    public Guid MetagraphId { get; init; }
}

public sealed class RefreshMetagraphBalancesCommandHandler(
    DagContext dagContext,
    IHttpClientFactory httpClientFactory,
    ILogger<RefreshMetagraphBalancesCommandHandler> logger)
    : IRequestHandler<RefreshMetagraphBalancesCommand, bool>
{
    public async Task<bool> Handle(RefreshMetagraphBalancesCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Refreshing balances for {MetagraphId}", request.MetagraphId);

        using var httpClient = httpClientFactory.CreateClient();

        var metagraph = await dagContext.Metagraphs
            .FirstOrDefaultAsync(x => x.Id == new MetagraphId(request.MetagraphId) && x.MetagraphAddress != null,
                cancellationToken);

        if (metagraph is null || metagraph.MetagraphAddress is null)
        {
            return false;
        }

        var hypergraph = await dagContext.Hypergraphs
            .FirstOrDefaultAsync(x => x.Id == new HypergraphId(metagraph.HypergraphId.Value), cancellationToken);

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

        var metagraphBalances = new List<MetagraphBalance>();

        var lastCurrencySnapshots = latestSnapshot[1].GetProperty("lastCurrencySnapshots");
        var metagraphData = lastCurrencySnapshots.GetProperty($"{metagraph.MetagraphAddress.Value}");
        var rightElement = metagraphData.GetProperty("Right");

        var balancesElement = rightElement[1].GetProperty("balances");

        if (balancesElement is null)
        {
            return false;
        }

        foreach (var balance in balancesElement.EnumerateObject())
        {
            metagraphBalances.Add(MetagraphBalance.Create(metagraph.Id, metagraph.MetagraphAddress,
                new WalletAddress(balance.Name),
                balance.Value.GetInt64()));
        }

        await dagContext.Database.ExecuteSqlRawAsync(
            "DELETE FROM MetagraphBalances WHERE MetagraphId = {0}",
            metagraph.Id.Value);

        dagContext.MetagraphBalances.AddRange(metagraphBalances);

        return true;
    }
}
