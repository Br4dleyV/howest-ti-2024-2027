using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using ReadingSummary.Api.Endpoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReadingList.UnitTests;

public class SummaryEndpointTests
{
    [Theory]
    [InlineData("null")]
    [InlineData("[null]")]
    [InlineData("[{}]")]
    [InlineData("""[{"title":"A title","isRead":false}]""")]
    [InlineData("""[{"id":"11111111-1111-1111-1111-111111111111","isRead":false}]""")]
    [InlineData("""[{"id":"11111111-1111-1111-1111-111111111111","title":"A title"}]""")]
    [InlineData("""[{"id":"00000000-0000-0000-0000-000000000000","title":"A title","isRead":false}]""")]
    [InlineData("""[{"id":"11111111-1111-1111-1111-111111111111","title":null,"isRead":false}]""")]
    [InlineData("""[{"id":"11111111-1111-1111-1111-111111111111","title":"","isRead":false}]""")]
    [InlineData("""[{"id":"11111111-1111-1111-1111-111111111111","title":"   ","isRead":false}]""")]
    [InlineData("""[{"id":"11111111-1111-1111-1111-111111111111","title":"A title","isRead":null}]""")]
    [InlineData("""[{"id":"11111111-1111-1111-1111-111111111111","title":"A title","isRead":false},null]""")]
    public async Task HandleAsync_WithInvalidTitleData_ReturnsBadGateway(string json)
    {
        // Arrange
        /// Return valid JSON with unusable title data without making a real HTTP request
        using var handler = new JsonHandler(json);
        using var client = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://reading-list.test")
        };
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        // Act
        var result = await SummaryEndpoint.HandleAsync(client, timeout.Token);

        // Assert
        /// Invalid upstream data must return a clear failure instead of a partial or incorrect summary
        var problem = Assert.IsType<ProblemHttpResult>(result);
        Assert.Equal(StatusCodes.Status502BadGateway, problem.StatusCode);
        Assert.Equal("Bad Gateway", problem.ProblemDetails.Title);
    }

    [Theory]
    [InlineData("[]", 0, 0)]
    [InlineData("""[{"id":"11111111-1111-1111-1111-111111111111","title":"Unread title","isRead":false},{"id":"22222222-2222-2222-2222-222222222222","title":"Finished title","isRead":true}]""", 1, 1)]
    public async Task HandleAsync_WithValidTitleData_ReturnsSummary(string json, int unread, int finished)
    {
        // Arrange
        /// An empty list and titles with both reading statuses are valid upstream responses
        using var handler = new JsonHandler(json);
        using var client = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://reading-list.test")
        };
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        // Act
        var result = await SummaryEndpoint.HandleAsync(client, timeout.Token);

        // Assert
        var response = Assert.IsType<Ok<ReadingSummary.Api.Models.ReadingSummary>>(result);
        Assert.NotNull(response.Value);
        Assert.Equal(unread, response.Value.Unread);
        Assert.Equal(finished, response.Value.Finished);
    }

    [Fact]
    public async Task HandleAsync_WithInvalidJson_ReturnsBadGateway()
    {
        // Arrange
        /// Create an instance of the InvalidJsonHandler to simulate a response with invalid JSON
        using var handler = new InvalidJsonHandler();

        /// Create an HTTP client with the InvalidJsonHandler and set the base address to "http://reading-list.test"
        using var client = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://reading-list.test")
        };

        /// Create a cancellation token source with a timeout of 5 seconds
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        // Act
        /// Call the HandleAsync method of the SummaryEndpoint class with the HTTP client and cancellation token
        var result = await SummaryEndpoint.HandleAsync(client, timeout.Token);

        // Assert
        /// Assert that the result is of type ProblemHttpResult, indicating a Bad Gateway response
        var problem = Assert.IsType<ProblemHttpResult>(result);

        Assert.Equal(StatusCodes.Status502BadGateway, problem.StatusCode); // Assert that the status code is 502 Bad Gateway
        Assert.Equal("Bad Gateway", problem.ProblemDetails.Title); // Assert that the title of the problem details is "Bad Gateway"
    }

    [Fact]
    public async Task HandleAsync_WhenUpstreamTimesOut_ReturnsGatewayTimeout()
    {
        // Arrange
        /// Create an instance of the WaitingHandler to simulate a timeout scenario
        using var handler = new WaitingHandler();

        /// Create an HTTP client with the WaitingHandler and set the base address to "http://reading-list.test" and a timeout of 100 milliseconds
        using var client = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://reading-list.test"), // Set the base address for the HTTP client
            Timeout = TimeSpan.FromMilliseconds(100) // Set the timeout for the HTTP client to 1 second
        };

        /// Create a cancellation token source with a timeout of 5 seconds
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        // Act
        /// Call the HandleAsync method of the SummaryEndpoint class with the HTTP client and cancellation token
        var result = await SummaryEndpoint.HandleAsync(client, timeout.Token);

        // Assert
        /// Assert that the result is of type ProblemHttpResult, indicating a Gateway Timeout response
        var problem = Assert.IsType<ProblemHttpResult>(result);

        Assert.Equal(StatusCodes.Status504GatewayTimeout, problem.StatusCode); // Assert that the status code is 504 Gateway Timeout
        Assert.False(timeout.IsCancellationRequested); // Assert that the cancellation token has not been triggered
    }

    [Fact]
    public async Task HandleAsync_WhenCallerCancels_PropagatesCancellation()
    {
        // Arrange
        /// Create an instance of the WaitingHandler to simulate a delay in response
        using var handler = new WaitingHandler();

        /// Create an HTTP client with the WaitingHandler and set the base address to "http://reading-list.test"
        using var client = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://reading-list.test"), // Set the base address for the HTTP client
            Timeout = Timeout.InfiniteTimeSpan // Set the timeout for the HTTP client to infinite to allow for cancellation
        };

        /// Create a cancellation token source with a timeout of 100 milliseconds
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        // Act and Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => SummaryEndpoint.HandleAsync(client, cancellation.Token));
    }

    // This class is a custom HttpMessageHandler that returns supplied JSON to test the upstream HTTP contract.
    private sealed class JsonHandler(string json) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            return Task.FromResult(response); // Return the response without real network traffic
        }
    }

    // This class is a custom HttpMessageHandler that simulates a response with invalid JSON content.
    private sealed class InvalidJsonHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Create a response message with status code 200 OK and invalid JSON content
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent("This is invalid JSON", Encoding.UTF8, "application/json")
            };
            return Task.FromResult(response); // Return the response message as a completed task
        }
    }

    // This class is a custom HttpMessageHandler that simulates a delay in response to trigger a timeout scenario.
    private sealed class WaitingHandler : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Simulate a delay of 10 seconds to trigger a timeout scenario
            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

            // Throw an exception if the handler finishes without cancellation
            throw new InvalidOperationException("The handler should only finish through cancellation.");
        }
    }
}
