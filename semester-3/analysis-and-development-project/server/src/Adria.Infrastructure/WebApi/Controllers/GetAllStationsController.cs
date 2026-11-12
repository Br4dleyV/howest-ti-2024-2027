using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public static class GetAllStationsController
{
    public static async Task<Results<Ok<List<StationResp>>, NotFound>> Invoke(
        [FromServices] IUseCase<Task<IReadOnlyCollection<StationDto>>> getAllStations)
    {
        IReadOnlyCollection<StationDto> stations = await getAllStations.Execute();
        return TypedResults.Ok(ToResponse(stations));
    }

    private static List<StationResp> ToResponse(IReadOnlyCollection<StationDto> stations)
    {
        return stations.Select(x =>
            new StationResp(
                x.Id, 
                x.Name, 
                new CoordResp(x.Coordinates.Latitude, x.Coordinates.Longitude)
                )).ToList();
    }
}