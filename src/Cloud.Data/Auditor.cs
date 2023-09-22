namespace Cloud.Data;

using Cloud.Domain.Application.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class Auditor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is not DataContext databaseContext)
        {
            return base.SavingChanges(eventData, result);
        }

        OnBeforeSavedChanges(databaseContext);

        return base.SavingChanges(eventData, result);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        if (eventData.Context is not DataContext context)
        {
            return base.SavedChanges(eventData, result);
        }

        OnAfterSavedChanges(context);

        return base.SavedChanges(eventData, result);
    }

    private static void OnBeforeSavedChanges(DataContext context)
    {
        context.ChangeTracker.DetectChanges();

        var dateTime = DateTime.UtcNow;

        foreach (var entityEntry in context.ChangeTracker.Entries<ChangeTrackingEntity>())
        {
            switch (entityEntry.State)
            {
                case EntityState.Added:
                    entityEntry.Entity.IsActive = true;
                    entityEntry.Entity.DateCreated = entityEntry.Entity.DateUpdated = dateTime;
                    continue;

                case EntityState.Modified:
                    entityEntry.Entity.DateUpdated = dateTime;
                    continue;

                case EntityState.Deleted:
                    throw new InvalidOperationException();
                case EntityState.Detached:
                case EntityState.Unchanged:
                    break;
                default:
                    continue;
            }
        }
    }

    private static void OnAfterSavedChanges(DataContext context)
    {
    }
}
