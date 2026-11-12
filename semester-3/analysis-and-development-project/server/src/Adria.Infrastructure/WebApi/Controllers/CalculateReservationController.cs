using Adria.Application.Actions;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Models;
using Adria.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using CoordInput = Adria.Application.Actions.CoordInput;

namespace Adria.Infrastructure.WebApi.Controllers;

public static class CalculateReservationController
{
    public static async Task<Ok<CalculateReservationResp>> Invoke(
        [FromServices] IUseCase<CalculateReservationInput, Task<CalculateReservationDto>> action,
        [FromQuery(Name = "endStationId")] Guid endStationId,
        [FromQuery(Name = "latitude")] double lat,
        [FromQuery(Name = "longitude")] double lng
    )
    {
        CalculateReservationInput input = new(
            new CoordInput(lat, lng),
            endStationId
        );

        CalculateReservationDto result = await action.Execute(input);
        return TypedResults.Ok(ToResponse(result));
    }

    private static CalculateReservationResp ToResponse(CalculateReservationDto dto)
    {
        return new CalculateReservationResp(
            dto.distance,
            dto.pods.Select(pod => new PodWithEtaResp(
                pod.Id,
                pod.Name,
                pod.IsActive,
                pod.Capacity,
                pod.MaxCapacity,
                new Coordinates(pod.CurrentLocation.Latitude, pod.CurrentLocation.Longitude),
                pod.Eta
            )).ToList()
        );
    }
}