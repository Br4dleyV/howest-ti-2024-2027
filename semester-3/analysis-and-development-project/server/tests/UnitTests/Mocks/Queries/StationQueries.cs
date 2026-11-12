using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;

namespace UnitTests.Mocks;

public class MockAllStationsQuery : IAllStationsQuery
{
    private readonly List<StationDto> _stations = [];
    public Task<IReadOnlyCollection<StationDto>> Fetch()
    {
        IReadOnlyCollection<StationDto> result = _stations.AsReadOnly();
        return Task.FromResult(result);
    }

    public void AddStation(StationDto station)
    {
        _stations.Add(station);
    }

    public void Clear()
    {
        _stations.Clear();
    }
}