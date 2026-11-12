using Adria.Application.Actions;
using Adria.Application.Contracts;
using Adria.Domain.Models;
using Adria.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public static class AddReservationController
{
    public static async Task<IResult> Invoke(
        [FromServices] IUseCase<AddReservationInput, Task<Guid>> action,
        [FromBody] AddReservationBody body
    )
    {
        if (body.UserId == Guid.Empty)
        {
            throw new ArgumentException("UserId cannot be empty");
        }

        if (body.EndStationId == Guid.Empty)
        {
            throw new ArgumentException("EndStationId cannot be empty");
        }

        if (body.PodId == Guid.Empty)
        {
            throw new ArgumentException("PodId cannot be empty");
        }

        AddReservationInput input = new(
            UserId: body.UserId,
            StartLocation: body.StartLocation,
            EndStationId: body.EndStationId,
            PodId: body.PodId
        );

        Guid reservationId = await action.Execute(input);

        return TypedResults.Ok(new ReservationResp(reservationId));
    }
}

public sealed record AddReservationBody(
    Guid UserId,
    Coordinates StartLocation,
    Guid EndStationId,
    Guid PodId
);