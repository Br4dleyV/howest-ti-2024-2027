using Adria.Domain.Models;

namespace UnitTests.Adria.Domain;

public class NotificationMessageTests
{
    [Fact]
    public void Constructor_WithAllParameters_CreatesNotificationMessage()
    {
        // Arrange
        var title = "Regenwaarschuwing in zone (1, 2)";
        var body = "Het regent!";
        var url = "/alerts";

        // Act
        var subscriptionInfo = new NotificationMessage(title, body, url);

        // Assert
        Assert.Equal(subscriptionInfo.Title, title);

        Assert.Equal(subscriptionInfo.Body, body);
        Assert.Equal(subscriptionInfo.Url, url);
    }
}