using Carter;
using DagScan.Application.Features.GetMetagraphs;
using DagScan.Application.Features.GetMetagraphValidatorNodes;
using MediatR;

namespace DagScan.Api.Features.Metagraph;

public class MetagraphModule() : CarterModule("/metagraphs")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/{network}", async (string network, ISender sender) =>
        {
            var request = new GetMetagraphsQuery(network);
            var response = await sender.Send(request);
            return Results.Ok(response);
        });

        app.MapGet("/{network}/validators",
            async (string network, string? metagraphAddress, ISender sender) =>
            {
                var request = new GetMetagraphValidatorNodesQuery(network, metagraphAddress);
                var response = await sender.Send(request);
                return Results.Ok(response);
            });
    }
}
