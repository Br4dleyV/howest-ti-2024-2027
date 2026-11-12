namespace Adria.Infrastructure.Persistence.Queries;

using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using System.Data.Common;
using Microsoft.Extensions.Logging;

public sealed class AllPodsQuery(
    DbProviderFactory factory,
    string connectionString,
    ILogger<AllPodsQuery> logger
)
    : IAllPodsQuery
{
    private static readonly string QRY = @"
        SELECT id, name, is_active, capacity, max_capacity, ST_X(current_location) as lat, ST_Y(current_location) as lng
        FROM pods;
    ";

    public async Task<IReadOnlyCollection<PodDto>> Fetch()
    {
        logger.LogInformation("Starting to fetch all pods");

        using var connection = factory.CreateConnection()
            ?? throw new InvalidOperationException("DbProviderFactory returned a null DbConnection.");

        connection.ConnectionString = connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = QRY;
        
        using var reader = await command.ExecuteReaderAsync();
        
        var pods = new List<PodDto>();

        while (await reader.ReadAsync())
        {
            var idOrd = reader.GetOrdinal("id");
            var nameOrd = reader.GetOrdinal("name");
            var activeOrd = reader.GetOrdinal("is_active");
            var capacityOrd = reader.GetOrdinal("capacity");
            var maxCapacityOrd = reader.GetOrdinal("max_capacity");
            var latOrd = reader.GetOrdinal("lat");
            var lngOrd = reader.GetOrdinal("lng");
            
            pods.Add(new PodDto(
                Id: reader.GetGuid(idOrd),
                Name: reader.GetString(nameOrd),
                IsActive: reader.GetBoolean(activeOrd),
                Capacity: reader.GetInt32(capacityOrd),
                MaxCapacity: reader.GetInt32(maxCapacityOrd),
                CurrentLocation: new CoordinatesDto(reader.GetDouble(latOrd), reader.GetDouble(lngOrd))
            ));
        }

        logger.LogInformation("Fetched {Count} pods", pods.Count);

        return pods.AsReadOnly();
    }
}
