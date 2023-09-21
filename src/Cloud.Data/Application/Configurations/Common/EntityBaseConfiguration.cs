using Cloud.Domain.Application.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cloud.Data.Application.Configurations.Common;

public abstract class EntityBaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntityBase
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(entity => entity.ID);

        builder.Property(entity => entity.ID)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.HasQueryFilter(entity => entity.IsActive);
    }
}
