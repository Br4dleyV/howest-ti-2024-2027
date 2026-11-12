namespace Adria.Infrastructure.WebApi.Controllers.Responses;

public sealed record StationResp(Guid Id, string Name, CoordResp Coordinates);