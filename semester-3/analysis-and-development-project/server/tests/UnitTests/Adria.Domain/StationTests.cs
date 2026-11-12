using Adria.Domain.Models;

namespace UnitTests.Adria.Domain;

public sealed class StationTests
{
    [Fact]
    public void Constructor_WithAllParameters_CreatesStation()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Central Station";
        var location = new Coordinates(51.05, 3.72);

        // Act
        var station = new Station(id, name, location);

        // Assert
        Assert.Equal(id, station.Id);
        Assert.Equal(name, station.Name);
        Assert.Equal(location, station.Coordinates);
    }
}