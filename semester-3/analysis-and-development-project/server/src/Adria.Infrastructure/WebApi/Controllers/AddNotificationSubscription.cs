using Adria.Application.Contracts;
using Adria.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public static class AddNotificationSubscription
{
    public static async Task<Results<Ok<SubscriptionResponse>, BadRequest>> Invoke(
        [FromServices] ISubscriptionStore store,
        [FromBody] SubscriptionRequest request
    )
    {
        await store.AddAsync(request.Endpoint, request.P256dh, request.Auth);

        return TypedResults.Ok(new SubscriptionResponse(request.Endpoint));
    }
}

public sealed record SubscriptionRequest(
    string Endpoint,
    string P256dh,
    string Auth
);