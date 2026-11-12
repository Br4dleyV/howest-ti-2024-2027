using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adria.Application.Contracts;

namespace Adria.Infrastructure.PushNotifications;

public class InMemorySubscriptionStore : ISubscriptionStore
{
    private readonly ConcurrentDictionary<string, SubscriptionInfo> _subscriptions = new();

    public Task AddAsync(string endpoint, string p256dh, string auth)
    {
        var info = new SubscriptionInfo(endpoint, p256dh, auth);
        _subscriptions[endpoint] = info;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<SubscriptionInfo>> GetAllAsync()
    {
        IReadOnlyCollection<SubscriptionInfo> result = _subscriptions.Values.ToList();
        return Task.FromResult(result);
    }
}