using System.Data.Common;
using Adria.Domain.Models;
using Adria.Domain.Repositories;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public class AdoReservationRepository(
    DbProviderFactory factory,
    string connectionString,
    ILogger<AdoReservationRepository> logger
)
    : AbstractAdoRepository(factory, connectionString), IReservationRepository
{
    private static readonly string TABLE_RESERVATIONS = "reservations";
    private static readonly string TABLE_STATIONS = "stations";
    private static readonly string TABLE_PODS = "pods";

    //
    // original columns
    //

    private static readonly string COL_ID = "id";
    private static readonly string COL_NAME = "name";

    private static readonly string COL_USER_ID = "user_id";
    private static readonly string COL_CREATED_AT = "created_at";

    private static readonly string COL_START_LOCATION = "start_location";
    private static readonly string COL_END_STATION = "end_station";
    private static readonly string COL_POD_ID = "pod_id";

    private static readonly string COL_COORDS = "coords";
    private static readonly string COL_CURRENT_LOCATION = "current_location";

    private static readonly string COL_CAPACITY = "capacity";
    private static readonly string COL_MAX_CAPACITY = "max_capacity";
    private static readonly string COL_IS_ACTIVE = "is_active";

    //
    // result columns
    //

    private static readonly string ALIAS_RESERVATION_ID = "reservation_id";
    private static readonly string ALIAS_USER_ID = "user_id";
    private static readonly string ALIAS_CREATED_AT = "created_at";
    private static readonly string ALIAS_START_LAT = "start_lat";
    private static readonly string ALIAS_START_LON = "start_lon";

    private static readonly string ALIAS_STATION_ID = "station_id";
    private static readonly string ALIAS_STATION_NAME = "station_name";
    private static readonly string ALIAS_STATION_LAT = "station_lat";
    private static readonly string ALIAS_STATION_LON = "station_lon";

    private static readonly string ALIAS_POD_ID = "pod_id";
    private static readonly string ALIAS_POD_NAME = "pod_name";
    private static readonly string ALIAS_POD_CAPACITY = "pod_capacity";
    private static readonly string ALIAS_POD_MAX_CAPACITY = "pod_max_capacity";
    private static readonly string ALIAS_POD_IS_ACTIVE = "pod_is_active";
    private static readonly string ALIAS_POD_LAT = "pod_lat";
    private static readonly string ALIAS_POD_LON = "pod_lon";

    private static readonly string INSERT_RESERVATION = $@"
        INSERT INTO {TABLE_RESERVATIONS}
        ({COL_ID}, {COL_USER_ID}, {COL_START_LOCATION}, {COL_END_STATION}, {COL_CREATED_AT}, {COL_POD_ID})
        VALUES
        (@Id, @UserId, POINT(@StartLon, @StartLat), @EndStationId, @CreatedAt, @PodId);
    ";
    
    private static readonly string SELECT_RESERVATION_BASE = $@"
        SELECT
            r.{COL_ID}         AS {ALIAS_RESERVATION_ID},
            r.{COL_USER_ID}    AS {ALIAS_USER_ID},
            r.{COL_CREATED_AT} AS {ALIAS_CREATED_AT},
            ST_Y(r.{COL_START_LOCATION}) AS {ALIAS_START_LAT},
            ST_X(r.{COL_START_LOCATION}) AS {ALIAS_START_LON},

            s.{COL_ID}   AS {ALIAS_STATION_ID},
            s.{COL_NAME} AS {ALIAS_STATION_NAME},
            ST_Y(s.{COL_COORDS}) AS {ALIAS_STATION_LAT},
            ST_X(s.{COL_COORDS}) AS {ALIAS_STATION_LON},

            p.{COL_ID}   AS {ALIAS_POD_ID},
            p.{COL_NAME} AS {ALIAS_POD_NAME},
            p.{COL_CAPACITY} AS {ALIAS_POD_CAPACITY},
            p.{COL_MAX_CAPACITY} AS {ALIAS_POD_MAX_CAPACITY},
            p.{COL_IS_ACTIVE} AS {ALIAS_POD_IS_ACTIVE},
            ST_Y(p.{COL_CURRENT_LOCATION}) AS {ALIAS_POD_LAT},
            ST_X(p.{COL_CURRENT_LOCATION}) AS {ALIAS_POD_LON}
        FROM {TABLE_RESERVATIONS} r
        JOIN {TABLE_STATIONS} s ON r.{COL_END_STATION} = s.{COL_ID}
        JOIN {TABLE_PODS} p ON r.{COL_POD_ID} = p.{COL_ID}
    ";

    private static readonly string SELECT_RESERVATION_BY_ID = $@"
        {SELECT_RESERVATION_BASE}
        WHERE r.{COL_ID} = @Id;
    ";

    private static readonly string SELECT_RESERVATIONS_BY_USER_ID = $@"
        {SELECT_RESERVATION_BASE}
        WHERE r.{COL_USER_ID} = @UserId
        ORDER BY r.{COL_CREATED_AT} DESC;
    ";

    private static Reservation MapReservation(DbDataReader reader)
    {
        var startLocation = new Coordinates(
            reader.GetDouble(reader.GetOrdinal(ALIAS_START_LAT)),
            reader.GetDouble(reader.GetOrdinal(ALIAS_START_LON))
        );

        var station = new Station(
            reader.GetGuid(reader.GetOrdinal(ALIAS_STATION_ID)),
            reader.GetString(reader.GetOrdinal(ALIAS_STATION_NAME)),
            new Coordinates(
                reader.GetDouble(reader.GetOrdinal(ALIAS_STATION_LAT)),
                reader.GetDouble(reader.GetOrdinal(ALIAS_STATION_LON))
            )
        );

        var pod = new Pod(
            reader.GetGuid(reader.GetOrdinal(ALIAS_POD_ID)),
            reader.GetString(reader.GetOrdinal(ALIAS_POD_NAME)),
            reader.GetBoolean(reader.GetOrdinal(ALIAS_POD_IS_ACTIVE)),
            reader.GetInt32(reader.GetOrdinal(ALIAS_POD_CAPACITY)),
            reader.GetInt32(reader.GetOrdinal(ALIAS_POD_MAX_CAPACITY)),
            new Coordinates(
                reader.GetDouble(reader.GetOrdinal(ALIAS_POD_LAT)),
                reader.GetDouble(reader.GetOrdinal(ALIAS_POD_LON))
            )
        );

        return new Reservation(
            reader.GetGuid(reader.GetOrdinal(ALIAS_RESERVATION_ID)),
            reader.GetGuid(reader.GetOrdinal(ALIAS_USER_ID)),
            startLocation,
            station,
            pod,
            reader.GetDateTime(reader.GetOrdinal(ALIAS_CREATED_AT))
        );
    }

    public async Task<Reservation?> ById(Guid reservationId)
    {
        DbParameter id = CreateParameter("@Id", reservationId.ToByteArray());
        DbDataReader dbDataReader = await ExecuteReaderAsync(SELECT_RESERVATION_BY_ID, [id]);

        try
        {
            if (await dbDataReader.ReadAsync())
            {
                return MapReservation(dbDataReader);
            }

            return null;
        }
        catch (DbException ex)
        {
            logger.LogError(ex, "Failed to read reservation {ReservationId} from database.", reservationId);
            throw new AdriaDatabaseException("Failed to read reservation from database.", ex);
        }
        finally
        {
            logger.LogInformation("Disposing DbDataReader for reservation {ReservationId}.", reservationId);
            await dbDataReader.DisposeAsync();
        }
    }

    public async Task Save(Reservation reservation)
    {
        logger.LogInformation("Saving reservation {ReservationId} to database.", reservation.Id);

        DbParameter[] parameters =
        [
            CreateParameter("@Id", reservation.Id.ToByteArray()),
            CreateParameter("@UserId", reservation.UserId.ToByteArray()),
            CreateParameter("@StartLon", reservation.StartLocation.Longitude),
            CreateParameter("@StartLat", reservation.StartLocation.Latitude),
            CreateParameter("@EndStationId", reservation.EndStation.Id.ToByteArray()),
            CreateParameter("@CreatedAt", reservation.ReservationTime),
            CreateParameter("@PodId", reservation.Pod.Id.ToByteArray())
        ];

        try
        {
            await ExecuteNonQueryAsync(INSERT_RESERVATION, parameters);
        }
        catch (DbException ex)
        {
            logger.LogError(ex, "Failed to save reservation {ReservationId} to database.", reservation.Id);
            throw new AdriaDatabaseException("Failed to save reservation to database.", ex);
        }
    }

    public async Task<IReadOnlyCollection<Reservation>> ByUserId(Guid userId)
    {
        DbParameter id = CreateParameter("@UserId", userId.ToByteArray());
        DbDataReader dbDataReader = await ExecuteReaderAsync(SELECT_RESERVATIONS_BY_USER_ID, [id]);

        var reservations = new List<Reservation>();

        try
        {
            while (await dbDataReader.ReadAsync())
            {
                reservations.Add(MapReservation(dbDataReader));
            }

            return reservations;
        }
        catch (DbException ex)
        {
            logger.LogError(ex, "Failed to read reservations for user {UserId} from database.", userId);
            throw new AdriaDatabaseException("Failed to read reservations for user from database.", ex);
        }
        finally
        {
            logger.LogInformation("Disposing DbDataReader for reservations for user {UserId}.", userId);
            await dbDataReader.DisposeAsync();
        }
    }
}