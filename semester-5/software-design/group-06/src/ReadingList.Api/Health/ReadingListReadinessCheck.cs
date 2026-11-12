using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ReadingList.Api.Health;

public class ReadingListReadinessCheck(ReadingListDbContext db) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        await db.Titles.AsNoTracking().AnyAsync(cancellationToken);

        return HealthCheckResult.Healthy("The titles table is accessible.");
    }

}
