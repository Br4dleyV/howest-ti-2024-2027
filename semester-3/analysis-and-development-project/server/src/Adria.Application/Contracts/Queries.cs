using Adria.Application.Contracts.Data;

namespace Adria.Application.Contracts;

public interface IAllStationsQuery
{
    Task<IReadOnlyCollection<StationDto>> Fetch();
}

public interface IAllPodsQuery
{
    Task<IReadOnlyCollection<PodDto>> Fetch();
}