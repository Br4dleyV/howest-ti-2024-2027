using Adria.Domain.Models;

namespace Adria.Application.Contracts.Data;

public record PodDto(
    Guid Id,
    string Name,
    bool IsActive,
    int Capacity,
    int MaxCapacity,
    CoordinatesDto CurrentLocation
)
{
    public PodDto(Pod pod) : this(pod.Id, pod.Name, pod.IsActive, pod.Passengers, pod.MaxPassengers, new CoordinatesDto(pod.CurrentLocation.Latitude, pod.CurrentLocation.Longitude)) {}

    public Pod ToPod()
    {
        return new Pod(
            Id,
            Name,
            IsActive,
            Capacity,
            MaxCapacity,
            new Coordinates(CurrentLocation.Latitude, CurrentLocation.Longitude)
        );
    }
}