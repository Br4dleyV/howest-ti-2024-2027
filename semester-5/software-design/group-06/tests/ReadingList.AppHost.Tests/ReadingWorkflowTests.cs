using Aspire.Hosting.Testing;
using System.Net;
using System.Net.Http.Json;

namespace ReadingList.AppHost.Tests;

public class ReadingWorkflowTests
{
    [Fact]
    public async Task ReadingList_CanCreateAndFinishTitle()
    {
        // Arrange
        /// Create a cancellation token source with a timeout of 2 minutes
        using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(2));

        /// Create an instance of the distributed application host for testing
        await using var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.ReadingList_AppHost>(cancellationToken: timeout.Token);

        /// Build the application host
        await using var app = await appHost.BuildAsync(timeout.Token);

        /// Start the application host
        await app.StartAsync(timeout.Token);

        /// Wait for the "reading-list-api" resource to become healthy
        await app.ResourceNotifications.WaitForResourceHealthyAsync("reading-list-api", timeout.Token);

        /// Create an HTTP client for the "reading-list-api" resource
        using var client = app.CreateHttpClient("reading-list-api", "http");

        /// Wait for the "reading-summary-api" resource to become healthy
        await app.ResourceNotifications.WaitForResourceHealthyAsync("reading-summary-api", timeout.Token);

        /// Create an HTTP client for the "reading-summary-api" resource
        using var summaryClient = app.CreateHttpClient("reading-summary-api", "http");

        // Act
        /// Send a GET request to the "/titles" endpoint and assert that the response is successful
        using var response = await client.GetAsync("/titles", timeout.Token);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Assert that the response status code is 200 OK
        Assert.Equal("[]", (await response.Content.ReadAsStringAsync(timeout.Token)).Trim()); // Assert that the response content is an empty JSON array

        // Act
        using var createResponse = await client.PostAsJsonAsync("/titles", new { Title = "Spider-Man: No Way Home" }, timeout.Token);

        // Assert
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode); // Assert that the response status code is 201 Created

        /// Read the response content as a TitleResponse object
        var createdTitle = await createResponse.Content.ReadFromJsonAsync<TitleResponse>(cancellationToken: timeout.Token);

        // Assert
        Assert.NotNull(createdTitle); // Assert that the created title is not null
        Assert.NotEqual(Guid.Empty, createdTitle.Id); // Assert that the created title has a valid Id
        Assert.Equal("Spider-Man: No Way Home", createdTitle.Title); // Assert that the created title has the expected title
        Assert.False(createdTitle.IsRead); // Assert that the created title is not read

        // Act
        var titles = await client.GetFromJsonAsync<List<TitleResponse>>("/titles", timeout.Token);

        // Assert
        Assert.NotNull(titles); // Assert that the titles list is not null

        var savedTitle = Assert.Single(titles); // Assert that there is exactly one title in the list
        Assert.Equal(createdTitle.Id, savedTitle.Id); // Assert that the saved title has the same Id as the created title
        Assert.Equal("Spider-Man: No Way Home", savedTitle.Title); // Assert that the saved title has the expected title
        Assert.False(savedTitle.IsRead); // Assert that the saved title is not read

        // Act
        /// Send a GET request to the "/summary" endpoint and read the response as a SummaryResponse object
        using var summaryResponse = await summaryClient.GetAsync("/summary", timeout.Token);

        // Assert
        Assert.Equal(HttpStatusCode.OK, summaryResponse.StatusCode);

        /// Read the response content as a SummaryResponse object
        var summary = await summaryResponse.Content.ReadFromJsonAsync<SummaryResponse>(cancellationToken: timeout.Token);

        // Assert
        Assert.NotNull(summary); // Assert that the summary is not null
        Assert.Equal(1, summary.Unread); // Assert that there is 1 unread title
        Assert.Equal(0, summary.Finished); // Assert that there are 0 finished titles

        // Act
        /// Send a PUT request to the "/titles/{id}/finish" endpoint to mark the title as finished
        using var finishResponse = await client.PutAsync($"/titles/{createdTitle.Id}/finish", null, timeout.Token);

        // Assert
        Assert.Equal(HttpStatusCode.OK, finishResponse.StatusCode); // Assert that the response status code is 200 OK

        /// Read the response content as a TitleResponse object
        var finishedTitle = await finishResponse.Content.ReadFromJsonAsync<TitleResponse>(cancellationToken: timeout.Token);

        Assert.NotNull(finishedTitle); // Assert that the finished title is not null
        Assert.Equal(createdTitle.Id, finishedTitle.Id); // Assert that the finished title has the same Id as the created title
        Assert.Equal("Spider-Man: No Way Home", finishedTitle.Title); // Assert that the finished title has the expected title
        Assert.True(finishedTitle.IsRead); // Assert that the finished title is marked as read

        // Act
        /// Send a GET request to the "/summary" endpoint again to check the updated summary
        using var updatedSummaryResponse = await summaryClient.GetAsync("/summary", timeout.Token);

        // Assert
        Assert.Equal(HttpStatusCode.OK, updatedSummaryResponse.StatusCode); // Assert that the response status code is 200 OK

        /// Read the response content as a SummaryResponse object
        var updatedSummary = await updatedSummaryResponse.Content.ReadFromJsonAsync<SummaryResponse>(cancellationToken: timeout.Token);

        Assert.NotNull(updatedSummary); // Assert that the updated summary is not null
        Assert.Equal(0, updatedSummary.Unread); // Assert that there are now 0 unread titles
        Assert.Equal(1, updatedSummary.Finished); // Assert that there is now 1 finished title
    }

    [Fact]
    public async Task ReadingList_WithInvalidTitle_ReturnsBadRequest()
    {
        // Arrange
        /// Create a cancellation token source with a timeout of 2 minutes
        using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(2));

        /// Create an instance of the distributed application host for testing
        await using var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.ReadingList_AppHost>(cancellationToken: timeout.Token);

        /// Build the application host
        await using var app = await appHost.BuildAsync(timeout.Token);

        /// Start the application host
        await app.StartAsync(timeout.Token);

        /// Wait for the "reading-list-api" resource to become healthy
        await app.ResourceNotifications.WaitForResourceHealthyAsync("reading-list-api", timeout.Token);

        /// Create an HTTP client for the "reading-list-api" resource
        using var client = app.CreateHttpClient("reading-list-api", "http");

        // Act
        /// Send a POST request to the "/titles" endpoint with an invalid title (empty or whitespace) and assert that it returns a BadRequest response
        using var response = await client.PostAsJsonAsync("/titles", new { Title = "     " }, timeout.Token);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // Assert that the response status code is 400 Bad Request

        var titles = await client.GetFromJsonAsync<List<TitleResponse>>("/titles", timeout.Token);

        Assert.NotNull(titles); // Assert that the titles list is not null
        Assert.Empty(titles); // Assert that the titles list is empty since the invalid title was not added
    }
}

// Model for the response from the "/titles" endpoint
public record TitleResponse(Guid Id, string Title, bool IsRead);
// Model for the response from the "/summary" endpoint
public record SummaryResponse(int Unread, int Finished);