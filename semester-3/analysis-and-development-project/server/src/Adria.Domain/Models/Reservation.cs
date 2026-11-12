namespace Adria.Domain.Models;

public class Reservation(
    Guid id,
    Guid userId,
    Coordinates startLocation,
    Station endStation,
    Pod pod,
    DateTime reservationTime
)
{
    public Guid Id { get; private set; } = id;
    public Guid UserId { get; private set; } = userId;
    public Coordinates StartLocation { get; private set; } = startLocation;
    public Station EndStation { get; private set; } = endStation;
    public Pod Pod { get; private set; } = pod;
    public DateTime ReservationTime { get; private set; } = reservationTime;

    public Reservation(Guid userId, Coordinates startLocation, Station endStation, Pod pod) : this(Guid.NewGuid(),
        userId, startLocation, endStation, pod, DateTime.Now) {}
}