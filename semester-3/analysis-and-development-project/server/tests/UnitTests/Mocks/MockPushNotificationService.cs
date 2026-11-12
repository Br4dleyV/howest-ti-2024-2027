using Adria.Application.Contracts;
using Adria.Domain.Models;

namespace UnitTests.Mocks;

public class MockPushNotificationService : IPushNotificationService
{
    public IList<NotificationMessage> NotificationMessages { get; } = new List<NotificationMessage>();

    public Task SendToAllAsync(NotificationMessage message)
    {
        NotificationMessages.Add(message);
        return Task.CompletedTask;
    }
}