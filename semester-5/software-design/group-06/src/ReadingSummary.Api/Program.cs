// Initialize app builder
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Http.Resilience;
using ReadingList.ServiceDefaults;
using ReadingSummary.Api.Health;
using ReadingSummary.Api.Endpoints;
using ReadingSummary.Api.Models;
using ReadingSummary.Api.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureOpenTelemetry(); // Send structured logs and request traces to the Aspire dashboard
builder.Services.AddOpenApi(); // Register OpenAPI support for API documentation
builder.Services.AddHealthChecks().AddCheck<SummaryReadinessCheck>  ("summary-ready", tags: new[] { "ready" }, timeout: TimeSpan.FromSeconds(10)); // Register health checks for monitoring the app's health
builder.Services.AddServiceDiscovery(); // Add service discovery for Aspire

// Configure the HTTP client for the reading list service with resilience and service discovery
var readingListClient = builder.Services.AddHttpClient("reading-list", client =>
{
    client.BaseAddress = new Uri("http://reading-list-api"); // With Aspire service name

    // Resilience-handler manages the timeouts
    client.Timeout = Timeout.InfiniteTimeSpan; // Set timeout for HTTP requests
});

// Add resilience handler to the HTTP client for handling timeouts and retries
readingListClient.AddStandardResilienceHandler(ReadingListResilience.Configure);
readingListClient.AddServiceDiscovery(); // Enable service discovery for the HTTP client

// Build the app
var app = builder.Build();

// If app is in developer mode
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Map OpenAPI endpoints for API documentation
}

// Add middleware to redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

//------------------------------------------------------

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false // Exclude all checks, just return 200 OK for liveness probe
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready") // Include only checks tagged with "ready" for readiness probe
});

app.MapGet("/summary", async (IHttpClientFactory factory, CancellationToken cancellationToken) =>
{
    // Create an HTTP client for the "reading-list" service using the IHttpClientFactory
    var client = factory.CreateClient("reading-list");
    // Call the SummaryEndpoint.HandleAsync method to get the reading summary and return the result
    var result = await SummaryEndpoint.HandleAsync(client, cancellationToken);
    var statusCode = (result as IStatusCodeHttpResult)?.StatusCode;

    // Record the outcome within the current trace so upstream failures can be investigated in the dashboard.
    if (statusCode >= 400)
    {
        app.Logger.LogWarning("Reading summary request failed with status code {StatusCode}.", statusCode);
    }
    else
    {
        app.Logger.LogInformation("Reading summary request completed with status code {StatusCode}.", statusCode);
    }

    return result;

})
.WithName("GetReadingSummary")
.WithDescription("Get a count of unread and finished titles from the reading list service.");

//------------------------------------------------------

// Start web server and listen for requests
app.Run();
