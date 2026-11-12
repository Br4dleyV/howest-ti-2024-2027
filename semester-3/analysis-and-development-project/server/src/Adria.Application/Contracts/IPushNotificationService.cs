using Adria.Domain.Models;

namespace Adria.Application.Contracts;

public interface IPushNotificationService
{
    Task SendToAllAsync(NotificationMessage message);
}