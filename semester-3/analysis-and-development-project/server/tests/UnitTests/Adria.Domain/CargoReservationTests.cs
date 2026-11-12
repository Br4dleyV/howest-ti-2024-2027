using Adria.Domain.Models;

namespace UnitTests.Adria.Domain;

public sealed class CargoReservationTests
{
    [Fact]
    public void Constructor_WithAllParameters_CreatesCargoReservation()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var start = new Coordinates(50.0, 3.0);
        var end = new Station(Guid.NewGuid(), "Station", new Coordinates(53.2, 5.7));
        var pod = new Pod(Guid.NewGuid(), "Pod", true, 3, 10, new Coordinates(0, 0));
        var reservationTime = DateTime.Today.AddHours(8);

        var mass = 50;
        var width = 20;
        var height = 30;
        var length = 40;
        var note = "Handle with care";

        // Act
        var cargo = new CargoReservation(
            id,
            userId,
            start,
            end,
            pod,
            reservationTime,
            mass,
            width,
            height,
            length,
            note
        );

        // Assert
        Assert.Equal(id, cargo.Id);
        Assert.Equal(userId, cargo.UserId);
        Assert.Equal(start, cargo.StartLocation);
        Assert.Equal(end, cargo.EndStation);
        Assert.Equal(pod, cargo.Pod);
        Assert.Equal(reservationTime, cargo.ReservationTime);

        Assert.Equal(mass, cargo.Mass);
        Assert.Equal(width, cargo.Width);
        Assert.Equal(height, cargo.Height);
        Assert.Equal(length, cargo.Length);
        Assert.Equal(note, cargo.Note);
    }
}