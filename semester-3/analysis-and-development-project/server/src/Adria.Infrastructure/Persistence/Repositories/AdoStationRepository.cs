using System.Data.Common;
using Adria.Domain.Models;
using Adria.Domain.Repositories;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public class AdoStationRepository(
    DbProviderFactory factory,
    string connectionString,
    ILogger<AdoStationRepository> logger
)
    : AbstractAdoRepository(factory, connectionString), IStationRepository
{
    private static readonly string TABLE_STATIONS = "stations";

    //
    // original columns
    //

    private static readonly string COL_ID = "id";
    private static readonly string COL_NAME = "name";
    private static readonly string COL_COORDS = "coords";

    //
    // result columns
    //

    private static readonly string ALIAS_STATION_LAT = "station_lat";
    private static readonly string ALIAS_STATION_LON = "station_lon";

    private static readonly string SELECT_STATION_BY_ID = $@"
        SELECT
            s.{COL_ID},
            s.{COL_NAME},
            ST_Y(s.{COL_COORDS}) AS {ALIAS_STATION_LAT},
            ST_X(s.{COL_COORDS}) AS {ALIAS_STATION_LON}
        FROM {TABLE_STATIONS} s
        WHERE s.{COL_ID} = @Id;
    ";

    public async Task<Station?> ById(Guid stationId)
    {
        DbParameter id = CreateParameter("@Id", stationId.ToByteArray());
        DbDataReader dbDataReader = await ExecuteReaderAsync(SELECT_STATION_BY_ID, [id]);

        try
        {
            if (await dbDataReader.ReadAsync())
            {
                return new Station(
                    dbDataReader.GetGuid(dbDataReader.GetOrdinal(COL_ID)),
                    dbDataReader.GetString(dbDataReader.GetOrdinal(COL_NAME)),
                    new Coordinates(
                        dbDataReader.GetDouble(dbDataReader.GetOrdinal(ALIAS_STATION_LAT)),
                        dbDataReader.GetDouble(dbDataReader.GetOrdinal(ALIAS_STATION_LON))
                    )
                );
            }

            return null;
        }
        catch (DbException ex)
        {
            logger.LogError(ex, "Failed to read station with ID {StationId} from database.", stationId);
            throw new AdriaDatabaseException("Failed to read station from database.", ex);
        }
        finally
        {
            logger.LogInformation("Disposing DbDataReader for station with ID {StationId}.", stationId);
            await dbDataReader.DisposeAsync();
        }
    }
}