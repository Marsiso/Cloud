namespace Cloud.Domain.Utilities;

public interface IPasswordHasher
{
    (string, string) HashPassword(string? password);
    PasswordVerificationResult VerifyPassword(string? password, string? key, string? salt);
}
