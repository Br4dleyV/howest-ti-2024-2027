using Adria.Application.Contracts.Data;

namespace UnitTests.Adria.Application.Dtos;

public class PodWithEtaDtoTests
{
    [Fact]
    public void Constructor_WithAllParameters_CreatesPodWithEtaDto()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Pod Test";
        var isActive = true;
        var capacity = 8;
        var maxCapacity = 20;
        var location = new CoordinatesDto(51.0, 3.0);
        var eta = DateTime.Today.AddMinutes(30);

        // Act
        var dto = new PodWithEtaDto(id, name, isActive, capacity, maxCapacity, location, eta);

        // Assert
        Assert.Equal(id, dto.Id);
        Assert.Equal(name, dto.Name);
        Assert.Equal(isActive, dto.IsActive);
        Assert.Equal(capacity, dto.Capacity);
        Assert.Equal(maxCapacity, dto.MaxCapacity);
        Assert.Equal(location, dto.CurrentLocation);
        Assert.Equal(eta, dto.Eta);
    }
}