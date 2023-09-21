using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Cloud.Data.Tests.Helpers;

public class TestDataContextWrapper : IDisposable
{
    private bool _disposed;

    public TestDataContextWrapper()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);

        var connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = ":memory:",
            Mode = SqliteOpenMode.ReadWriteCreate
        }.ToString();

        Connection = new SqliteConnection(connectionString);

        Connection.Open();

        optionsBuilder.UseSqlite(Connection);

        var auditor = new Auditor();
        var options = optionsBuilder.Options;

        Context = new DataContext(options, auditor);

        Context.Database.EnsureCreated();
    }

    public DataContext Context { get; }
    public SqliteConnection Connection { get; }

    public void Dispose()
    {
        if (!_disposed)
        {
            Context.Dispose();
            Connection.Dispose();
        }

        GC.SuppressFinalize(this);

        _disposed = true;
    }
}
