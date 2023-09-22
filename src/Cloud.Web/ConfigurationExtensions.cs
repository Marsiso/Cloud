namespace Cloud.Web;

using System.Globalization;
using Cloud.Application.Hosted;
using Cloud.Application.ViewModels;
using Cloud.Core.Utilities;
using Cloud.Data;
using Cloud.Domain.Utilities;
using Cloud.Domain.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddSqliteDatabaseSession(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddScoped<Auditor>();
        services.AddScoped<ISaveChangesInterceptor, Auditor>();

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
                .UseOpenIddict()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
                .EnableDetailedErrors(environment.IsDevelopment())
                .EnableSensitiveDataLogging(environment.IsDevelopment());
        });

        return services;
    }

    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.Scan(selectors =>
            selectors.FromAssemblyOf<ViewModelBase>()
                .AddClasses(classes => classes.AssignableTo<ViewModelBase>())
                .AsSelf()
                .WithScopedLifetime());

        return services;
    }

    public static IServiceCollection AddUtilities(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IValidator<HashProviderOptions>, HashProviderOptionsValidator>();

        services.AddOptions<HashProviderOptions>()
            .Bind(configuration.GetSection(HashProviderOptions.SectionName))
            .ValidateFluently()
            .ValidateOnStart();

        services.AddSingleton<HashProvider>();
        services.AddSingleton<IHashProvider, HashProvider>();

        return services;
    }

    public static IServiceCollection AddIdentityProvider(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => options.LoginPath = "/account/login");

        services.AddOpenIddict()
            .AddCore(options =>
                options.UseEntityFrameworkCore()
                    .UseDbContext<DataContext>())
            .AddServer(options =>
                options.AllowClientCredentialsFlow()
                    /*.RequireProofKeyForCodeExchange()*/
                    .SetAuthorizationEndpointUris("/connect/authorize")
                    .SetTokenEndpointUris("/connect/token")
                    .AddEphemeralEncryptionKey()
                    .AddEphemeralSigningKey()
                    .DisableAccessTokenEncryption()
                    .RegisterScopes("api")
                    .UseAspNetCore()
                    .EnableTokenEndpointPassthrough())
            .AddClient();

        services.AddHostedService<IdentityProviderDataGenerator>();

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
