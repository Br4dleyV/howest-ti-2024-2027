using Adria.Domain.Models;
using Adria.Domain.Repositories;

namespace UnitTests.Mocks.Repositories;

public class MockReservationRepository : IReservationRepository
{
    public readonly IList<Reservation> Reservations = new List<Reservation>();

    public Task<Reservation?> ById(Guid reservationId)
    {
        throw new NotImplementedException();
    }

    public Task Save(Reservation reservation)
    {
        Reservations.Add(reservation);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<Reservation>> ByUserId(Guid userId)
    {
        throw new NotImplementedException();
    }
}