using Adria.Domain.Models;

namespace Adria.Domain.Repositories;

public interface IStationRepository
{
    Task<Station?> ById(Guid stationId);
}