using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace Cloud.Application.Utilities;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasherOptions _options;

    public PasswordHasher(IOptions<PasswordHasherOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options, nameof(options));

        _options = options.Value;
    }

    public (string, string) HashPassword(string? password)
    {
        ArgumentException.ThrowIfNullOrEmpty(password, nameof(password));

        Span<byte> passwordBytes = stackalloc byte[password.Length];
        Encoding.UTF8.GetBytes(password, passwordBytes);

        Span<byte> saltBytes = stackalloc byte[_options.SaltSize];
        RandomNumberGenerator.Fill(saltBytes);

        Span<byte> keyBytes = stackalloc byte[_options.KeySize];
        Rfc2898DeriveBytes.Pbkdf2(passwordBytes, saltBytes, keyBytes, _options.Cycles, _options.Algorithm);

        return (Convert.ToBase64String(keyBytes), Convert.ToBase64String(saltBytes));
    }

    public PasswordVerificationResult VerifyPassword(string? password, string? key, string? salt)
    {
        ArgumentException.ThrowIfNullOrEmpty(password, nameof(password));
        ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));
        ArgumentException.ThrowIfNullOrEmpty(salt, nameof(salt));

        Span<byte> passwordBytes = stackalloc byte[password.Length];
        Encoding.UTF8.GetBytes(password, passwordBytes);

        var keyBytes = Convert.FromBase64String(key);
        var saltBytes = Convert.FromBase64String(salt);

        Span<byte> newKeyBytes = stackalloc byte[_options.KeySize];
        Rfc2898DeriveBytes.Pbkdf2(passwordBytes, saltBytes, newKeyBytes, _options.Cycles, _options.Algorithm);

        if (CryptographicOperations.FixedTimeEquals(newKeyBytes, keyBytes))
        {
            return PasswordVerificationResult.Success;
        }
        else
        {
            return PasswordVerificationResult.Fail;
        }
    }
}
