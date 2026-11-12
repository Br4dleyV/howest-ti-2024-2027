namespace Adria.Infrastructure.WebApi.Controllers.Responses;


public record CalculateReservationResp(double distance, IReadOnlyCollection<PodWithEtaResp> pods);