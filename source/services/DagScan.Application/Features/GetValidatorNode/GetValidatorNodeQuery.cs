using DagScan.Application.Data;
using DagScan.Application.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DagScan.Application.Features.GetValidatorNode;

public sealed record GetValidatorNodeQuery(string HypergraphName, string WalletAddress)
    : IRequest<ValidatorNodeDto?>;

internal sealed class GetValidatorNodeQueryHandler(ReadOnlyDagContext dagContext)
    : IRequestHandler<GetValidatorNodeQuery, ValidatorNodeDto?>
{
    public async Task<ValidatorNodeDto?> Handle(GetValidatorNodeQuery request, CancellationToken cancellationToken)
    {
        var hypergraph = await dagContext.Hypergraphs.FirstOrDefaultAsync(x => x.Name == request.HypergraphName,
            cancellationToken: cancellationToken);

        if (hypergraph is null)
        {
            return null;
        }

        var metagraphs = await dagContext.Metagraphs
            .Where(x => x.HypergraphId == hypergraph.Id)
            .ToListAsync(cancellationToken: cancellationToken);
        var metagraphIds = metagraphs.Select(metagraph => metagraph.Id).ToList();

        var hypergraphValidatorNode = await dagContext.HypergraphValidatorNodes
            .Where(x =>
                x.HypergraphId == hypergraph.Id &&
                x.WalletAddress == new WalletAddress(request.WalletAddress))
            .Select(x => new HypergraphValidatorNodeDto(
                    x.WalletAddress.Value,
                    x.WalletId.Value,
                    x.IpAddress,
                    x.NodeStatus,
                    x.IsInConsensus,
                    x.ServiceProvider,
                    x.Country,
                    x.City,
                    x.Coordinates != null ? x.Coordinates.Latitude : null,
                    x.Coordinates != null ? x.Coordinates.Longitude : null
                )
            ).FirstOrDefaultAsync(cancellationToken);

        var persistedMetagraphValidatorNodes = await dagContext.MetagraphValidatorNodes
            .Where(x =>
                metagraphIds.Contains(x.MetagraphId) &&
                x.WalletAddress == new WalletAddress(request.WalletAddress))
            .ToListAsync(cancellationToken);

        var metagraphValidatorNodes = persistedMetagraphValidatorNodes.Select(x =>
        {
            var metagraph = metagraphs.FirstOrDefault(metagraph => metagraph.Id == x.MetagraphId);
            var metagraphAddress = metagraph?.MetagraphAddress?.Value;

            return new MetagraphValidatorNodeDto(
                x.WalletAddress.Value,
                x.WalletId.Value,
                metagraphAddress,
                x.MetagraphType.ToString(),
                x.IpAddress,
                x.NodeStatus,
                x.ServiceProvider,
                x.Country,
                x.City,
                x.Coordinates?.Latitude,
                x.Coordinates?.Longitude
            );
        }).ToList();

        if (hypergraphValidatorNode is null && metagraphValidatorNodes.Count == 0)
        {
            return null;
        }

        var validatorResult = new ValidatorNodeDto()
        {
            HypergraphValidatorNodeDto = hypergraphValidatorNode, MetagraphNodes = metagraphValidatorNodes.ToList()
        };

        return validatorResult;
    }
}
