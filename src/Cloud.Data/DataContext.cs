namespace Cloud.Data;

using Cloud.Domain.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class DataContext : DbContext
{
    private readonly ISaveChangesInterceptor interceptor;

    public DbSet<User> Users { get; set; } = default!;

    public DataContext(DbContextOptions<DataContext> options, ISaveChangesInterceptor interceptor) : base(options) => this.interceptor = interceptor;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.AddInterceptors(this.interceptor);
    }
}
