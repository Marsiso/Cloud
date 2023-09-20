namespace Cloud.Domain.Application.Models.Common;

public abstract class ChangeTrackingEntity : EntityBase
{
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }

    public User? UserCreatedBy { get; set; }
    public User? UserUpdatedBy { get; set; }
}
