using Adria.Application.Contracts.Data;
using Adria.Domain.Models;

namespace UnitTests.Adria.Application.Dtos;

public sealed class PodDtoTests
{
    [Fact]
    public void Constructor_WithAllParameters_CreatesPodDto()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Pod Test";
        var isActive = true;
        var capacity = 8;
        var maxCapacity = 20;
        var location = new CoordinatesDto(51.0, 3.0);

        // Act
        var dto = new PodDto(id, name, isActive, capacity, maxCapacity, location);

        // Assert
        Assert.Equal(id, dto.Id);
        Assert.Equal(name, dto.Name);
        Assert.Equal(isActive, dto.IsActive);
        Assert.Equal(capacity, dto.Capacity);
        Assert.Equal(maxCapacity, dto.MaxCapacity);
        Assert.Equal(location, dto.CurrentLocation);
    }

    [Fact]
    public void Constructor_FromPod_CreatesEquivalentPodDto()
    {
        // Arrange
        var pod = new Pod(
            Guid.NewGuid(),
            "Pod X",
            true,
            5,
            15,
            new Coordinates(51.2, 3.3)
        );

        // Act
        var dto = new PodDto(pod);

        // Assert
        Assert.Equal(pod.Id, dto.Id);
        Assert.Equal(pod.Name, dto.Name);
        Assert.Equal(pod.IsActive, dto.IsActive);
        Assert.Equal(pod.Passengers, dto.Capacity);
        Assert.Equal(pod.MaxPassengers, dto.MaxCapacity);
        Assert.Equal(pod.CurrentLocation.Latitude, dto.CurrentLocation.Latitude);
        Assert.Equal(pod.CurrentLocation.Longitude, dto.CurrentLocation.Longitude);
    }

    [Fact]
    public void ToPod_ConvertsDtoToPodCorrectly()
    {
        // Arrange
        var dto = new PodDto(
            Guid.NewGuid(),
            "Pod Y",
            true,
            7,
            14,
            new CoordinatesDto(51.5, 3.8)
        );

        // Act
        var pod = dto.ToPod();

        // Assert
        Assert.Equal(dto.Id, pod.Id);
        Assert.Equal(dto.Name, pod.Name);
        Assert.Equal(dto.IsActive, pod.IsActive);
        Assert.Equal(dto.Capacity, pod.Passengers);
        Assert.Equal(dto.MaxCapacity, pod.MaxPassengers);
        Assert.Equal(dto.CurrentLocation.Latitude, pod.CurrentLocation.Latitude);
        Assert.Equal(dto.CurrentLocation.Longitude, pod.CurrentLocation.Longitude);
    }
}
