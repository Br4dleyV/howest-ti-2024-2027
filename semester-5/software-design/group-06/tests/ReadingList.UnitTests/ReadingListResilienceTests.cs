using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using ReadingSummary.Api.Endpoints;
using ReadingSummary.Api.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

// These tests are designed to validate the resilience of the reading list service, ensuring that it can handle various failure scenarios gracefully.
namespace ReadingList.UnitTests
{
    public class ReadingListResilienceTests
    {
        [Fact]
        public async Task Get_WhenFirstAttemptFails_RetriesAndSucceeds()
        {
            // Arrange
            /// Create an instance of the TemporaryFailureHandler to simulate a temporary failure scenario
            var handler = new TemporaryFailureHandler();

            /// Create service collection to register the HTTP client and its dependencies
            var services = new ServiceCollection();

            /// Add an HTTP client named "retry-test" to the service collection with a base address and timeout configuration
            var clientBuilder = services.AddHttpClient("retry-test", client =>
            {
                client.BaseAddress = new Uri("http://reading-list.test"); // Set the base address for the HTTP client
                client.Timeout = TimeSpan.FromSeconds(5); // Set the timeout for the HTTP client to 5 seconds
            });

            /// Configure the primary HTTP message handler to use the TemporaryFailureHandler, which simulates a temporary failure scenario
            clientBuilder.ConfigurePrimaryHttpMessageHandler(() => handler); // Configure the primary HTTP message handler to use the TemporaryFailureHandler
            clientBuilder.AddStandardResilienceHandler(ReadingListResilience.Configure); // Add the standard resilience handler to the HTTP client

            /// Build the service provider to resolve dependencies
            using var provider = services.BuildServiceProvider(); // Build the service provider to resolve dependencies

            using var client = provider.GetRequiredService<IHttpClientFactory>().CreateClient("retry-test"); // Create an HTTP client instance

            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5)); // Create a cancellation token source with a timeout of 5 seconds

            // Act
            using var response = await client.GetAsync("/titles", timeout.Token); // Send a GET request to the "/titles" endpoint

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Assert that the response status code is OK (200)
            Assert.Equal(2, handler.RequestCount); // Assert that the request count is 2, indicating that the first attempt failed and the second attempt succeeded
            Assert.Equal("[]", await response.Content.ReadAsStringAsync(timeout.Token)); // Assert that the response content is an empty JSON array
        }

        [Fact]
        public async Task HandleAsync_WhenUpstreamStaysUnavailable_Returns503()
        {
            // Arrange
            var handler = new UnavailableHandler(); // Create an instance of the UnavailableHandler to simulate an upstream service that remains unavailable
            var services = new ServiceCollection(); // Create a service collection to register the HTTP client and its dependencies

            /// Add an HTTP client named "unavailable-test" to the service collection with a base address and timeout configuration
            var clientBuilder = services.AddHttpClient("unavailable-test", client =>
            {
                client.BaseAddress = new Uri("http://reading-list.test"); // Set the base address for the HTTP client
                client.Timeout = Timeout.InfiniteTimeSpan; // Set the timeout for the HTTP client to 5 seconds
            });

            clientBuilder.ConfigurePrimaryHttpMessageHandler(() => handler); // Configure the primary HTTP message handler to use the UnavailableHandler
            clientBuilder.AddStandardResilienceHandler(ReadingListResilience.Configure); // Add the standard resilience handler to the HTTP client

            using var provider = services.BuildServiceProvider(); // Build the service provider to resolve dependencies

            using var client = provider.GetRequiredService<IHttpClientFactory>().CreateClient("unavailable-test"); // Create an HTTP client instance

            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5)); // Create a cancellation token source with a timeout of 5 seconds

            // Act
            var result = await SummaryEndpoint.HandleAsync(client, timeout.Token); // Call the HandleAsync method of the SummaryEndpoint with the HTTP client and cancellation token

            // Assert
            var problem = Assert.IsType<ProblemHttpResult>(result); // Assert that the result is of type ProblemHttpResult

            Assert.Equal(StatusCodes.Status503ServiceUnavailable, problem.StatusCode); // Assert that the status code of the problem result is 503 Service Unavailable
            Assert.Equal(2, handler.RequestCount); // Assert that the request count is 2, indicating that the first attempt failed and the second attempt also failed
        }

        private sealed class TemporaryFailureHandler : DelegatingHandler
        {
            public int RequestCount { get; private set; } = 0; // Property to track the number of requests made
            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                RequestCount++; // Increment the request count for each request made

                if (RequestCount == 1)
                {
                    // Simulate a temporary failure on the first request by returning a 503 Service Unavailable response
                    return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
                }

                // On subsequent requests, return a successful response with an empty JSON array
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("[]", Encoding.UTF8, "application/json") // Return an empty JSON array as the response content
                };
            }
        }

        private sealed class UnavailableHandler : DelegatingHandler
        {
            public int RequestCount { get; private set; } = 0; // Property to track the number of requests made
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                RequestCount++; // Increment the request count for each request made
                // Simulate an upstream service that remains unavailable by always returning a 503 Service Unavailable response
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));
            }
        }
    }
}
