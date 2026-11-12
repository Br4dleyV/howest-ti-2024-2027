using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public class AllStationsQuery(
    DbProviderFactory factory,
    string connectionString,
    ILogger<AllStationsQuery> logger)
    : IAllStationsQuery
{
    private readonly DbProviderFactory _factory = factory;
    private readonly string _connectionString = connectionString;
    private readonly ILogger<AllStationsQuery> _logger = logger;

    private const string Query = "SELECT id, name, ST_X(coords) as lat, ST_Y(coords) as lng FROM stations";


    public async Task<IReadOnlyCollection<StationDto>> Fetch()
        {
            _logger.LogInformation("Starting to fetch all stations");

            await using var connection = _factory.CreateConnection()
                                   ?? throw new InvalidOperationException("DbProviderFactory returned a null DbConnection.");

            connection.ConnectionString = _connectionString;
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = Query;

            await using var reader = await command.ExecuteReaderAsync();
            
            var stations = new List<StationDto>();
            
            var idOrd = reader.GetOrdinal("id");
            var nameOrd = reader.GetOrdinal("name");
            var lngOrd = reader.GetOrdinal("lng");
            var latOrd = reader.GetOrdinal("lat");
            
            while (await reader.ReadAsync())
            {
                stations.Add(new StationDto(
                    Id: reader.GetGuid(idOrd),
                    Name: reader.GetString(nameOrd),
                    Coordinates: new CoordinatesDto(reader.GetDouble(latOrd), reader.GetDouble(lngOrd))
                ));
            }

            _logger.LogInformation("Fetched {Count} stations", stations.Count);

            return stations.AsReadOnly();
        }   
}