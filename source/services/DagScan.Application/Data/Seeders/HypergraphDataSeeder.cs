using DagScan.Application.Domain;
using DagScan.Core.Persistence;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Data.Seeders;

public sealed class HypergraphDataSeeder(
    DagContext dagContext,
    ILogger<HypergraphDataSeeder> logger)
    : IDataSeeder
{
    public int Order => 1;

    public async Task SeedAsync()
    {
        if (dagContext.Hypergraphs.Any())
        {
            return;
        }

        logger.LogInformation("Seeding Hypergraph data");

        var hypergraph = Hypergraph.Create("mainnet", "http://54.215.18.98:9000/", "https://be-mainnet.constellationnetwork.io/", true, 0, 766718);
        await dagContext.AddAsync(hypergraph);
        await dagContext.SaveChangesAsync();

        logger.LogInformation("Hypergraph data seeded");
    }
}
