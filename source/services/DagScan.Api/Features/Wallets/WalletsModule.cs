using Carter;
using DagScan.Application.Features.GetWalletBalances;
using DagScan.Application.Features.GetWalletRewards;
using MediatR;

namespace DagScan.Api.Features.Wallets;

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

        app.MapGet("/{network}/{addresses}/rewards",
            async (string network, string addresses, string? startDate, string? endDate, ISender sender) =>
            {
                var startDateValid = DateOnly.TryParse(startDate, out var fromDate);
                var endDateValid = DateOnly.TryParse(endDate, out var toDate);

                var request = new GetWalletRewardsQuery(network, addresses,
                    startDateValid && endDateValid ? fromDate : null, startDateValid && endDateValid ? toDate : null);
                var response = await sender.Send(request);
                return Results.Ok(response);
            });
    }
}
