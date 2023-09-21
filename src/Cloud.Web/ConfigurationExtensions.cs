using System.Globalization;
using Cloud.Application.Validations;
using Cloud.Core.Application.Users.Queries;
using Cloud.Domain.Utilities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Localization;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Cloud.Web;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddSqliteDatabaseSession(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
    {
        services.AddScoped<Auditor>();
        services.AddScoped<ISaveChangesInterceptor, Auditor>();

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

    public static IServiceCollection AddPasswordHasher(this IServiceCollection services)
    {
        services.AddOptions<PasswordHasherOptions>()
            .ValidateOnStart();

        services.AddSingleton<PasswordHasher>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        return services;
    }

    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddScoped<MainLayoutViewModel>();
        services.AddScoped<CultureSelectorViewModel>();

        return services;
    }

    public static IServiceCollection AddCqrs(this IServiceCollection services)
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetUserQuery).Assembly));
        services.AddValidatorsFromAssembly(typeof(FluentValidationPipelineBehaviour<,>).Assembly, ServiceLifetime.Singleton);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationPipelineBehaviour<,>));

        return services;
    }

    public static WebApplication UseSqliteInitializer(this WebApplication app)
    {
        var services = app.Services;
        var env = app.Environment;

        using var serviceScope = services.CreateScope();

        using var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();

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

    public static WebApplication UseLocalizationResources(this WebApplication app)
    {
        var services = app.Services;

        var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("cs") };

        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });

        return app;
    }
}
