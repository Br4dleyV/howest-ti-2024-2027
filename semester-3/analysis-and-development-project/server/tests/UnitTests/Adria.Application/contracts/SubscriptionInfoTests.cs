using Adria.Application.Contracts;

namespace UnitTests.Adria.Application.contracts;

public class SubscriptionInfoTests
{
    [Fact]
    public void Constructor_WithAllParameters_CreatesSubscriptionInfo()
    {
        // Arrange
        var endpoint = "https://fcm.googleapis.com/fcm/send/c3lK1JxA2Bc:APA91bHqFakeEndpointString1234567890";
        var p256dh = "BOr7f8Hn2FakeP256dhKey1234567890abcdefghijklmnopqrstu";
        var auth = "mF_fakeAuthKey_12345";

        // Act
        var subscriptionInfo = new SubscriptionInfo(endpoint, p256dh, auth);

        // Assert
        Assert.Equal(subscriptionInfo.Endpoint, endpoint);
        Assert.Equal(subscriptionInfo.P256dh, p256dh);
        Assert.Equal(subscriptionInfo.Auth, auth);
    }
}