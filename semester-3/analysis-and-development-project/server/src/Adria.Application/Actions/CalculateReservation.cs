using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Models;
using Adria.Domain.Repositories;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Utils = Adria.Domain.Shared.Utils;

namespace Adria.Application.Actions;

public record CoordInput(double Latitude, double Longitude);

public record CalculateReservationInput(CoordInput StartLocation, Guid EndStation);

public class CalculateReservation(
    IAllPodsQuery allPodsQuery,
    IStationRepository stationRepository,
    ILogger<CalculateReservation> logger
)
    : IUseCase<CalculateReservationInput, Task<CalculateReservationDto>>
{
    public async Task<CalculateReservationDto> Execute(CalculateReservationInput input)
    {
        Station station = await stationRepository.ById(input.EndStation) ??
            throw new ElementNotFoundException($"Station with ID {input.EndStation} not found.");

        logger.LogInformation("Fetching closest pods going to station {EndStationId}, from lat {Latitude} long {Longitude}",
            input.EndStation,
            input.StartLocation.Latitude,
            input.StartLocation.Longitude
        );

        Coordinates startLocation = new Coordinates(input.StartLocation.Latitude, input.StartLocation.Longitude);

        IReadOnlyCollection<PodDto> pods = await allPodsQuery.Fetch();

        PodsList podsList = new PodsList(pods.Select(p => p.ToPod()).ToList());

        IReadOnlyCollection<Pod> closestPods = podsList.GetClosestPodsToLocation(startLocation);

        double distanceToEndStation = Utils.RandomDouble(50, 250);

        IReadOnlyCollection<PodWithEtaDto> closestPodsWithEtas = closestPods.Select(p => new PodWithEtaDto(
            Id: p.Id,
            Name: p.Name,
            IsActive: p.IsActive,
            Capacity: p.Passengers,
            MaxCapacity: p.MaxPassengers,
            CurrentLocation: new CoordinatesDto(p.CurrentLocation.Latitude, p.CurrentLocation.Longitude),
            Eta: p.GetEtaToStation(startLocation, station)
        )).ToList();

        return new CalculateReservationDto(distanceToEndStation, closestPodsWithEtas);
    }
}