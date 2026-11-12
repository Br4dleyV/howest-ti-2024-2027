namespace Adria.Domain.Models;

public record Coordinates(double Latitude, double Longitude)
{
    public override string ToString()
    {
        return $"({Latitude}, {Longitude})";
    }
}