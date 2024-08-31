using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using DagScan.Application.Data;
using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.CQRS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DagScan.Application.Features.SyncMetagraphValidatorNodes;

public sealed record UpsertMetagraphValidatorNodesCommand : ICommand<bool>
{
    public Guid MetagraphId { get; init; }
}

public sealed class UpsertMetagraphValidatorNodesCommandHandler(
    DagContext dagContext,
    IHttpClientFactory httpClientFactory)
    : IRequestHandler<UpsertMetagraphValidatorNodesCommand, bool>
{
    public async Task<bool> Handle(UpsertMetagraphValidatorNodesCommand request, CancellationToken cancellationToken)
    {
        using var httpClient = httpClientFactory.CreateClient();

        var metagraph = await dagContext.Metagraphs
            .FirstOrDefaultAsync(x => x.Id == new MetagraphId(request.MetagraphId), cancellationToken);

        if (metagraph is null)
        {
            return false;
        }

        foreach (var metagraphEndpoint in metagraph.MetagraphEndpoints)
        {
            httpClient.BaseAddress = new Uri(metagraphEndpoint.ApiBaseAddress);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var validatorNodes = await httpClient.GetFromJsonAsync<MetagraphClusterInfo[]>("cluster/info",
                new JsonSerializerOptions(JsonSerializerDefaults.Web), cancellationToken: cancellationToken) ?? [];

            if (validatorNodes.Length == 0)
            {
                continue;
            }

            var metagraphNodes =
                await dagContext.MetagraphValidatorNodes
                    .Where(x => x.MetagraphId == metagraph.Id && x.MetagraphType == metagraphEndpoint.MetagraphType)
                    .ToListAsync(cancellationToken: cancellationToken);

            foreach (var validatorNode in validatorNodes.ToList())
            {
                var persistedNode = metagraphNodes.FirstOrDefault(x => x.WalletId.Value == validatorNode.Id);
                if (persistedNode is null)
                {
                    var newNode = MetagraphValidatorNode.Create(metagraph.HypergraphId, metagraph.Id,
                        metagraphEndpoint.MetagraphType, new WalletId(validatorNode.Id),
                        WalletAddress.CreateFromWalletId(validatorNode.Id), validatorNode.State, validatorNode.Ip);

                    await dagContext.MetagraphValidatorNodes.AddAsync(newNode, cancellationToken);
                    newNode.Created();
                }
                else
                {
                    if (persistedNode.NodeStatus != validatorNode.State)
                    {
                        persistedNode.UpdateNodeStatus(validatorNode.State);
                    }

                    if (persistedNode.IpAddress != validatorNode.Ip)
                    {
                        persistedNode.UpdateNodeIpAddress(validatorNode.Ip);
                    }
                }
            }

            // Mark nodes as offline if they are not included in validatorNodes API response and not already marked as offline
            foreach (var persistedNode in metagraphNodes.Where(x => x.NodeStatus != "Offline"))
            {
                if (!metagraphNodes.Select(x => x.WalletId).Contains(persistedNode.WalletId))
                {
                    persistedNode.UpdateNodeStatus("Offline");
                }
            }
        }

        return true;
    }
}
