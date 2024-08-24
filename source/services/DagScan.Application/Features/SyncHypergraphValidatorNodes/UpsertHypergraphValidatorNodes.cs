using System.Net.Http.Json;
using System.Text.Json;
using DagScan.Application.Data;
using DagScan.Application.Domain;
using DagScan.Core.CQRS;
using DagScan.Core.Extensions;
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
        var validatorNodes = await httpClient.GetFromJsonAsync<HypergraphClusterInfo[]>("cluster/info",
            new JsonSerializerOptions(JsonSerializerDefaults.Web), cancellationToken: cancellationToken) ?? [];

        if (validatorNodes.Length == 0)
        {
            return false;
        }

        var hypergraphNodes =
            await dagContext.HypergraphValidatorNodes.ToListAsync(cancellationToken: cancellationToken);

        foreach (var validatorNode in validatorNodes.ToList())
        {
            var persistedNode = hypergraphNodes.FirstOrDefault(x => x.WalletId == validatorNode.Id);

            if (persistedNode is null)
            {
                var newNode = HypergraphValidatorNode.Create(hypergraph.Id, validatorNode.Id,
                    validatorNode.Id.ConvertNodeIdToWalletHash(),
                    validatorNode.State, validatorNode.Ip);

                await dagContext.HypergraphValidatorNodes.AddAsync(newNode, cancellationToken);
                newNode.Created();
            }
            else
            {
                persistedNode.UpdateNodeInfo(validatorNode.State, validatorNode.Ip);
            }
        }

        // Mark nodes as offline if they are not included in validatorNodes API response
        foreach (var persistedNode in hypergraphNodes)
        {
            if (!hypergraphNodes.Select(x => x.WalletId).Contains(persistedNode.WalletId))
            {
                persistedNode.MarkAsOffline();
            }
        }

        return true;
    }
}
