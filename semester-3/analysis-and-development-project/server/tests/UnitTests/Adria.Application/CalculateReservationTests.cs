using Adria.Application.Actions;
using Adria.Application.Contracts.Data;
using Adria.Domain.Models;
using Adria.Domain.Shared.Exceptions;
using UnitTests.Mocks;
using UnitTests.Mocks.Queries;
using UnitTests.Mocks.Repositories;

namespace UnitTests.Adria.Application;

public sealed class CalculateReservationTests
{
    [Fact]
    public async Task Execute_WithPods_ReturnsReservationDto()
    {
        // Arrange
        var pod1 = new PodDto(Guid.NewGuid(), "Pod 1", true, 5, 15, new CoordinatesDto(51.0, 3.0));
        var pod2 = new PodDto(Guid.NewGuid(), "Pod 2", false, 3, 10, new CoordinatesDto(51.1, 3.1));
        var pods = new List<PodDto> { pod1, pod2 };

        var mockQuery = new MockAllPodsQuery();
        mockQuery.SetPods(pods);

        var mockLogger = new MockLogger<CalculateReservation>();
        var mockStationRepository = new MockStationRepository();
        var useCase = new CalculateReservation(mockQuery, mockStationRepository, mockLogger);
        
        var station = new Station(Guid.NewGuid(), "Station", new Coordinates(51.0, 3.0));
        mockStationRepository.AddStation(station);

        var input = new CalculateReservationInput(new CoordInput(51.05, 3.05), station.Id);

        // Act
        var result = await useCase.Execute(input);

        // Assert
        Assert.NotNull(result);
        Assert.InRange(result.distance, 50, 250);
        Assert.NotEmpty(result.pods);
        Assert.Contains(result.pods, p => p.Name == "Pod 1");
        Assert.Contains(result.pods, p => p.Name == "Pod 2");
        Assert.Single(mockLogger.LoggedMessages);
        Assert.Contains("Fetching closest pods going to station", mockLogger.LoggedMessages[0]);
    }

    [Fact]
    public async Task Execute_WithNoPods_ReturnsEmptyPodsCollection()
    {
        // Arrange
        var pods = new List<PodDto>();

        var mockQuery = new MockAllPodsQuery();
        mockQuery.SetPods(pods);

        var mockLogger = new MockLogger<CalculateReservation>();
        var mockStationRepository = new MockStationRepository();
        var useCase = new CalculateReservation(mockQuery, mockStationRepository, mockLogger);
        
        var station = new Station(Guid.NewGuid(), "Station", new Coordinates(51.0, 3.0));
        mockStationRepository.AddStation(station);

        var input = new CalculateReservationInput(new CoordInput(51.05, 3.05), station.Id);

        // Act
        var result = await useCase.Execute(input);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.pods);
        Assert.InRange(result.distance, 50, 250);
        Assert.Single(mockLogger.LoggedMessages);
        Assert.Contains("Fetching closest pods going to station", mockLogger.LoggedMessages[0]);
    }

    [Fact]
    public async Task Execute_WithSinglePod_ReturnsSinglePodInDto()
    {
        // Arrange
        var pod1 = new PodDto(Guid.NewGuid(), "Pod 1", true, 5, 15, new CoordinatesDto(51.0, 3.0));
        var pods = new List<PodDto> { pod1 };

        var mockQuery = new MockAllPodsQuery();
        mockQuery.SetPods(pods);

        var mockLogger = new MockLogger<CalculateReservation>();
        var mockStationRepository = new MockStationRepository();
        var useCase = new CalculateReservation(mockQuery, mockStationRepository, mockLogger);

        var station = new Station(Guid.NewGuid(), "Station", new Coordinates(51.0, 3.0));
        mockStationRepository.AddStation(station);

        var input = new CalculateReservationInput(new CoordInput(51.05, 3.05), station.Id);

        // Act
        var result = await useCase.Execute(input);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.pods);
        Assert.Contains(result.pods, p => p.Name == "Pod 1");
        Assert.InRange(result.distance, 50, 250);
        Assert.Single(mockLogger.LoggedMessages);
        Assert.Contains("Fetching closest pods going to station", mockLogger.LoggedMessages[0]);
    }
    
    [Fact]
    public async Task Execute_WithMissingStation_ThrowsElementNotFoundException()
    {
        // Arrange
        var pod1 = new PodDto(Guid.NewGuid(), "Pod 1", true, 5, 15, new CoordinatesDto(51.0, 3.0));
        var pods = new List<PodDto> { pod1 };

        var mockQuery = new MockAllPodsQuery();
        mockQuery.SetPods(pods);

        var mockLogger = new MockLogger<CalculateReservation>();
        var mockStationRepository = new MockStationRepository();
        var useCase = new CalculateReservation(mockQuery, mockStationRepository, mockLogger);

        var input = new CalculateReservationInput(
            new CoordInput(51.05, 3.05),
            Guid.NewGuid()
        );

        // Act & Assert
        await Assert.ThrowsAsync<ElementNotFoundException>(() => useCase.Execute(input));
        Assert.Empty(mockLogger.LoggedMessages);
    }
}
