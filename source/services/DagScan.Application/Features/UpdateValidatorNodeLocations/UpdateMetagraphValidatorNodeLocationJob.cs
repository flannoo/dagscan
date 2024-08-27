using System.Net.Http.Json;
using System.Text.Json;
using DagScan.Application.Data;
using DagScan.Application.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DagScan.Application.Features.UpdateValidatorNodeLocations;

public sealed class UpdateMetagraphValidatorNodeLocationJob(
    DagContext dagContext,
    IHttpClientFactory httpClientFactory)
{
    public async Task Execute(MetagraphValidatorNodeId metagraphValidatorNodeId)
    {
        var validatorNode = await dagContext.MetagraphValidatorNodes.FirstOrDefaultAsync(x =>
            x.Id == metagraphValidatorNodeId);

        if (validatorNode == null)
        {
            throw new Exception($"Metagraph validator node '{metagraphValidatorNodeId.Value}' could not be found");
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

        validatorNode.UpdateServiceProviderInfo(location.Isp, location.Country, location.City,
            new Coordinate(location.Lat, location.Lon));

        await dagContext.SaveChangesAsync();
    }
}
