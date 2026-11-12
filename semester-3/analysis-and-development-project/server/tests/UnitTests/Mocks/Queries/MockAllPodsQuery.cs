using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;

namespace UnitTests.Mocks.Queries;

public class MockAllPodsQuery : IAllPodsQuery
{
    private List<PodDto> _pods = [];

    public Task<IReadOnlyCollection<PodDto>> Fetch()
    {
        IReadOnlyCollection<PodDto> result = _pods.AsReadOnly();
        return Task.FromResult(result);
    }

    public void SetPods(List<PodDto> pods)
    {
        _pods = pods;
    }

    public void Clear()
    {
        _pods.Clear();
    }
}