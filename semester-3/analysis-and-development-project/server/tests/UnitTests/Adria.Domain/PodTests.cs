using Adria.Domain.Models;

namespace UnitTests.Adria.Domain;

public sealed class PodTests
{
    [Fact]
    public void Constructor_WithAllParameters_CreatesPod()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test pod";
        var isActive = true;
        var passengers = 15;
        var maxPassengers = 30;
        var currentLocation =  new Coordinates(0, 0);

        // Act
        var pod = new Pod(id, name, isActive, passengers, maxPassengers, currentLocation);

        // Assert
        Assert.Equal(pod.Id, id);
        Assert.Equal(name, pod.Name);
        Assert.True(pod.IsActive);
        Assert.Equal(passengers, pod.Passengers);
        Assert.Equal(maxPassengers, pod.MaxPassengers);
        Assert.Equal(currentLocation, pod.CurrentLocation);
    }

    [Fact]
    public void GetDistanceToLocation_GivesCorrectDistance()
    {
        // Arrange
        var pod = new Pod(Guid.NewGuid(), "Test Pod", true, 15, 30, new Coordinates(51.2093, 3.2247));
        var userLocation = new Coordinates(51.2089, 3.2260);
        
        // Act
        var distance = pod.GetDistanceToPod(userLocation);
        
        // Assert
        Assert.InRange(distance, 0.09, 0.11);
    }
}
