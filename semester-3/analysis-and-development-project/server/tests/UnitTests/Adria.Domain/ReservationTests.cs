using Adria.Domain.Models;

namespace UnitTests.Adria.Domain;

public sealed class ReservationTests
{
    [Fact]
    public void Constructor_WithAllParameters_CreatesReservation()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var start = new Coordinates(50.0, 3.0);
        var end = new Station(Guid.NewGuid(), "Station", new Coordinates(53.2, 5.7));
        var pod = new Pod(Guid.NewGuid(), "Pod", true, 3, 10, new Coordinates(0, 0));
        var reservationTime = DateTime.Today.AddHours(8);

        // Act
        var reservation = new Reservation(id, userId, start, end, pod, reservationTime);

        // Assert
        Assert.Equal(id, reservation.Id);
        Assert.Equal(userId, reservation.UserId);
        Assert.Equal(start, reservation.StartLocation);
        Assert.Equal(end, reservation.EndStation);
        Assert.Equal(pod, reservation.Pod);
        Assert.Equal(reservationTime, reservation.ReservationTime);
    }
}