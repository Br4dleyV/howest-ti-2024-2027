using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Models;

namespace UnitTests.Mocks.Queries;

public class MockAllStationsQuery : IAllStationsQuery
{
    private readonly List<StationDto> _stations = [];

    public Task<IReadOnlyCollection<StationDto>> Fetch()
    {
        IReadOnlyCollection<StationDto> result = _stations.AsReadOnly();
        return Task.FromResult(result);
    }

    public void Add(StationDto station)
    {
        _stations.Add(station);
    }

    public void Clear()
    {
        _stations.Clear();
    }
}