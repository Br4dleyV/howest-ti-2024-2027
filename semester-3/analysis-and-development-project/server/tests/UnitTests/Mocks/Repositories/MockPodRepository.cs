using Adria.Domain.Models;
using Adria.Domain.Repositories;

namespace UnitTests.Mocks.Repositories;

public class MockPodRepository : IPodRepository
{
    private readonly List<Pod> _pods = [];

    public void AddPod(Pod pod)
    {
        _pods.Add(pod);
    }

    public Task<Pod?> ById(Guid podId)
    {
        return Task.FromResult(_pods.FirstOrDefault(p => p.Id == podId));
    }
}