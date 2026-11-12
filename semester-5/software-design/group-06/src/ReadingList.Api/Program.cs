using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using ReadingList.Api.Health;
using ReadingList.Api.Models;
using ReadingList.ServiceDefaults;

// Initialize app builder
var builder = WebApplication.CreateBuilder(args);
builder.ConfigureOpenTelemetry(); // Send structured logs and request traces to the Aspire dashboard
builder.Services.AddOpenApi(); // Register OpenAPI support for API documentation
builder.Services.AddHealthChecks()
    .AddCheck<ReadingListReadinessCheck>("reading-list-ready", tags: new[] { "ready" }, timeout: TimeSpan.FromSeconds(5)); // Register health checks for monitoring the app's health
builder.AddNpgsqlDbContext<ReadingListDbContext>("reading-list-db"); // Add PGSQL database context for reading list

// Build the app
var app = builder.Build();

// If app is in developer mode
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Map OpenAPI endpoints for API documentation
}

// Add middleware to redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

//------------------------------------------------------

app.MapHealthChecks("/health");
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false // Exclude all checks, just return 200 OK for liveness probe
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

app.MapPost("/titles", async (CreateTitleRequest request, ReadingListDbContext db) =>
{
    TitleItem newItem;
    try
    {
        newItem = TitleItem.Create(request.Title);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }

    db.Titles.Add(newItem);

    await db.SaveChangesAsync();

    return Results.Created($"/titles/{newItem.Id}", newItem);
}).WithName("CreateTitle");

app.MapGet("/titles", async (ReadingListDbContext db) =>
{
    var titles = await db.Titles.ToListAsync();
    app.Logger.LogInformation("Returned {TitleCount} titles from PostgreSQL.", titles.Count); // Link the database result to the current request trace
    return Results.Ok(titles);
}).WithName("GetTitles");

app.MapPut("/titles/{id:guid}/finish", async (Guid id, ReadingListDbContext db) =>
{
    var titleItem = await db.Titles.FindAsync(id);
    if (titleItem is null)
        return Results.NotFound("Title with the specified ID was not found.");

    try
    {
        titleItem.Finish();
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(ex.Message);
    }

    await db.SaveChangesAsync();
    return Results.Ok(titleItem);
}).WithName("MarkTitleAsRead");

//------------------------------------------------------

using var scope = app.Services.CreateScope(); // Create a scope to resolve services
var db = scope.ServiceProvider.GetRequiredService<ReadingListDbContext>(); // Resolve the database context from the service provider

await db.Database.MigrateAsync(); // Apply any pending migrations to the database

app.Run();

// Requests
record CreateTitleRequest(string Title);
