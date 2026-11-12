using Adria.Domain.Models;
using Adria.Domain.Repositories;

namespace UnitTests.Mocks.Repositories;

public class MockStationRepository : IStationRepository
{
    private readonly List<Station> _stations = [];

    public void AddStation(Station station)
    {
        _stations.Add(station);
    }

    public Task<Station?> ById(Guid stationId)
    {
        return Task.FromResult<Station?>(_stations.FirstOrDefault(s => s.Id == stationId));
    }
}