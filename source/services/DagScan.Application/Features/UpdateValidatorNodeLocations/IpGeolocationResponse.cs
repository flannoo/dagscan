namespace DagScan.Application.Features.UpdateValidatorNodeLocations;

public sealed record IpGeolocationResponse(string Isp, string Country, string City, double Lon, double Lat);
