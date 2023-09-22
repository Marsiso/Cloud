namespace Cloud.Core.Utilities;

using System;
using System.Security.Cryptography;
using System.Text;
using Cloud.Domain.Enums;
using Cloud.Domain.Utilities;
using Microsoft.Extensions.Options;

public class HashProvider : IHashProvider
{
    private readonly HashProviderOptions options;

    public HashProvider(IOptions<HashProviderOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options, nameof(options));

        this.options = options.Value;
    }

    public (string, string) GetHash(string? value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value, nameof(value));

        Span<byte> valueBytes = stackalloc byte[value.Length];
        Encoding.UTF8.GetBytes(value, valueBytes);

        Span<byte> saltBytes = stackalloc byte[this.options.SaltSize];
        RandomNumberGenerator.Fill(saltBytes);

        Span<byte> keyBytes = stackalloc byte[this.options.KeySize];
        Rfc2898DeriveBytes.Pbkdf2(valueBytes, saltBytes, keyBytes, this.options.Cycles, this.options.Algorithm);

        return (Convert.ToBase64String(keyBytes), Convert.ToBase64String(saltBytes));
    }

    public HashVerificationResult VerifyHash(string? value, string? key, string? salt)
    {
        ArgumentException.ThrowIfNullOrEmpty(value, nameof(value));
        ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));
        ArgumentException.ThrowIfNullOrEmpty(salt, nameof(salt));

        Span<byte> valueBytes = stackalloc byte[value.Length];
        Encoding.UTF8.GetBytes(value, valueBytes);

        var keyBytes = Convert.FromBase64String(key);
        var saltBytes = Convert.FromBase64String(salt);

        Span<byte> newKeyBytes = stackalloc byte[this.options.KeySize];
        Rfc2898DeriveBytes.Pbkdf2(valueBytes, saltBytes, newKeyBytes, this.options.Cycles, this.options.Algorithm);

        if (CryptographicOperations.FixedTimeEquals(newKeyBytes, keyBytes))
        {
            return HashVerificationResult.Success;
        }

        return HashVerificationResult.Failed;
    }
}
