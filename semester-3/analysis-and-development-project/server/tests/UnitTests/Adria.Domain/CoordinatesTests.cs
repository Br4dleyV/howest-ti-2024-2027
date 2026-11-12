using Adria.Domain.Models;

namespace UnitTests.Adria.Domain;

public sealed class CoordinatesTests
{
    [Fact]
    public void Constructor_WithLatitudeAndLongitude_CreatesCoordinates()
    {
        // Arrange
        double lat = 51.123;
        double lon = 3.456;

        // Act
        var coords = new Coordinates(lat, lon);

        // Assert
        Assert.Equal(lat, coords.Latitude);
        Assert.Equal(lon, coords.Longitude);
    }
}