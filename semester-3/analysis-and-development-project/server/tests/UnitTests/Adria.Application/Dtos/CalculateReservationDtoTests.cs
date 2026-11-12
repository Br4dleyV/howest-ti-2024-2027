using Adria.Application.Contracts.Data;

namespace UnitTests.Adria.Application.Dtos;

public sealed class CalculateReservationDtoTests
{
    [Fact]
    public void Constructor_WithAllParameters_CreatesDto()
    {
        // Arrange
        var distance = 12.5;
        var pods = new List<PodWithEtaDto>
        {
            new PodWithEtaDto(
                Guid.NewGuid(),
                "Pod A",
                true,
                10,
                20,
                new CoordinatesDto(51.0, 3.0),
                DateTime.Today.AddHours(1)
            ),
            new PodWithEtaDto(
                Guid.NewGuid(),
                "Pod B",
                false,
                5,
                20,
                new CoordinatesDto(51.1, 3.1),
                DateTime.Today.AddHours(2)
            )
        }.AsReadOnly();

        // Act
        var dto = new CalculateReservationDto(distance, pods);

        // Assert
        Assert.Equal(distance, dto.distance);
        Assert.Equal(pods, dto.pods);
    }
}