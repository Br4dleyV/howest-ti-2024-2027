using Microsoft.Extensions.Http.Resilience;

namespace ReadingSummary.Api.Services;

// This class configures resilience settings for the HTTP client used to communicate with the reading list service. It sets timeouts, retry policies, and other resilience-related options to ensure that the application can handle transient failures and timeouts gracefully.
public class ReadingListResilience
{
    public static void Configure(HttpStandardResilienceOptions options)
    {
        options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(5); // Set total request timeout to 5 seconds
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(2); // Set attempt timeout to 2 seconds

        options.Retry.MaxRetryAttempts = 1; // Set maximum retry attempts to 1
        options.Retry.Delay = TimeSpan.FromMilliseconds(200); // Set delay between retries to 200 milliseconds
        options.Retry.UseJitter = true; // Enable jitter for retry delays
        options.Retry.DisableForUnsafeHttpMethods(); // Disable retries for unsafe HTTP methods (e.g., POST, PUT, DELETE)
    }
}
