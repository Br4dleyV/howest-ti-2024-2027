namespace Adria.Domain.Models;

public class Station(Guid id, string name, Coordinates coordinates)
{
    public Guid Id { get; private set; } = id;
    public string Name { get; private set; } = name;
    public Coordinates  Coordinates { get; private set; } = coordinates;
}
