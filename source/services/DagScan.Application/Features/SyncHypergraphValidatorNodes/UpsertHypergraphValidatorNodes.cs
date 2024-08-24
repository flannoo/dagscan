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

        var hypergraph = await dagContext.Hypergraphs.Include(x => x.HypergraphValidatorNodes)
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

        foreach (var validatorNode in validatorNodes.ToList())
        {
            var persistedNode =
                hypergraph.HypergraphValidatorNodes.FirstOrDefault(x => x.WalletId == validatorNode.Id);

            if (persistedNode is null)
            {
                var newNode = HypergraphValidatorNode.Create(validatorNode.Id,
                    validatorNode.Id.ConvertNodeIdToWalletHash(),
                    validatorNode.State, validatorNode.Ip);

                hypergraph.AddValidatorNode(newNode);
            }
            else
            {
                persistedNode.UpdateNodeInfo(validatorNode.State, validatorNode.Ip);
            }
        }

        // Mark nodes as offline if they are not included in validatorNodes API response
        var validatorNodeIds = validatorNodes.Select(vn => vn.Id).ToHashSet();
        foreach (var persistedNode in hypergraph.HypergraphValidatorNodes)
        {
            if (!validatorNodeIds.Contains(persistedNode.WalletId))
            {
                persistedNode.MarkAsOffline();
            }
        }

        return true;
    }
}
