using System.Data.Common;
using Adria.Domain.Models;
using Adria.Domain.Repositories;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public class AdoPodRepository(
    DbProviderFactory factory,
    string connectionString,
    ILogger<AdoPodRepository> logger
)
    : AbstractAdoRepository(factory, connectionString), IPodRepository
{
    private static readonly string TABLE_PODS = "pods";

    //
    // original columns
    //

    private static readonly string COL_ID = "id";
    private static readonly string COL_NAME = "name";
    private static readonly string COL_CAPACITY = "capacity";
    private static readonly string COL_MAX_CAPACITY = "max_capacity";
    private static readonly string COL_IS_ACTIVE = "is_active";
    private static readonly string COL_CURRENT_LOCATION = "current_location";

    //
    // result columns
    //

    private static readonly string ALIAS_POD_LAT = "pod_lat";
    private static readonly string ALIAS_POD_LON = "pod_lon";

    private static readonly string SELECT_POD_BY_ID = $@"
        SELECT
            p.{COL_ID},
            p.{COL_NAME},
            p.{COL_IS_ACTIVE},
            p.{COL_CAPACITY},
            p.{COL_MAX_CAPACITY},
            ST_Y(p.{COL_CURRENT_LOCATION}) AS {ALIAS_POD_LAT},
            ST_X(p.{COL_CURRENT_LOCATION}) AS {ALIAS_POD_LON}
        FROM {TABLE_PODS} p
        WHERE p.{COL_ID} = @Id;
    ";

    public async Task<Pod?> ById(Guid podId)
    {
        DbParameter id = CreateParameter("@Id", podId.ToByteArray());
        DbDataReader dbDataReader = await ExecuteReaderAsync(SELECT_POD_BY_ID, [id]);

        try
        {
            if (await dbDataReader.ReadAsync())
            {
                return new Pod(
                    dbDataReader.GetGuid(dbDataReader.GetOrdinal(COL_ID)),
                    dbDataReader.GetString(dbDataReader.GetOrdinal(COL_NAME)),
                    dbDataReader.GetBoolean(dbDataReader.GetOrdinal(COL_IS_ACTIVE)),
                    dbDataReader.GetInt32(dbDataReader.GetOrdinal(COL_CAPACITY)),
                    dbDataReader.GetInt32(dbDataReader.GetOrdinal(COL_MAX_CAPACITY)),
                    new Coordinates(
                        dbDataReader.GetDouble(dbDataReader.GetOrdinal(ALIAS_POD_LAT)),
                        dbDataReader.GetDouble(dbDataReader.GetOrdinal(ALIAS_POD_LON))
                    )
                );
            }

            return null;
        }
        catch (DbException ex)
        {
            logger.LogError(ex, "Failed to read pod with ID {PodId} from database.", podId);
            throw new AdriaDatabaseException("Failed to read pod from database.", ex);
        }
        finally
        {
            logger.LogInformation("Disposing DbDataReader for pod with ID {PodId}.", podId);
            await dbDataReader.DisposeAsync();
        }
    }
}