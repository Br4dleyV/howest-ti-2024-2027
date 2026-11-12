namespace Adria.Domain.Models;

public class User(Guid id, string firstName, string lastName, DateTime dateOfBirth, Role role)
{
    public Guid Id { get; private set; } = id;
    public string FirstName { get; private set; } = firstName;
    public string LastName { get; private set; } = lastName;
    public DateTime DateOfBirth { get; private set; } = dateOfBirth;
    public Role Role { get; private set; } = role;
}