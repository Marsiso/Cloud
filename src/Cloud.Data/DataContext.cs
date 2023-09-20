﻿using Cloud.Domain.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Cloud.Data;

public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; } = default!;

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}
