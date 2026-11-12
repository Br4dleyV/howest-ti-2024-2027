using Adria.Domain.Models;

namespace Adria.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> ById(Guid userId);
}