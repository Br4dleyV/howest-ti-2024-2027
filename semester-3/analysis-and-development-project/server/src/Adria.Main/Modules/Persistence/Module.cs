using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Domain.Repositories;
using Adria.Infrastructure.Persistence.Queries;
using Adria.Infrastructure.Persistence.Repositories;

namespace Adria.Main.Modules.Persistence;

public static class PersistenceModule
{
    private static string _connectionString = string.Empty;
    
    public static IServiceCollection AddPersistenceModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        _connectionString = configuration["Persistence:ConnectionString"]!;
        
        return services
            .AddAdoServices(configuration)
            .AddRepositories()
            .AddQueries();
    }

    public static WebApplication UsePersistenceModule(this WebApplication app)
    {
        return app.ApplyMigrations();
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services
    )
    {
        // Configure repositories here.
        return services
            .AddScoped<IUserRepository, AdoUserRepository>(serviceProvider => new AdoUserRepository(
                serviceProvider.GetRequiredService<DbProviderFactory>(),
                _connectionString,
                serviceProvider.GetRequiredService<ILogger<AdoUserRepository>>()
            ))
            .AddScoped<IStationRepository, AdoStationRepository>(serviceProvider => new AdoStationRepository(
                serviceProvider.GetRequiredService<DbProviderFactory>(),
                _connectionString,
                serviceProvider.GetRequiredService<ILogger<AdoStationRepository>>()
            ))
            .AddScoped<IPodRepository, AdoPodRepository>(serviceProvider => new AdoPodRepository(
                serviceProvider.GetRequiredService<DbProviderFactory>(),
                _connectionString,
                serviceProvider.GetRequiredService<ILogger<AdoPodRepository>>()
            ))
            .AddScoped<IReservationRepository, AdoReservationRepository>(serviceProvider => new AdoReservationRepository(
                serviceProvider.GetRequiredService<DbProviderFactory>(),
                _connectionString,
                serviceProvider.GetRequiredService<ILogger<AdoReservationRepository>>()
            ));
    }

    private static IServiceCollection AddQueries(
        this IServiceCollection services
    )
    {
        return services
            .AddScoped<IAllStationsQuery, AllStationsQuery>(serviceProvider => new AllStationsQuery(
                serviceProvider.GetRequiredService<DbProviderFactory>(),
                _connectionString,
                serviceProvider.GetRequiredService<ILogger<AllStationsQuery>>()
            )).AddScoped<IAllPodsQuery, AllPodsQuery>(serviceProvider => new AllPodsQuery(
                serviceProvider.GetRequiredService<DbProviderFactory>(),
                _connectionString,
                serviceProvider.GetRequiredService<ILogger<AllPodsQuery>>()
            ));
    }

    private static IServiceCollection AddAdoServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        string provider = configuration["Persistence:Provider"]!;

        DbProviderFactories.RegisterFactory(provider, MySql.Data.MySqlClient.MySqlClientFactory.Instance);

        return services.AddScoped(serviceProvider => DbProviderFactories.GetFactory(provider));
    }
    
    private static WebApplication ApplyMigrations(this WebApplication app)
    {
        IServiceProvider serviceProvider = app.Services.CreateScope().ServiceProvider;
        DbProviderFactory factory = serviceProvider.GetRequiredService<DbProviderFactory>();
        string connectionString = serviceProvider.GetRequiredService<IConfiguration>()["Persistence:ConnectionString"]!;

        using DbConnection connection = factory.CreateConnection()!;
        connection.ConnectionString = connectionString;
        connection.Open();

        string scriptPath = Path.Combine(
            AppContext.BaseDirectory,
            "Persistence/Scripts",
            "create_database.sql"
        );

        using DbCommand command = connection.CreateCommand();
        command.CommandText = File.ReadAllText(scriptPath);
        command.ExecuteNonQuery();
        return app;
    }
}
