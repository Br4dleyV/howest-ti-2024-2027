using Adria.Application.Actions;
using Adria.Application.Contracts.Data;
using UnitTests.Mocks;
using UnitTests.Mocks.Queries;

namespace UnitTests.Adria.Application;

public sealed class SearchAllPodsTests
{
    [Fact]
    public async Task Execute_WithPods_ReturnsPods()
    {
        // Arrange
        var pod1 = new PodDto(Guid.NewGuid(), "Pod 1", true, 0, 0, new(0, 0));
        var pod2 = new PodDto(Guid.NewGuid(), "Pod 2", false, 0, 0, new(0, 0));
        var pods = new List<PodDto> { pod1, pod2 };

        var mockQuery = new MockAllPodsQuery();
        mockQuery.SetPods(pods);

        var mockLogger = new MockLogger<SearchAllPods>();
        var useCase = new SearchAllPods(mockQuery, mockLogger);

        // Act
        var result = await useCase.Execute(new (null, null));

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.Name == "Pod 1");
        Assert.Contains(result, p => p.Name == "Pod 2");
        Assert.Single(mockLogger.LoggedMessages);
        Assert.Contains("Fetching all pods", mockLogger.LoggedMessages[0]);
    }
    
    [Fact]
    public async Task Execute_WithNoPods_ReturnsEmptyCollection()
    {
        // Arrange
        var pods = new List<PodDto>();

        var mockQuery = new MockAllPodsQuery();
        mockQuery.SetPods(pods);

        var mockLogger = new MockLogger<SearchAllPods>();
        var useCase = new SearchAllPods(mockQuery, mockLogger);

        // Act
        var result = await useCase.Execute(new (null, null));

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        Assert.Single(mockLogger.LoggedMessages);
        Assert.Contains("Fetching all pods", mockLogger.LoggedMessages[0]);
    }
    
    [Fact]
    public async Task Execute_WithSinglePod_ReturnsOnePod()
    {
        // Arrange
        var pod1 = new PodDto(Guid.NewGuid(), "Pod 1", true, 0, 0, new(0, 0));
        var pods = new List<PodDto> { pod1 };

        var mockQuery = new MockAllPodsQuery();
        mockQuery.SetPods(pods);

        var mockLogger = new MockLogger<SearchAllPods>();
        var useCase = new SearchAllPods(mockQuery, mockLogger);

        // Act
        var result = await useCase.Execute(new (null, null));

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, p => p.Name == "Pod 1");
        Assert.Single(mockLogger.LoggedMessages);
        Assert.Contains("Fetching all pods", mockLogger.LoggedMessages[0]);
    }
}
