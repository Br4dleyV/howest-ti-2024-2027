using Adria.Domain.Models;

namespace Adria.Infrastructure.WebApi.Controllers.Responses;

public sealed record PodWithEtaResp
(
    Guid Id,
    string Name,
    bool IsActive,
    int Capacity,
    int MaxCapacity,
    Coordinates CurrentLocation,
    DateTime Eta
);