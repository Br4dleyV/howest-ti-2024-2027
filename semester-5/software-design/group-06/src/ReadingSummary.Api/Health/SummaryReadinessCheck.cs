using Microsoft.Extensions.Diagnostics.HealthChecks;
using ReadingSummary.Api.Endpoints;

namespace ReadingSummary.Api.Health;

public class SummaryReadinessCheck(IHttpClientFactory factory) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var client = factory.CreateClient("reading-list"); // Use the named client configured in Program.cs

        var result = await SummaryEndpoint.HandleAsync(client, cancellationToken); // Call the endpoint to check readiness

        if (result is IStatusCodeHttpResult
            {
                StatusCode: StatusCodes.Status200OK
            })
        {
            return HealthCheckResult.Healthy("Reading List is available and a summary can be calculated");
        }

        return HealthCheckResult.Unhealthy("Reading List did not provide a usable response");
    }

}
