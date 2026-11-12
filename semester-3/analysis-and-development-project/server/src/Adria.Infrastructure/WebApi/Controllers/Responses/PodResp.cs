using Adria.Domain.Models;

namespace Adria.Infrastructure.WebApi.Controllers.Responses;

public sealed record PodResp(
    Guid Id,
    string Name,
    bool IsActive,
    int Capacity,
    int MaxCapacity,
    Coordinates CurrentLocation
);