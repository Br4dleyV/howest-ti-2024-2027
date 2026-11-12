using Adria.Application.Contracts;
using Adria.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Actions;

public record SendWeatherAlertInput(
    string Subject,
    Coordinates Zone,
    string Message
);

public class SendWeatherAlert(IPushNotificationService pushService, ILogger<SendWeatherAlert> logger)
    : IUseCase<SendWeatherAlertInput, Task>
{
    public async Task Execute(SendWeatherAlertInput input)
    {
        logger.LogInformation("Sending weather alert to all users");

        var notification = new NotificationMessage
        (
            title: BuildTitle(input),
            body: input.Message,
            url: "/alerts"
        );

        await pushService.SendToAllAsync(notification);
    }

    private static string BuildTitle(SendWeatherAlertInput input)
    {
        return input.Subject switch
        {
            "extreme_wind" => $"Extreme wind in zone {input.Zone}",
            "storm" => $"Storm in zone {input.Zone}",
            "rain" => $"Regenwaarschuwing in zone {input.Zone}",
            _ => $"Waarschuwing in zone {input.Zone}"
        };
    }
}