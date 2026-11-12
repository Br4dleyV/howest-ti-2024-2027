namespace Adria.Domain.Models;

public class CargoReservation(
    Guid id,
    Guid userId,
    Coordinates startLocation,
    Station endStation,
    Pod pod,
    DateTime reservationTime,
    int mass,
    int width,
    int height,
    int length,
    string note
) : Reservation(id, userId, startLocation, endStation, pod, reservationTime)
{
    public int Mass { get; private set; } = mass;
    public int Width { get; private set; } = width;
    public int Height { get; private set; } = height;
    public int Length { get; private set; } = length;
    public string Note { get; private set; } = note;
}