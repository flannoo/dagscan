using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Data.Seeders;

public sealed class MetagraphDataSeeder(
    DagContext dagContext,
    ILogger<MetagraphDataSeeder> logger)
    : IDataSeeder
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

        var dagMetagraph = Metagraph.Create(hypergraph.Id, null, "DAG", "DAG", null, true, 0);
        dagMetagraph.AddMetagraphEndpoint(new MetagraphEndpoint("http://54.215.18.98:9010/",
            MetagraphTypes.DagL1));

        var dorMetagraph = Metagraph.Create(hypergraph.Id,
            new MetagraphAddress("DAG0CyySf35ftDQDQBnd1bdQ9aPyUdacMghpnCuM"), "DOR", "DOR",
            new WalletAddress("DAG0o6WSyvc7XfzujwJB1e25mfyzgXoLYDD6wqnk"), true, 0);

        dorMetagraph.AddMetagraphEndpoint(new MetagraphEndpoint("http://dor-metagraph-mainnet-l0-1396659502.us-west-2.elb.amazonaws.com:7000",
            MetagraphTypes.MetagraphL0));
        dorMetagraph.AddMetagraphEndpoint(new MetagraphEndpoint("http://dor-metagraph-mainnet-cl1-1843193940.us-west-2.elb.amazonaws.com:8000",
            MetagraphTypes.MetagraphCurrencyL1));
        dorMetagraph.AddMetagraphEndpoint(new MetagraphEndpoint("http://dor-metagraph-mainnet-568250314.us-west-2.elb.amazonaws.com:9000",
            MetagraphTypes.MetagraphDataL1));

        var elPacaMetagraph = Metagraph.Create(hypergraph.Id,
            new MetagraphAddress("DAG7ChnhUF7uKgn8tXy45aj4zn9AFuhaZr8VXY43"), "El Paca", "PACA",
            new WalletAddress("DAG5VxUBiDx24wZgBwjJ1FeuVP1HHVjz6EzXa3z6"), true, 0);

        elPacaMetagraph.AddMetagraphEndpoint(new MetagraphEndpoint("http://elpaca-l0-2006678808.us-west-1.elb.amazonaws.com:9100",
            MetagraphTypes.MetagraphL0));
        elPacaMetagraph.AddMetagraphEndpoint(new MetagraphEndpoint("http://elpaca-cl1-1512652691.us-west-1.elb.amazonaws.com:9200",
            MetagraphTypes.MetagraphCurrencyL1));
        elPacaMetagraph.AddMetagraphEndpoint(new MetagraphEndpoint("http://elpaca-dl1-550039959.us-west-1.elb.amazonaws.com:9300",
            MetagraphTypes.MetagraphDataL1));

        dagContext.Metagraphs.Add(dagMetagraph);
        dagContext.Metagraphs.Add(dorMetagraph);
        dagContext.Metagraphs.Add(elPacaMetagraph);

        await dagContext.SaveChangesAsync();

        logger.LogInformation("Metagraph data seeded");
    }
}
