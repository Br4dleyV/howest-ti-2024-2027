using Adria.Application.Actions;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Models;
using Adria.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public static class SearchAllPodsController
{
    public static async Task<Results<Ok<List<PodResp>>, BadRequest<string>, NotFound>> Invoke(
        [FromServices] IUseCase<SearchAllPodsInput, Task<IReadOnlyCollection<PodDto>>> searchAllPods,
        [FromQuery(Name = "latitude")] double? lat,
        [FromQuery(Name = "longitude")] double? lng
    )
    {
        if (lat.HasValue ^ lng.HasValue)
        {
            return TypedResults.BadRequest("latitude and longitude either have to be both set or unset");
        }

        SearchAllPodsInput input = new(lat, lng);
        
        IReadOnlyCollection<PodDto> podData = await searchAllPods.Execute(input);

        return TypedResults.Ok(ToResponse(podData));
    }

    private static List<PodResp> ToResponse(IReadOnlyCollection<PodDto> podData)
    {
        return podData.Select(data => new PodResp(
            Id: data.Id,
            Name: data.Name,
            IsActive: data.IsActive,
            Capacity: data.Capacity,
            MaxCapacity: data.MaxCapacity,
            CurrentLocation: new Coordinates(data.CurrentLocation.Latitude, data.CurrentLocation.Longitude)
        )).ToList();
    }
}
