namespace Cloud.Domain.Application.Models.Common;

public abstract class EntityBase
{
    public int ID { get; set; }
    public bool IsActive { get; set; }
}
