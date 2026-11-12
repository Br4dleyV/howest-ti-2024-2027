using Testcontainers.PostgreSql;
using Microsoft.EntityFrameworkCore;
using ReadingList.Api.Models;

namespace ReadingList.IntegrationTests;

public class TitlePersistenceTests
{
    [Fact]
    public async Task Title_CanBeSavedReloadedAndFinished()
    {
        // Arrange
        /// Setup a PostgreSQL container for testing
        await using var postgresContainer = new PostgreSqlBuilder("postgres:17.6-alpine").Build();

        /// Cancellation token to ensure the test does not run indefinitely (if container doesn't start in 2 minutes, the test will fail)
        using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(2));

        /// Start the PostgreSQL container
        await postgresContainer.StartAsync(timeout.Token);

        /// Configure the DbContext to use the PostgreSQL container's connection string
        /// GetConnectionString gives the address, dynamic port and credentials for the PostgreSQL container
        /// UseNpgsql is the method to configure the DbContext to use PostgreSQL
        var options = new DbContextOptionsBuilder<ReadingListDbContext>()
            .UseNpgsql(postgresContainer.GetConnectionString())
            .Options;

        /// Create the database schema in the PostgreSQL container
        /// await using var db means that the db context will be disposed of asynchronously when it goes out of scope
        await using var db = new ReadingListDbContext(options);

        // Act
        /// Apply any pending migrations to the database
        await db.Database.MigrateAsync(timeout.Token);

        // Assert
        /// Check that the Titles table is empty after migration
        Assert.Empty(await db.Titles.ToListAsync(timeout.Token));

        // Act
        /// Create a new TitleItem and add it to the database
        TitleItem title = TitleItem.Create("Spider-Man: Homecoming");
        db.Titles.Add(title);
        await db.SaveChangesAsync(timeout.Token);

        /// Now, we will read the title back from the database to verify it was saved correctly
        await using var readDb = new ReadingListDbContext(options);
        var savedTitle = await readDb.Titles.SingleAsync(timeout.Token);

        // Assert
        Assert.Equal(title.Id, savedTitle.Id);
        Assert.Equal("Spider-Man: Homecoming", savedTitle.Title);
        Assert.False(savedTitle.IsRead);

        // Act
        /// Mark the title as read and save the changes
        savedTitle.Finish();
        await readDb.SaveChangesAsync(timeout.Token);

        /// Read the title back again to verify that it was marked as read
        await using var verifyDb = new ReadingListDbContext(options);
        var finishedTitle = await verifyDb.Titles.SingleAsync(timeout.Token);

        // Assert
        Assert.Equal(title.Id, finishedTitle.Id);
        Assert.Equal("Spider-Man: Homecoming", finishedTitle.Title);
        Assert.True(finishedTitle.IsRead);
    }
}