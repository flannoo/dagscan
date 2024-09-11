using System.Net.Http.Headers;
using DagScan.Application.Data;
using DagScan.Application.Domain;
using DagScan.Core.Scheduling;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace DagScan.Application.Features.SyncCurrencyPrices;

[AutomaticRetry(Attempts = 0)]
public sealed class SyncCurrencyPricesJob(
    DagContext dagContext,
    IHttpClientFactory httpClientFactory,
    ILogger<SyncCurrencyPricesJob> logger) : IJob
{
    public string Schedule => Cron.Hourly();

    public async Task Execute()
    {
        logger.LogInformation("Start {JobName} Execution", nameof(SyncCurrencyPricesJob));

        var metagraphs = await dagContext.Metagraphs.Where(x => !string.IsNullOrEmpty(x.CoingeckoId)).ToListAsync();

        var coingeckoBaseAddress =
            "https://api.coingecko.com"; //"api/v3/simple/price?ids=constellation-labs&vs_currencies=usd";

        using var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(coingeckoBaseAddress);
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);

        await using var transaction = await dagContext.Database.BeginTransactionAsync();

        try
        {
            foreach (var metagraph in metagraphs)
            {
                var currencyUrl = $"/api/v3/simple/price?ids={metagraph.CoingeckoId!}&vs_currencies=usd";

                var response = await httpClient.GetAsync(currencyUrl);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);
                var usdPrice = json[metagraph.CoingeckoId!]?["usd"]?.Value<decimal>() ?? 0;

                var currencyPrice = await dagContext.CurrencyPrices
                    .Where(x => x.MetagraphAddress == metagraph.MetagraphAddress && x.Date == currentDate)
                    .FirstOrDefaultAsync();

                if (currencyPrice == null)
                {
                    currencyPrice = CurrencyPrice.Create(metagraph.MetagraphAddress, usdPrice, currentDate);
                    dagContext.CurrencyPrices.Add(currencyPrice);
                    continue;
                }

                currencyPrice.UpdatePrice(usdPrice);
            }

            await dagContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }

        logger.LogInformation("Completed {JobName} Execution", nameof(SyncCurrencyPricesJob));
    }
}
