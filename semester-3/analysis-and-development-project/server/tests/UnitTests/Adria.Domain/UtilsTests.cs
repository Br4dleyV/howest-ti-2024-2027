using Adria.Domain.Models;
using Adria.Domain.Shared;
using Xunit;

namespace UnitTests.Adria.Domain.Shared;

public sealed class UtilsTests
{
    [Fact]
    public void GetDistanceToLocation_KnownCoordinates_ReturnsExpectedDistance()
    {
        // Arrange: Coordinates of Brussels and Paris
        var brussels = new Coordinates(50.8503, 4.3517);
        var paris = new Coordinates(48.8566, 2.3522);

        // Act
        double distance = Utils.GetDistanceToLocation(brussels, paris);

        // Assert: Approx distance ~264 km
        Assert.InRange(distance, 263, 265);
    }

    [Fact]
    public void GetDistanceToLocation_SameCoordinates_ReturnsZero()
    {
        // Arrange
        var location = new Coordinates(51.0, 3.0);

        // Act
        double distance = Utils.GetDistanceToLocation(location, location);

        // Assert
        Assert.Equal(0, distance, 5); // Allow very small floating point epsilon
    }

    [Fact]
    public void RandomInt_GeneratesValueWithinRange()
    {
        // Arrange
        int min = 10;
        int max = 20;

        // Act
        int value = Utils.RandomInt(min, max);

        // Assert
        Assert.InRange(value, min, max - 1);
    }

    [Fact]
    public void RandomDouble_GeneratesValueWithinRange()
    {
        // Arrange
        int min = 50;
        int max = 200;

        // Act
        double value = Utils.RandomDouble(min, max);

        // Assert
        Assert.InRange(value, min, max);
    }

    [Fact]
    public void RandomInt_MultipleCalls_ReturnDifferentValues()
    {
        // Arrange
        int min = 0;
        int max = 100;

        // Act
        var values = new HashSet<int>();
        for (int i = 0; i < 100; i++)
        {
            values.Add(Utils.RandomInt(min, max));
        }

        // Assert
        Assert.True(values.Count > 1, "RandomInt should generate multiple different values.");
    }

    [Fact]
    public void RandomDouble_MultipleCalls_ReturnDifferentValues()
    {
        // Arrange
        int min = 0;
        int max = 100;

        // Act
        var values = new HashSet<double>();
        for (int i = 0; i < 100; i++)
        {
            values.Add(Utils.RandomDouble(min, max));
        }

        // Assert
        Assert.True(values.Count > 1, "RandomDouble should generate multiple different values.");
    }
}
