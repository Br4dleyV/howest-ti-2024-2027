// Initialize Aspire application builder
var builder = DistributedApplication.CreateBuilder(args);

// Register Postgres database with Aspire
var readingListDatabase = builder
    .AddPostgres("postgres")
    .AddDatabase("reading-list-db");

// Register ReadingList.Api project with Aspire and assign service name
var readingList = builder.AddProject<Projects.ReadingList_Api>("reading-list-api")
    .WithReference(readingListDatabase) // Tells Aspire that ReadingList.Api depends on Postgres, so Aspire automatically supplies database connection information and injects environment variables so ReadingList.Api can find Postgres without hardcoding the URL
    .WaitFor(readingListDatabase)
    .WithHttpHealthCheck("/health/ready");

// Register ReadingSummary.Api project with Aspire and assign service name, and reference ReadingList.Api project
var readingSummary = builder.AddProject<Projects.ReadingSummary_Api>("reading-summary-api")
    .WithReference(readingList) // Tells Aspire that ReadingSummary.Api depends on ReadingList.Api, so Aspire automatically supplies service-discovery endpoint configuration and injects environment variables so ReadingSummary.Api can find ReadingList.Api without hardcoding the URL
    .WaitFor(readingList)
    .WithHttpHealthCheck("/health/ready"); // Tells Aspire to check the /health/ready endpoint for a healthy response before considering ReadingSummary.Api healthy (also used in tests to wait for the service to be ready before sending requests)

// Build and start Aspire orchestration host
builder.Build().Run();
