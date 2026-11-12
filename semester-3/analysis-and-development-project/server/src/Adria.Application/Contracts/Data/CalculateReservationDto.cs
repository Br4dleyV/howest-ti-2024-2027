namespace Adria.Application.Contracts.Data;

public record CalculateReservationDto(double distance, IReadOnlyCollection<PodWithEtaDto> pods);