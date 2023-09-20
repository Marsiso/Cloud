﻿using Cloud.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Cloud.Web;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddSqliteDatabaseSession(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
    {
        services
            .AddOptions<DataContextOptions>()
            .Bind(config.GetSection(DataContextOptions.SectionName))
            .ValidateOnStart();

        var contextOptions = config.GetSection(DataContextOptions.SectionName).Get<DataContextOptions>();

        ArgumentNullException.ThrowIfNull(contextOptions);

        services.AddDbContext<DataContext>(options =>
        {
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            options.UseSqlite();

            var fullpath = Path.GetFullPath(contextOptions.Source);

            var pathExists =
                Path.IsPathRooted(fullpath)
                && Path.HasExtension(fullpath);

            if (!pathExists) throw new InvalidOperationException();

            var connectionStringBase = $"Data Source={contextOptions.Source};";

            var connectionString = new SqliteConnectionStringBuilder(connectionStringBase)
            {
                Mode = SqliteOpenMode.ReadWriteCreate
            }.ToString();

            options.UseSqlite(connectionString);

            if (env.IsDevelopment())
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        });

        return services;
    }

    public static WebApplication UseSqliteInitializer(this WebApplication app)
    {
        var services = app.Services;
        var env = app.Environment;

        using var serviceScope = services.CreateScope();

        var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();

        if (env.IsDevelopment())
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
        else
        {
            context.Database.EnsureCreated();
        }

        return app;
    }
}