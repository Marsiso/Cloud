using Cloud.Domain.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Cloud.Data;

public class DataContext : DbContext
{
    private readonly ISaveChangesInterceptor _auditor;

    public DbSet<User> Users { get; set; } = default!;

    public DataContext(DbContextOptions<DataContext> options, ISaveChangesInterceptor auditor) : base(options)
    {
        _auditor = auditor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.AddInterceptors(_auditor);
    }
}
