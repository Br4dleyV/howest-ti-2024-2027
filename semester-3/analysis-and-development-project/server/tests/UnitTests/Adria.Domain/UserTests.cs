using Adria.Domain.Models;

namespace UnitTests.Adria.Domain;

public sealed class UserTests
{
    [Fact]
    public void Constructor_WithAllParameters_CreatesUser()
    {
        // Arrange
        var id = Guid.NewGuid();
        var firstName = "John";
        var lastName = "Doe";
        var dob = new DateTime(1990, 1, 1);
        var role = Role.Admin;

        // Act
        var user = new User(id, firstName, lastName, dob, role);

        // Assert
        Assert.Equal(id, user.Id);
        Assert.Equal(firstName, user.FirstName);
        Assert.Equal(lastName, user.LastName);
        Assert.Equal(dob, user.DateOfBirth);
        Assert.Equal(role, user.Role);
    }
}