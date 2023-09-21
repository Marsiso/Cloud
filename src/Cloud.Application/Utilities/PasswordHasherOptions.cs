using System.Security.Cryptography;

namespace Cloud.Application.Utilities;

public class PasswordHasherOptions
{
    public required int KeySize = 32;
    public required int SaltSize = 16;
    public required int Cycles = 1_572_864;
    public required HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;
}
