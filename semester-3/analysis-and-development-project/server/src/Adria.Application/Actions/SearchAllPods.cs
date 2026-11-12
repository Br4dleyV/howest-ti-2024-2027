using Microsoft.Extensions.Logging;

using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;

namespace Adria.Application.Actions;

public record SearchAllPodsInput(double? Lat, double? Lng);

public class SearchAllPods(IAllPodsQuery allPodsQuery, ILogger<SearchAllPods> logger)
    : IUseCase<SearchAllPodsInput, Task<IReadOnlyCollection<PodDto>>>
{
    public Task<IReadOnlyCollection<PodDto>> Execute(SearchAllPodsInput input)
    {
        logger.LogInformation("Fetching all pods");

        return allPodsQuery.Fetch();
    }
}