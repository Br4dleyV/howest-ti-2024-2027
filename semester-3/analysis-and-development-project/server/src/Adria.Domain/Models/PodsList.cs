namespace Adria.Domain.Models;

public class PodsList(List<Pod> pods)
{
    private List<Pod> Pods { get; set; } = pods;

    public IReadOnlyCollection<Pod> GetClosestPodsToLocation(Coordinates location)
    {
        return Pods
            .OrderBy(pod => pod.GetDistanceToPod(location))
            .Take(4)
            .ToList()
            .AsReadOnly();
    }
}