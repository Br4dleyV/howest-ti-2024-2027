namespace Adria.Application.Contracts.Data;

public record PodWithEtaDto(
    Guid Id,
    string Name,
    bool IsActive,
    int Capacity,
    int MaxCapacity,
    CoordinatesDto CurrentLocation,
    DateTime Eta
);