namespace Cloud.Data.Application.Configurations.Common;

using Cloud.Domain.Application.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public abstract class ChangeTrackingEntityConfiguration<TEntity> : EntityBaseConfiguration<TEntity> where TEntity : ChangeTrackingEntity
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        builder.Property(entity => entity.DateCreated)
            .HasDefaultValueSql("datetime('now')")
            .ValueGeneratedOnAdd();

        builder.Property(entity => entity.DateUpdated)
            .HasDefaultValueSql("datetime('now')")
            .ValueGeneratedOnAddOrUpdate();

        builder.HasOne(entity => entity.UserCreatedBy)
            .WithMany()
            .HasForeignKey(entity => entity.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(entity => entity.UserUpdatedBy)
            .WithMany()
            .HasForeignKey(entity => entity.UpdatedBy)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
