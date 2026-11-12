namespace Adria.Application.Contracts.Data;

public record StationDto(Guid Id, string Name, CoordinatesDto Coordinates);