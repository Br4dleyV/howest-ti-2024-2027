using System.Data.Common;
using Adria.Domain.Models;
using Adria.Domain.Repositories;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public class AdoUserRepository(
    DbProviderFactory factory,
    string connectionString,
    ILogger<AdoUserRepository> logger
)
    : AbstractAdoRepository(factory, connectionString), IUserRepository
{
    private static readonly string TABLE_USERS = "users";

    //
    // schema columns
    //

    private static readonly string COL_ID = "id";
    private static readonly string COL_FIRSTNAME = "firstname";
    private static readonly string COL_LASTNAME = "lastname";
    private static readonly string COL_DOB = "dob";
    private static readonly string COL_ROLE = "role";

    private static readonly string SELECT_USER_BY_ID = $@"
        SELECT
            {COL_ID},
            {COL_FIRSTNAME},
            {COL_LASTNAME},
            {COL_DOB},
            {COL_ROLE}
        FROM {TABLE_USERS}
        WHERE {COL_ID} = @Id;
    ";

    public async Task<User?> ById(Guid userId)
    {
        DbParameter id = CreateParameter("@Id", userId.ToByteArray());
        DbDataReader dbDataReader = await ExecuteReaderAsync(SELECT_USER_BY_ID, [id]);

        try
        {
            if (await dbDataReader.ReadAsync())
            {
                return new User(
                    dbDataReader.GetGuid(dbDataReader.GetOrdinal(COL_ID)),
                    dbDataReader.GetString(dbDataReader.GetOrdinal(COL_FIRSTNAME)),
                    dbDataReader.GetString(dbDataReader.GetOrdinal(COL_LASTNAME)),
                    dbDataReader.GetDateTime(dbDataReader.GetOrdinal(COL_DOB)),
                    Enum.Parse<Role>(dbDataReader.GetString(dbDataReader.GetOrdinal(COL_ROLE)), ignoreCase: true)
                );
            }

            return null;
        }
        catch (DbException ex)
        {
            logger.LogError(ex, "Failed to read user with ID {UserId} from database.", userId);
            throw new AdriaDatabaseException("Failed to read user from database.", ex);
        }
        finally
        {
            logger.LogInformation("Disposing DbDataReader for user with ID {UserId}.", userId);
            await dbDataReader.DisposeAsync();
        }
    }
}