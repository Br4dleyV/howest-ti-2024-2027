using Adria.Application.Actions;
using Adria.Application.Contracts;
using Adria.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public static class AddWeatherAlertController
{
    public static async Task<IResult> Invoke(
        [FromServices] IUseCase<SendWeatherAlertInput, Task> action,
        [FromBody] ExternalAlertBody body
    )
    {
        SendWeatherAlertInput input = new(
            Subject: body.Subject,
            Zone: body.Zone,
            Message: body.Message
        );
        
        await action.Execute(input);

        return Results.Ok();
    }
}

public sealed record ExternalAlertBody(
    string Subject,
    Coordinates Zone,
    string Message
);