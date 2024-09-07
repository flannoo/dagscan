using Carter;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Application.Features.GetHypergraphValidatorNodes;
using DagScan.Application.Features.GetHypergraphValidatorNodesUptime;
using MediatR;

namespace DagScan.Api.Features.Hypergraph;

public class HypergraphModule() : CarterModule("/hypergraph")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/{network}/validators", async (string network, ISender sender) =>
        {
            var request = new GetHypergraphValidatorNodesQuery(network);
            var response = await sender.Send(request);
            return Results.Ok(response);
        });

        app.MapGet("/{network}/validators/{walletAddress}/uptime",
            async (string network, string walletAddress, string? startDate, string? endDate, ISender sender) =>
            {
                var startDateValid = DateOnly.TryParse(startDate, out var fromDate);
                var endDateValid = DateOnly.TryParse(endDate, out var toDate);

                var request = new GetHypergraphValidatorNodesUptimeQuery(network, walletAddress,
                    startDateValid && endDateValid ? fromDate : null, startDateValid && endDateValid ? toDate : null);
                var response = await sender.Send(request);
                return Results.Ok(response);
            });
    }
}
