namespace Cloud.Domain.Utilities;

using Cloud.Domain.Enums;

public interface IHashProvider
{
    (string, string) GetHash(string? value);
    HashVerificationResult VerifyHash(string? value, string? key, string? salt);
}
