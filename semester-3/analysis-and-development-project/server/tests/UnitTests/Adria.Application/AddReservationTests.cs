using Adria.Application.Actions;
using Adria.Domain.Models;
using Adria.Domain.Shared.Exceptions;
using UnitTests.Mocks;
using UnitTests.Mocks.Repositories;

namespace UnitTests.Adria.Application;

public class AddReservationTests
{
    [Fact]
    public async Task Execute_WithValidInput_ReturnsReservationId()
    {
        // Arrange
        var user = new User(
            Guid.NewGuid(),
            "John",
            "Doe",
            new DateTime(1990, 1, 1),
            Role.User
        );

        var station = new Station(
            Guid.NewGuid(),
            "End Station",
            new Coordinates(51.0, 3.0)
        );

        var pod = new Pod(
            Guid.NewGuid(),
            "Pod 1",
            true,
            0,
            5,
            new Coordinates(51.1, 3.1)
        );

        var mockUserRepository = new MockUserRepository();
        mockUserRepository.AddUser(user);

        var mockStationRepository = new MockStationRepository();
        mockStationRepository.AddStation(station);

        var mockPodRepository = new MockPodRepository();
        mockPodRepository.AddPod(pod);

        var mockReservationRepository = new MockReservationRepository();
        var mockLogger = new MockLogger<AddReservation>();

        var useCase = new AddReservation(
            mockReservationRepository,
            mockUserRepository,
            mockStationRepository,
            mockPodRepository,
            mockLogger
        );

        var input = new AddReservationInput(
            user.Id,
            new Coordinates(51.05, 3.05),
            station.Id,
            pod.Id
        );

        // Act
        var reservationId = await useCase.Execute(input);

        // Assert
        Assert.NotEqual(Guid.Empty, reservationId);
        Assert.Single(mockReservationRepository.Reservations);
        Assert.Equal(reservationId, mockReservationRepository.Reservations.First().Id);

        Assert.Single(mockLogger.LoggedMessages);
        Assert.Contains("New reservation created with ID", mockLogger.LoggedMessages[0]);
        Assert.Contains(user.Id.ToString(), mockLogger.LoggedMessages[0]);
    }

    [Fact]
    public async Task Execute_WithMissingUser_ThrowsElementNotFoundException()
    {
        // Arrange
        var mockUserRepository = new MockUserRepository();
        var mockStationRepository = new MockStationRepository();
        var mockPodRepository = new MockPodRepository();
        var mockReservationRepository = new MockReservationRepository();
        var mockLogger = new MockLogger<AddReservation>();

        var station = new Station(Guid.NewGuid(), "Station", new Coordinates(51.0, 3.0));
        mockStationRepository.AddStation(station);

        var pod = new Pod(Guid.NewGuid(), "Pod", true, 0, 5, new Coordinates(51.1, 3.1));
        mockPodRepository.AddPod(pod);

        var useCase = new AddReservation(
            mockReservationRepository,
            mockUserRepository,
            mockStationRepository,
            mockPodRepository,
            mockLogger
        );

        var input = new AddReservationInput(
            Guid.NewGuid(),
            new Coordinates(51.05, 3.05),
            station.Id,
            pod.Id
        );

        // Act & Assert
        await Assert.ThrowsAsync<ElementNotFoundException>(() => useCase.Execute(input));
        Assert.Empty(mockLogger.LoggedMessages);
    }

    [Fact]
    public async Task Execute_WithMissingStation_ThrowsElementNotFoundException()
    {
        // Arrange
        var user = new User(
            Guid.NewGuid(),
            "Jane",
            "Doe",
            new DateTime(1992, 5, 5),
            Role.User
        );

        var mockUserRepository = new MockUserRepository();
        mockUserRepository.AddUser(user);

        var mockStationRepository = new MockStationRepository();
        var mockPodRepository = new MockPodRepository();
        var mockReservationRepository = new MockReservationRepository();
        var mockLogger = new MockLogger<AddReservation>();

        var pod = new Pod(Guid.NewGuid(), "Pod", true, 0, 5, new Coordinates(51.1, 3.1));
        mockPodRepository.AddPod(pod);

        var useCase = new AddReservation(
            mockReservationRepository,
            mockUserRepository,
            mockStationRepository,
            mockPodRepository,
            mockLogger
        );

        var input = new AddReservationInput(
            user.Id,
            new Coordinates(51.05, 3.05),
            Guid.NewGuid(),
            pod.Id
        );

        // Act & Assert
        await Assert.ThrowsAsync<ElementNotFoundException>(() => useCase.Execute(input));
        Assert.Empty(mockLogger.LoggedMessages);
    }

    [Fact]
    public async Task Execute_WithMissingPod_ThrowsElementNotFoundException()
    {
        // Arrange
        var user = new User(
            Guid.NewGuid(),
            "Jane",
            "Doe",
            new DateTime(1992, 5, 5),
            Role.User
        );

        var station = new Station(
            Guid.NewGuid(),
            "Station",
            new Coordinates(51.0, 3.0)
        );

        var mockUserRepository = new MockUserRepository();
        mockUserRepository.AddUser(user);

        var mockStationRepository = new MockStationRepository();
        mockStationRepository.AddStation(station);

        var mockPodRepository = new MockPodRepository();
        var mockReservationRepository = new MockReservationRepository();
        var mockLogger = new MockLogger<AddReservation>();

        var useCase = new AddReservation(
            mockReservationRepository,
            mockUserRepository,
            mockStationRepository,
            mockPodRepository,
            mockLogger
        );

        var input = new AddReservationInput(
            user.Id,
            new Coordinates(51.05, 3.05),
            station.Id,
            Guid.NewGuid()
        );

        // Act & Assert
        await Assert.ThrowsAsync<ElementNotFoundException>(() => useCase.Execute(input));
        Assert.Empty(mockLogger.LoggedMessages);
    }
}