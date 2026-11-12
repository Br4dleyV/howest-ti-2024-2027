using Adria.Domain.Models;
using Adria.Domain.Repositories;

namespace UnitTests.Mocks.Repositories;

public class MockUserRepository : IUserRepository
{
    private readonly List<User> _users = [];

    public void AddUser(User user)
    {
        _users.Add(user);
    }

    public Task<User?> ById(Guid userId)
    {
        return Task.FromResult(_users.FirstOrDefault(u => u.Id == userId));
    }
}