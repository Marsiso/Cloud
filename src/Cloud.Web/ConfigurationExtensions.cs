namespace Cloud.Web;

using System.Globalization;
using Cloud.Application.ViewModels.Shared;
using Cloud.Data;
using Microsoft.AspNetCore.Localization;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddSqliteDatabaseSession(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddOptions<DataContextOptions>()
            .Bind(configuration.GetSection(DataContextOptions.SectionName))
            .ValidateOnStart();

        var contextOptions = configuration.GetSection(DataContextOptions.SectionName).Get<DataContextOptions>();

        ArgumentNullException.ThrowIfNull(contextOptions);

        services.AddDbContext<DataContext>(options =>
        {
            var fullPath = Path.GetFullPath(contextOptions.Source);
            var pathExists = Path.IsPathRooted(fullPath) && Path.HasExtension(fullPath);

            if (!pathExists)
            {
                throw new InvalidOperationException();
            }

            var connectionStringBase = $"Data Source={contextOptions.Source};";

            var connectionString = new SqliteConnectionStringBuilder(connectionStringBase)
            {
                Mode = SqliteOpenMode.ReadWriteCreate
            }.ToString();

            options.UseSqlite(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
                .EnableDetailedErrors(environment.IsDevelopment())
                .EnableSensitiveDataLogging(environment.IsDevelopment());
        });

        return services;
    }

    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddScoped<MainLayoutViewModel>()
            .AddScoped<CultureSelectorViewModel>();

        return services;
    }

    public static WebApplication UseSqliteInitializer(this WebApplication application)
    {
        var services = application.Services;
        var environment = application.Environment;

        using var serviceScope = services.CreateScope();

        using var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();

        if (environment.IsDevelopment())
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
        else
        {
            context.Database.EnsureCreated();
        }

        return application;
    }

    public static WebApplication UseLocalizationResources(this WebApplication application)
    {
        var services = application.Services;

        var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("cs") };

        application.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });

        return application;
    }
}
