namespace Cloud.Data.Application.Configurations;

using Cloud.Data.Application.Configurations.Common;
using Cloud.Domain.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : ChangeTrackingEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.ToTable(Tables.Users);

        builder.HasIndex(entity => entity.Email).IsUnique();

        builder.Property(entity => entity.Email).HasMaxLength(256);
        builder.Property(entity => entity.GivenName).HasMaxLength(256);
        builder.Property(entity => entity.FamilyName).HasMaxLength(256);
        builder.Property(entity => entity.Password).HasMaxLength(512);
        builder.Property(entity => entity.PasswordSalt).HasMaxLength(512);
        builder.Property(entity => entity.ProfilePhotoURL).HasMaxLength(2048);
    }
}
