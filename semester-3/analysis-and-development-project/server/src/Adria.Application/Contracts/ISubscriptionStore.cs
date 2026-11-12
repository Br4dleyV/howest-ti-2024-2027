namespace Adria.Application.Contracts;

public interface ISubscriptionStore
{
    Task AddAsync(string endpoint, string p256dh, string auth);

    Task<IReadOnlyCollection<SubscriptionInfo>> GetAllAsync();
}

public record SubscriptionInfo(
    string Endpoint,
    string P256dh,
    string Auth
);