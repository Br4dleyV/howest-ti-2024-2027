using Adria.Domain.Shared;

namespace Adria.Domain.Models;

public class Pod(Guid id, string name, bool isActive, int passengers, int maxPassengers, Coordinates currentLocation)
{
    private const double SPEED_KILOMETERS_PER_HOUR = 50;

    public Guid Id { get; private set; } = id;
    public string Name { get; private set; } = name;
    public bool IsActive { get; private set; } = isActive;
    public int Passengers { get; private set; } = passengers;
    public int MaxPassengers { get; private set; } = maxPassengers;
    public Coordinates CurrentLocation { get; set; } = currentLocation;

    public double GetDistanceToPod(Coordinates location)
    {
        return Utils.GetDistanceToLocation(location, CurrentLocation);
    }

    public DateTime GetEtaToStation(Coordinates location, Station station)
    {
        double distance = GetDistanceToPod(location) + GetDistanceToPod(station.Coordinates);

        double hours = distance / SPEED_KILOMETERS_PER_HOUR;
        
        return DateTime.Now.AddHours(hours);
    }
}