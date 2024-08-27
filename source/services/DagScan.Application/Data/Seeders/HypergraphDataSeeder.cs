using DagScan.Application.Domain;
using DagScan.Core.Persistence;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Data.Seeders;

public sealed class HypergraphDataSeeder(
    DagContext dagContext,
    ILogger<HypergraphDataSeeder> logger)
    : IRequiredDataSeeder
{
    public int Order => 1;

    public async Task SeedAsync()
    {
        if (dagContext.Hypergraphs.Any())
        {
            return;
        }

        logger.LogInformation("Seeding Hypergraph data");

        var hypergraph = Hypergraph.Create("mainnet", "https://l0-lb-mainnet.constellationnetwork.io/", "https://be-mainnet.constellationnetwork.io/", true);
        await dagContext.AddAsync(hypergraph);
        await dagContext.SaveChangesAsync();

        logger.LogInformation("Hypergraph data seeded");
    }
}
