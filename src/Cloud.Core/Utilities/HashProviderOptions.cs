namespace Cloud.Core.Utilities;
using System.Security.Cryptography;

public class HashProviderOptions
{
    public const string SectionName = "Hashes";

    public readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

    public int KeySize { get; set; } = 32;
    public int SaltSize { get; set; } = 16;
    public int Cycles { get; set; } = 1_572_864;
}
