using Adria.Application.Actions;
using Adria.Domain.Models;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application;

public sealed class SendWeatherAlertTests
{
    [Fact]
    public async Task Execute_WithExtremeWind_BuildsCorrectNotification()
    {
        // Arrange
        var mockPush = new MockPushNotificationService();
        var mockLogger = new MockLogger<SendWeatherAlert>();

        var useCase = new SendWeatherAlert(mockPush, mockLogger);

        var input = new SendWeatherAlertInput(
            Subject: "extreme_wind",
            Zone: new Coordinates(10, 20),
            Message: "Be careful!"
        );

        // Act
        await useCase.Execute(input);

        // Assert
        Assert.Single(mockPush.NotificationMessages);
        var notification = mockPush.NotificationMessages[0];

        Assert.Equal("Extreme wind in zone (10, 20)", notification.Title);
        Assert.Equal("Be careful!", notification.Body);
        Assert.Equal("/alerts", notification.Url);

        Assert.Single(mockLogger.LoggedMessages);
        Assert.Contains("Sending weather alert to all users", mockLogger.LoggedMessages[0]);
    }

    [Fact]
    public async Task Execute_WithStorm_BuildsCorrectNotification()
    {
        // Arrange
        var mockPush = new MockPushNotificationService();
        var mockLogger = new MockLogger<SendWeatherAlert>();

        var useCase = new SendWeatherAlert(mockPush, mockLogger);

        var input = new SendWeatherAlertInput(
            "storm",
            new Coordinates(5, 5),
            "Storm alert!"
        );

        // Act
        await useCase.Execute(input);

        // Assert
        Assert.Single(mockPush.NotificationMessages);
        var notification = mockPush.NotificationMessages[0];

        Assert.Equal("Storm in zone (5, 5)", notification.Title);
        Assert.Equal("Storm alert!", notification.Body);
    }

    [Fact]
    public async Task Execute_WithRain_BuildsCorrectNotification()
    {
        // Arrange
        var mockPush = new MockPushNotificationService();
        var mockLogger = new MockLogger<SendWeatherAlert>();

        var useCase = new SendWeatherAlert(mockPush, mockLogger);

        var input = new SendWeatherAlertInput(
            "rain",
            new Coordinates(1, 2),
            "Het regent!"
        );

        // Act
        await useCase.Execute(input);

        // Assert
        var notif = mockPush.NotificationMessages.Single();

        Assert.Equal("Regenwaarschuwing in zone (1, 2)", notif.Title);
        Assert.Equal("Het regent!", notif.Body);
    }

    [Fact]
    public async Task Execute_WithUnknownSubject_UsesFallback()
    {
        // Arrange
        var mockPush = new MockPushNotificationService();
        var mockLogger = new MockLogger<SendWeatherAlert>();

        var useCase = new SendWeatherAlert(mockPush, mockLogger);

        var input = new SendWeatherAlertInput(
            "unknown_alert",
            new Coordinates(0, 0),
            "Something happened!"
        );

        // Act
        await useCase.Execute(input);

        // Assert
        var notif = mockPush.NotificationMessages.Single();

        Assert.Equal("Waarschuwing in zone (0, 0)", notif.Title);
        Assert.Equal("Something happened!", notif.Body);
    }
}