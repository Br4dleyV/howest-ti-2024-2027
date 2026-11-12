using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Actions;

public class SearchAllStations(IAllStationsQuery allStationsQuery, ILogger<SearchAllStations> logger)
    : IUseCase<Task<IReadOnlyCollection<StationDto>>>
{
    public Task<IReadOnlyCollection<StationDto>> Execute()
    {
        logger.LogInformation("Fetching all stations");

        return allStationsQuery.Fetch();
    }
}