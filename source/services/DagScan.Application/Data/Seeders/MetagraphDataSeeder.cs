using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Data.Seeders;

public sealed class MetagraphDataSeeder(
    DagContext dagContext,
    ILogger<MetagraphDataSeeder> logger)
    : IRequiredDataSeeder
{
    public int Order => 2;

    public async Task SeedAsync()
    {
        if (dagContext.Metagraphs.Any())
        {
            return;
        }

        logger.LogInformation("Seeding Metagraph data");

        var hypergraph = await dagContext.Hypergraphs.FirstOrDefaultAsync(h => h.Name == "mainnet");

        if (hypergraph == null)
        {
            return;
        }

        var dagMetagraph = Metagraph.Create(hypergraph.Id, null, "DAG", "DAG", null, true);
        dagMetagraph.AddMetagraphEndpoint(new MetagraphEndpoint("https://l1-lb-mainnet.constellationnetwork.io/",
            MetagraphTypes.DagL1));

        var dorMetagraph = Metagraph.Create(hypergraph.Id,
            new MetagraphAddress("DAG0CyySf35ftDQDQBnd1bdQ9aPyUdacMghpnCuM"), "DOR", "DOR",
            new WalletAddress("DAG0o6WSyvc7XfzujwJB1e25mfyzgXoLYDD6wqnk"), false);
        var elPacaMetagraph = Metagraph.Create(hypergraph.Id,
            new MetagraphAddress("DAG7ChnhUF7uKgn8tXy45aj4zn9AFuhaZr8VXY43"), "El Paca", "PACA",
            new WalletAddress("DAG5VxUBiDx24wZgBwjJ1FeuVP1HHVjz6EzXa3z6"), false);

        dagContext.Metagraphs.Add(dagMetagraph);
        dagContext.Metagraphs.Add(dorMetagraph);
        dagContext.Metagraphs.Add(elPacaMetagraph);

        await dagContext.SaveChangesAsync();

        logger.LogInformation("Metagraph data seeded");
    }
}
