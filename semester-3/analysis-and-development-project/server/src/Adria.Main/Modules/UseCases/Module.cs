using Adria.Application.Actions;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Models;

namespace Adria.Main.Modules.UseCases;

public static class UseCases
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<IUseCase<Task<IReadOnlyCollection<StationDto>>>, SearchAllStations>()
            .AddScoped<IUseCase<SearchAllPodsInput, Task<IReadOnlyCollection<PodDto>>>, SearchAllPods>()
            .AddScoped<IUseCase<CalculateReservationInput, Task<CalculateReservationDto>>, CalculateReservation>()
            .AddScoped<IUseCase<AddReservationInput, Task<Guid>>, AddReservation>()
            // Adria Shield
            .AddScoped<IUseCase<SendWeatherAlertInput, Task>, SendWeatherAlert>();
    }
}