using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using DagScan.Application.Data;
using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DagScan.Application.Features.UpdateValidatorNodeLocations;

public sealed class UpdateHypergraphValidatorNodeLocationJob(
    DagContext dagContext,
    IHttpClientFactory httpClientFactory)
{
    public async Task Execute(HypergraphValidatorNodeId hypergraphValidatorNodeId)
    {
        var validatorNode = await dagContext.HypergraphValidatorNodes.FirstOrDefaultAsync(x =>
            x.Id == hypergraphValidatorNodeId);

        if (validatorNode == null)
        {
            throw new Exception($"Hypergraph validator node '{hypergraphValidatorNodeId.Value}' could not be found");
        }

        using var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("IPAPI_URL") ??
                                         throw new Exception("IP API URL environment variable is not set"));
        var apiKey = Environment.GetEnvironmentVariable("IPAPI_KEY");
        var location = await httpClient.GetFromJsonAsync<IpGeolocationResponse>(
            $"json/{validatorNode.IpAddress}?key={apiKey}",
            new JsonSerializerOptions(JsonSerializerDefaults.Web));

        if (location == null)
        {
            throw new Exception($"Failed to retrieve geolocation for IP address {validatorNode.IpAddress}");
        }

        validatorNode.UpdateServiceProviderInfo(location.Isp, location.Country, location.City,
            new Coordinate(location.Lat, location.Lon));

        await dagContext.SaveChangesAsync();
    }
}
