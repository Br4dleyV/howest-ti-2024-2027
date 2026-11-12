using Polly.Timeout;
using ReadingSummary.Api.Models;
using ReadingSummary.Api.Services;
using System.Text.Json;

namespace ReadingSummary.Api.Endpoints;

// SummaryEndpoint is responsible for handling the /summary endpoint, which retrieves a summary of unread and finished titles from the reading list service.
public class SummaryEndpoint
{
    public static async Task<IResult> HandleAsync(HttpClient client, CancellationToken cancellationToken)
    {
        try
        {
            var titles = await client.GetFromJsonAsync<List<TitleItem>>("/titles", cancellationToken);

            // Valid JSON must also contain usable titles before the summary can be calculated.
            if (titles == null || titles.Any(title => title == null || title.Id == Guid.Empty || string.IsNullOrWhiteSpace(title.Title)))
            {
                return Results.Problem(
                    detail: "The reading list service returned an unexpected response.",
                    statusCode: StatusCodes.Status502BadGateway,
                    title: "Bad Gateway"
                );
            }

            var summary = ReadingSummaryCalculator.Calculate(titles);
            return Results.Ok(summary);
        }
        catch (HttpRequestException)
        {
            // Caught network failures, DNS resolution errors, or non-success HTTP status codes
            return Results.Problem(
                detail: "The reading list service is currently unavailable.",
                statusCode: StatusCodes.Status503ServiceUnavailable,
                title: "Service Unavailable"
            );
        }
        catch (JsonException)
        {
            // Caught JSON deserialization errors
            return Results.Problem(
                detail: "The reading list service returned invalid JSON.",
                statusCode: StatusCodes.Status502BadGateway,
                title: "Bad Gateway"
            );
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            // Caught timeout scenarios
            return Results.Problem(
                detail: "The reading list service did not respond in time.",
                statusCode: StatusCodes.Status504GatewayTimeout,
                title: "Gateway Timeout"
            );
        }
        catch (TimeoutRejectedException)
        {
            // Caught timeout scenarios from Polly
            return Results.Problem(
                detail: "The reading list service did not respond in time.",
                statusCode: StatusCodes.Status504GatewayTimeout,
                title: "Gateway Timeout"
            );
        }
    }
}
