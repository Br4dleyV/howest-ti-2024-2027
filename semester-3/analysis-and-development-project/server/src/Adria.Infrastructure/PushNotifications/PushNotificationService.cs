using System.Text.Json;
using Adria.Application.Contracts;
using Adria.Domain.Models;
using Microsoft.Extensions.Options;
using WebPush;

namespace Adria.Infrastructure.PushNotifications;

public class PushNotificationService(
    ISubscriptionStore subscriptionStore,
    IOptions<VapidSettings> vapidOptions
) : IPushNotificationService
{
    private readonly VapidSettings _vapid = vapidOptions.Value;
    private readonly WebPushClient _client = new();

    public async Task SendToAllAsync(NotificationMessage message)
    {
        var payload = JsonSerializer.Serialize(message);
        var subscriptions = await subscriptionStore.GetAllAsync();

        foreach (var s in subscriptions)
        {
            try
            {
                var subscription = new PushSubscription(
                    s.Endpoint,
                    s.P256dh,
                    s.Auth
                );

                var vapidDetails = new VapidDetails(
                    _vapid.Subject,
                    _vapid.PublicKey,
                    _vapid.PrivateKey
                );

                await _client.SendNotificationAsync(subscription, payload, vapidDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Push failed → {s.Endpoint}: {ex.Message}");
            }
        }
    }
}

public class VapidSettings
{
    public string Subject { get; set; } = "";
    public string PublicKey { get; set; } = "";
    public string PrivateKey { get; set; } = "";
}
