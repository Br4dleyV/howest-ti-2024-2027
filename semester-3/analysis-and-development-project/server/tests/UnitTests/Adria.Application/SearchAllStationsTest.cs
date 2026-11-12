using Adria.Application.Actions;
using Adria.Application.Contracts.Data;
using UnitTests.Mocks;
using MockAllStationsQuery = UnitTests.Mocks.Queries.MockAllStationsQuery;

namespace UnitTests.Adria.Application;

public class SearchAllStationsTest 
{
    [Fact]
    public async Task ExecuteCorrect()
    {
        List<StationDto> stations = [
            new StationDto(Guid.NewGuid(), "Romeo", new CoordinatesDto(48.8583D, 2.2923D)),
            new StationDto(Guid.NewGuid(), "Juliet", new CoordinatesDto(51.0263888889D, 2.6377777778D))
        ];
        
        var logger = new MockLogger<SearchAllStations>();
        var query = new MockAllStationsQuery();
        
        stations.ForEach(query.Add);
        
        var useCase = new SearchAllStations(query, logger);

        var result = await useCase.Execute();
        Assert.Equal(stations.AsReadOnly(), result);
    }
}