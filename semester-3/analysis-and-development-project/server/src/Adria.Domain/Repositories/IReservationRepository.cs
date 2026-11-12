using Adria.Domain.Models;

namespace Adria.Domain.Repositories;

public interface IReservationRepository
{
    Task<Reservation?> ById(Guid reservationId);
    Task Save(Reservation reservation);
    Task<IReadOnlyCollection<Reservation>> ByUserId(Guid userId);
}