using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using DagScan.Application.Data;
using DagScan.Application.Domain;
using Microsoft.EntityFrameworkCore;

namespace DagScan.Application.Features.UpdateValidatorNodeLocations;

public sealed class UpdateHypergraphValidatorNodeLocationJob(
    DagContext dagContext,
    IHttpClientFactory httpClientFactory)
{
    public async Task Execute(HypergraphId hypergraphId, HypergraphValidatorNodeId hypergraphValidatorNodeId)
    {
        var validatorNode = await dagContext.HypergraphValidatorNodes.FirstOrDefaultAsync(x =>
            x.Id == hypergraphValidatorNodeId && x.HypergraphId == hypergraphId);

        if (validatorNode == null)
        {
            throw new Exception($"Hypergraph validator node '{hypergraphValidatorNodeId.Value}' could not be found");
        }

        using var httpClient = httpClientFactory.CreateClient();
        // TODO: make it configurable and use free version if api key not found
        httpClient.BaseAddress = new Uri("http://pro.ip-api.com");
        var apiKey = Environment.GetEnvironmentVariable("APIKEY_IPAPI");
        var location = await httpClient.GetFromJsonAsync<IpGeolocationResponse>(
            $"json/{validatorNode.IpAddress}?key={apiKey}",
            new JsonSerializerOptions(JsonSerializerDefaults.Web));

        if (location == null)
        {
            throw new Exception($"Failed to retrieve geolocation for IP address {validatorNode.IpAddress}");
        }

        validatorNode.UpdateProviderInfo(location.Isp, location.Country, location.City, location.Lat,
            location.Lon);

        await dagContext.SaveChangesAsync();
    }
}
