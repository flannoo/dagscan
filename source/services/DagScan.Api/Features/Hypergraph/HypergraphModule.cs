using Carter;
using DagScan.Application.Features.GetHypergraphValidatorNodes;
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
    }
}
