using System.Security.Cryptography;
using System.Text;

namespace Cloud.Application.Utilities;

public class PasswordHasher
{
    private const int KeySize = 32;
    private const int SaltSize = 16;
    private const int Cycles = 1_572_864;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

    public static (string, string) HashPassword(string? password)
    {
        ArgumentException.ThrowIfNullOrEmpty(password, nameof(password));

        Span<byte> passwordBytes = stackalloc byte[password.Length];
        Encoding.UTF8.GetBytes(password, passwordBytes);

        Span<byte> saltBytes = stackalloc byte[SaltSize];
        RandomNumberGenerator.Fill(saltBytes);

        Span<byte> keyBytes = stackalloc byte[KeySize];
        Rfc2898DeriveBytes.Pbkdf2(passwordBytes, saltBytes, keyBytes, Cycles, Algorithm);

        return (Convert.ToBase64String(keyBytes), Convert.ToBase64String(saltBytes));
    }

    public static PasswordVerification VerifyPassword(string? password, string? key, string? salt)
    {
        ArgumentException.ThrowIfNullOrEmpty(password, nameof(password));
        ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));
        ArgumentException.ThrowIfNullOrEmpty(salt, nameof(salt));

        Span<byte> passwordBytes = stackalloc byte[password.Length];
        Encoding.UTF8.GetBytes(password, passwordBytes);

        var keyBytes = Convert.FromBase64String(key);
        var saltBytes = Convert.FromBase64String(salt);

        Span<byte> newKeyBytes = stackalloc byte[KeySize];
        Rfc2898DeriveBytes.Pbkdf2(passwordBytes, saltBytes, newKeyBytes, Cycles, Algorithm);

        if (CryptographicOperations.FixedTimeEquals(newKeyBytes, keyBytes))
        {
            return PasswordVerification.Success;
        }
        else
        {
            return PasswordVerification.Fail;
        }
    }
}

public enum PasswordVerification
{
    Success,
    Fail
}
