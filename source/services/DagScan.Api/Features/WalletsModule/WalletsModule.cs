using Carter;
using DagScan.Application.Features.GetWalletBalances;
using MediatR;

namespace DagScan.Api.Features.WalletsModule;

public class WalletsModule() : CarterModule("/wallets")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/{network}/{address}", async (string network, string address, ISender sender) =>
        {
            var request = new GetWalletBalancesQuery(network, address);
            var response = await sender.Send(request);
            return Results.Ok(response);
        });
    }
}
