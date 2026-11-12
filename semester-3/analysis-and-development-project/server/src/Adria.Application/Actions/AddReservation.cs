using Adria.Application.Contracts;
using Adria.Domain.Models;
using Adria.Domain.Repositories;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Actions;

public sealed record AddReservationInput(
    Guid UserId,
    Coordinates StartLocation,
    Guid EndStationId,
    Guid PodId
);

public sealed class AddReservation(
    IReservationRepository reservationRepository,
    IUserRepository userRepository,
    IStationRepository stationRepository,
    IPodRepository podRepository,
    ILogger<AddReservation> logger
)
    : IUseCase<AddReservationInput, Task<Guid>>
{
    public async Task<Guid> Execute(AddReservationInput input)
    {
        User user = await userRepository.ById(input.UserId) ??
            throw new ElementNotFoundException($"User with ID {input.UserId} not found");

        Station endStation = await stationRepository.ById(input.EndStationId) ??
            throw new ElementNotFoundException($"Station with ID {input.EndStationId} not found.");

        Pod pod = await podRepository.ById(input.PodId) ??
            throw new ElementNotFoundException($"Pod with ID {input.PodId} not found.");

        Reservation reservation = new Reservation(
            user.Id,
            input.StartLocation,
            endStation,
            pod
        );

        await reservationRepository.Save(reservation);
        
        logger.LogInformation(
            "New reservation created with ID {ReservationId} for user {UserId}",
            reservation.Id,
            user.Id
        );
        
        return reservation.Id;
    }
}