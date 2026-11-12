using Adria.Application.Actions;
using Adria.Infrastructure.WebApi.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi;

public static class Routes
{
    private static readonly string APPLICATION_JSON = "application/json";

    public static WebApplication MapRoutes(this WebApplication app)
    {
        MapStationsRoutes(app);
        MapPodsRoutes(app);
        MapReservationsRoutes(app);
        MapExternalWeatherAlert(app);

        return app;
    }

    private static void MapStationsRoutes(WebApplication app)
    {
        var stationsRoutes = app.MapGroup("/api/stations")
            .WithTags("Stations")
            .WithOpenApi();

        stationsRoutes.MapGet("", GetAllStationsController.Invoke)
            .WithDescription("Get all stations")
            .WithName(nameof(GetAllStationsController))
            .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
            .WithOpenApi();
    }

    private static void MapPodsRoutes(WebApplication app)
    {
        var podsRoutes = app.MapGroup("/api/pods")
            .WithTags("Pods")
            .WithOpenApi();

        podsRoutes
            .MapGet("", SearchAllPodsController.Invoke)
            .WithDescription("Get all pods.")
            .WithName(nameof(SearchAllPodsController))
            .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
            .WithOpenApi();
    }

    private static void MapReservationsRoutes(WebApplication app)
    {
        var reservationRoutes = app.MapGroup("/api/reservations")
            .WithTags("Reservations")
            .WithOpenApi();

        reservationRoutes
            .MapGet("/calculate", CalculateReservationController.Invoke)
            .WithDescription("Precalculate the reservations")
            .WithName(nameof(CalculateReservation))
            .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
            .WithOpenApi();

        reservationRoutes
            .MapPost("/", AddReservationController.Invoke)
            .WithDescription("Create a new reservation")
            .WithName(nameof(AddReservationController))
            .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
            .WithOpenApi();
    }


    private static void MapExternalWeatherAlert(WebApplication app)
    {
        var adriaShieldRoutes = app.MapGroup("/api/weather-alerts")
            .WithTags("WeatherAlert")
            .WithOpenApi();

        adriaShieldRoutes
            .MapPost("/", AddWeatherAlertController.Invoke)
            .WithDescription("Create a new weather alert")
            .WithName(nameof(AddWeatherAlertController))
            .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
            .WithOpenApi();

        adriaShieldRoutes
            .MapPost("/subscribe", AddNotificationSubscription.Invoke)
            .WithDescription("Create a new weather alert subscription")
            .WithName(nameof(AddNotificationSubscription))
            .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
            .WithOpenApi();
    }
}
