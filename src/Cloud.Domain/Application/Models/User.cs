using Cloud.Domain.Application.Models.Common;

namespace Cloud.Domain.Application.Models;

public class User : ChangeTrackingEntity
{
    public string GivenName { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool HasEmailConfirmed { get; set; }
    public string? Password { get; set; }
    public string? PasswordSalt { get; set; }
    public string? ProfilePhotoURL { get; set; }
}
