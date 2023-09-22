namespace Cloud.Core.Tests.Utilities;

using System.Diagnostics.CodeAnalysis;
using Cloud.Core.Utilities;
using Cloud.Domain.Enums;

public class HashProviderTestSuit
{
    [Fact]
    [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Code clarity")]
    public void GetHash_WhenGivenValidValue_ThenReturnKeyAndSalt()
    {
        // Arrange.
        var hashProviderOptions = new HashProviderOptions();
        var hashProviderOptionsWrapper = new OptionsTestWrapper<HashProviderOptions>(hashProviderOptions);
        var hashProvider = new HashProvider(hashProviderOptionsWrapper);

        const string value = "valueToBeHashed";

        // Act.
        var (key, salt) = hashProvider.GetHash(value);

        // Assert.
        Assert.NotNull(key);
        Assert.NotNull(salt);
        Assert.NotEmpty(key);
        Assert.NotEmpty(salt);
    }

    [Fact]
    [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Code clarity")]
    public void VerifyHash_WhenGivenValidValue_ThenReturnSuccessResult()
    {
        // Arrange.
        var hashProviderOptions = new HashProviderOptions();
        var hashProviderOptionsWrapper = new OptionsTestWrapper<HashProviderOptions>(hashProviderOptions);
        var hashProvider = new HashProvider(hashProviderOptionsWrapper);

        const string value = "valueToBeHashed";

        // Act.
        var (key, salt) = hashProvider.GetHash(value);
        var hashVerificationResult = hashProvider.VerifyHash(value, key, salt);

        // Assert.
        Assert.Equivalent(HashVerificationResult.Success, hashVerificationResult);
    }
}
