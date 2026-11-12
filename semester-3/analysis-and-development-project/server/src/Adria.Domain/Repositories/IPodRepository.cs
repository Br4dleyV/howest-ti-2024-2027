using Adria.Domain.Models;

namespace Adria.Domain.Repositories;

public interface IPodRepository
{
    Task<Pod?> ById(Guid podId);
}