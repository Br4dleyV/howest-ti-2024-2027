using Adria.Domain.Models;

namespace UnitTests.Adria.Domain;

public class PodsListTests
{
    [Fact]
    public void GetDistanceToLocation_GivesCorrectDistance()
    {
        // Arrange
        var ids = Enumerable.Range(0, 10).Select(x => Guid.NewGuid()).ToList();

        var pods = new PodsList([
            new Pod(ids[0], "Pod 1", true, 15, 30, new Coordinates(51.2090, 3.2200)),
            new Pod(ids[1], "Pod 2", true, 15, 30, new Coordinates(51.2100, 3.2210)),
            new Pod(ids[2], "Pod 3", true, 15, 30, new Coordinates(51.2110, 3.2220)),
            new Pod(ids[3], "Pod 4", true, 15, 30, new Coordinates(51.2120, 3.2230)),
            new Pod(ids[4], "Pod 5", true, 15, 30, new Coordinates(51.2130, 3.2240)),
            new Pod(ids[5], "Pod 6", true, 15, 30, new Coordinates(51.2140, 3.2250)),
            new Pod(ids[6], "Pod 7", true, 15, 30, new Coordinates(51.2150, 3.2260)),
            new Pod(ids[7], "Pod 8", true, 15, 30, new Coordinates(51.2160, 3.2270)),
            new Pod(ids[8], "Pod 9", true, 15, 30, new Coordinates(51.2170, 3.2280)),
            new Pod(ids[9], "Pod 10", true, 15, 30, new Coordinates(51.2180, 3.2290))
        ]);

        var userLocation = new Coordinates(51.2115, 3.2225);

        // Act
        var closestPods = pods.GetClosestPodsToLocation(userLocation);
        var closestPodsIds = closestPods.Select(x => x.Id).ToList();
        
        // Assert
        Assert.Equal(4, closestPods.Count);
        Assert.All([ids[1], ids[2], ids[3], ids[4]], id => Assert.Contains(id, closestPodsIds));
    }
}