using Adria.Application.Contracts;
using Adria.Infrastructure.PushNotifications;

namespace Adria.Main.Modules.PushNotifications;

public static class PushNotificationModule
{
    public static IServiceCollection AddPushNotifications(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<VapidSettings>(configuration.GetSection("Vapid"));

        services.AddSingleton<ISubscriptionStore, InMemorySubscriptionStore>();

        services.AddSingleton<IPushNotificationService, PushNotificationService>();

        return services;
    }
}