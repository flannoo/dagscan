using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using DagScan.Application.Data;
using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.CQRS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DagScan.Application.Features.SyncHypergraphValidatorNodes;

public sealed record UpsertHypergraphValidatorNodesCommand : ICommand<bool>
{
    public Guid HypergraphId { get; init; }
}

public sealed class UpsertHypergraphValidatorNodesCommandHandler(
    DagContext dagContext,
    IHttpClientFactory httpClientFactory)
    : IRequestHandler<UpsertHypergraphValidatorNodesCommand, bool>
{
    public async Task<bool> Handle(UpsertHypergraphValidatorNodesCommand request, CancellationToken cancellationToken)
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
        var validatorNodes = await httpClient.GetFromJsonAsync<HypergraphClusterInfo[]>("cluster/info",
            new JsonSerializerOptions(JsonSerializerDefaults.Web), cancellationToken: cancellationToken) ?? [];

        var inConsensusNodes = await httpClient.GetFromJsonAsync<HypergraphInConsensusInfo>("consensus/latest/peers",
            new JsonSerializerOptions(JsonSerializerDefaults.Web), cancellationToken: cancellationToken);

        if (validatorNodes.Length == 0 || inConsensusNodes is null)
        {
            return false;
        }

        var hypergraphNodes =
            await dagContext.HypergraphValidatorNodes.Where(x => x.HypergraphId == hypergraph.Id)
                .ToListAsync(cancellationToken: cancellationToken);

        foreach (var validatorNode in validatorNodes.ToList())
        {
            var persistedNode = hypergraphNodes.FirstOrDefault(x => x.WalletId.Value == validatorNode.Id);
            var nodeIsInConsensus = inConsensusNodes.Peers.Any(x => x.Id == validatorNode.Id);

            if (persistedNode is null)
            {
                var newNode = HypergraphValidatorNode.Create(hypergraph.Id, new WalletId(validatorNode.Id),
                    WalletAddress.CreateFromWalletId(validatorNode.Id),
                    validatorNode.State, validatorNode.Ip, nodeIsInConsensus);

                await dagContext.HypergraphValidatorNodes.AddAsync(newNode, cancellationToken);
                newNode.Created();
            }
            else
            {
                if (persistedNode.NodeStatus != validatorNode.State || persistedNode.IsInConsensus != nodeIsInConsensus)
                {
                    persistedNode.UpdateNodeStatus(validatorNode.State, nodeIsInConsensus);
                }

                if (persistedNode.IpAddress != validatorNode.Ip)
                {
                    persistedNode.UpdateNodeIpAddress(validatorNode.Ip);
                }
            }
        }

        // Mark nodes as offline if they are not included in validatorNodes API response and not already marked as offline
        foreach (var persistedNode in hypergraphNodes.Where(x => x.NodeStatus != "Offline"))
        {
            if (!hypergraphNodes.Select(x => x.WalletId).Contains(persistedNode.WalletId))
            {
                persistedNode.UpdateNodeStatus("Offline", false);
            }
        }

        return true;
    }
}
