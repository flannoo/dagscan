namespace DagScan.Application.Domain.ValueObjects;

public sealed class Coordinate
{
    public double Latitude { get; }
    public double Longitude { get; }

    public Coordinate(double latitude, double longitude)
    {
        if (latitude is < -90 or > 90)
            throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90.");

        if (longitude is < -180 or > 180)
            throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180.");

        Latitude = latitude;
        Longitude = longitude;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Coordinate other)
            return false;

        return Latitude == other.Latitude && Longitude == other.Longitude;
    }

    public override int GetHashCode() => HashCode.Combine(Latitude, Longitude);
}
