using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Data.Seeders;

public sealed class RewardTransactionConfigDataSeeder(
    DagContext dagContext,
    ILogger<RewardTransactionConfigDataSeeder> logger) : IDataSeeder
{
    public int Order => 3;

    public async Task SeedAsync()
    {
        if (dagContext.RewardTransactionConfigs.Any())
        {
            return;
        }

        logger.LogInformation("Seeding RewardTransactionConfig data");

        var hypergraph = await dagContext.Hypergraphs.FirstOrDefaultAsync(h => h.Name == "mainnet");

        if (hypergraph == null)
        {
            return;
        }

        var metagraphs = await dagContext.Metagraphs.Where(x => x.HypergraphId == hypergraph.Id).ToListAsync();

        await dagContext.RewardTransactionConfigs.AddRangeAsync(new List<RewardTransactionConfig>()
        {
            RewardTransactionConfig.Create(
                "Testnet",
                metagraphs.First(x => x.HypergraphId == hypergraph.Id && x.MetagraphAddress == null).Id,
                null,
                new WalletAddress("DAG2hdmjMKARKjeCrwG6StZENRvKU1KkwqxpcJ4K"),
                null,
                null,
                true
            ),
            RewardTransactionConfig.Create(
                "Integrationnet",
                metagraphs.First(x => x.HypergraphId == hypergraph.Id && x.MetagraphAddress == null).Id,
                null,
                new WalletAddress("DAG5LEmVdtMGryBs1UBLMfg37LoSiVHC8Q4odps9"),
                null,
                null,
                true
            ),
            RewardTransactionConfig.Create(
                "DTM",
                metagraphs.First(x => x.HypergraphId == hypergraph.Id && x.MetagraphAddress == null).Id,
                null,
                new WalletAddress("DAG5hKjytwENhcx2vmC5qWyWSgnqWmYq7cUvGLDG"),
                null,
                null,
                false
            ),
            RewardTransactionConfig.Create(
                "DTM",
                metagraphs.First(x => x.HypergraphId == hypergraph.Id && x.MetagraphAddress == null).Id,
                null,
                new WalletAddress("DAG8BSw4tGn269vbF9wor4FGd4yt9XGdJuiMb6uS"),
                null,
                null,
                false
            ),
            RewardTransactionConfig.Create(
                "Softnode",
                metagraphs.First(x => x.HypergraphId == hypergraph.Id && x.MetagraphAddress == null).Id,
                null,
                new WalletAddress("DAG6stSuh8C46VVHvFybyCjdC4UPu42xXq2Si9Z3"),
                null,
                null,
                true
            ),
            RewardTransactionConfig.Create(
                "Softnode",
                metagraphs.First(x => x.HypergraphId == hypergraph.Id && x.MetagraphAddress == null).Id,
                null,
                new WalletAddress("DAG77VVVRvdZiYxZ2hCtkHz68h85ApT5b2xzdTkn"),
                new WalletAddress("DAG4nBMH7KwFAnpfB6VaFRaKEwACNSidzeMuPtgV"),
                null,
                true
            ),
            RewardTransactionConfig.Create(
                "DTM",
                metagraphs.First(x => x.HypergraphId == hypergraph.Id && x.MetagraphAddress! ==
                    new MetagraphAddress("DAG0CyySf35ftDQDQBnd1bdQ9aPyUdacMghpnCuM")).Id,
                new MetagraphAddress("DAG0CyySf35ftDQDQBnd1bdQ9aPyUdacMghpnCuM"),
                new WalletAddress("DAG0U7R9jXMSiNMU5mgqpvCVuaBwfRBzY77nJZM1"),
                null,
                null,
                false
            ),
            RewardTransactionConfig.Create(
                "DOR Validator Tax",
                metagraphs.First(x => x.HypergraphId == hypergraph.Id && x.MetagraphAddress! ==
                    new MetagraphAddress("DAG0CyySf35ftDQDQBnd1bdQ9aPyUdacMghpnCuM")).Id,
                new MetagraphAddress("DAG0CyySf35ftDQDQBnd1bdQ9aPyUdacMghpnCuM"),
                new WalletAddress("DAG2JsH1QKj8LrzmcgX2pf9MAcdhQWuihYnZMUNW"),
                null,
                null,
                true
            ),
            RewardTransactionConfig.Create(
                "PylonFi DOR Rewards",
                metagraphs.First(x => x.HypergraphId == hypergraph.Id && x.MetagraphAddress! ==
                    new MetagraphAddress("DAG0CyySf35ftDQDQBnd1bdQ9aPyUdacMghpnCuM")).Id,
                new MetagraphAddress("DAG0CyySf35ftDQDQBnd1bdQ9aPyUdacMghpnCuM"),
                new WalletAddress("DAG4QoRicdQrSjgRkYqrkzrxoD1WgFianuATZac3"),
                null,
                null,
                true
            ),
            RewardTransactionConfig.Create(
                "PylonFi DOR Rewards",
                metagraphs.First(x => x.HypergraphId == hypergraph.Id && x.MetagraphAddress! ==
                    new MetagraphAddress("DAG0CyySf35ftDQDQBnd1bdQ9aPyUdacMghpnCuM")).Id,
                new MetagraphAddress("DAG0CyySf35ftDQDQBnd1bdQ9aPyUdacMghpnCuM"),
                new WalletAddress("DAG4FjXSpyVbBRFw4uDxSekZj3fTxmg87axJwKpx"),
                null,
                null,
                true
            )
        });

        await dagContext.SaveChangesAsync();

        logger.LogInformation("RewardTransactionConfig data seeded");
    }
}
